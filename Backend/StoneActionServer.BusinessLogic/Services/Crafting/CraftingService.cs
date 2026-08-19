using System.Text.Json;
using Microsoft.Extensions.Logging;
using StoneActionServer.BusinessLogic.Models.Crafting;
using StoneActionServer.BusinessLogic.Models.Modifiers;
using StoneActionServer.DAL.DTO;
using StoneActionServer.DAL.Models.Modifiers;
using StoneActionServer.DAL.Repositories;
using StoneActionServer.DAL.Repositories.Crafting.Models;
using StoneActionServer.DAL.Repositories.Modifiers;

namespace StoneActionServer.BusinessLogic.Services
{
    public class CraftingService : ICraftingService
    {
        private readonly ICraftingRepository _craftingRepository;
        private readonly IModifierRepository _modifierRepository;
        private readonly IEnumerable<IModifierCalculator> _calculators;
        
        public CraftingService(ICraftingRepository craftingRepository,
            IModifierRepository modifierRepository,
            IEnumerable<IModifierCalculator> calculators)
        {
            _calculators = calculators;
            _craftingRepository = craftingRepository;
            _modifierRepository = modifierRepository;
            
        }

        public async Task<bool> PerformCrafting(int userId, int craftingRecipeId)
        {
            try
            {
                var canCraft = await _craftingRepository.CanCraftRecipe(userId, craftingRecipeId);
                if (!canCraft)
                {
                    return false;
                }
                
                var recipes = await _craftingRepository.GetRecipes();
                var recipe = recipes.FirstOrDefault(x => x.Id == craftingRecipeId);
                if (recipe == null)
                    return false;
    
                var craftingContext = new CraftingContext
                {
                    UserId = userId,
                    RecipeId = craftingRecipeId,
                    ChanceOfSuccess = recipe.ChanceOfSuccess,
                    ResultItemId = recipe.ResultItemId,
                    ResultQuantity = recipe.ResultQuantity,
                    BaseRequiredMaterials = recipe.RequiredItems.ToDictionary(m => m.ItemId, m => m.Quantity),
                    FinalRequiredMaterials = recipe.RequiredItems.ToDictionary(m => m.ItemId, m => m.Quantity)
                };

                var activeModifiers = await _modifierRepository.GetActiveModifiersAsync(userId, craftingRecipeId);
                
                foreach (var modifier in activeModifiers)
                {
                    try
                    {
                        var calculator = _calculators.FirstOrDefault(c => c.CanHandle(modifier.ModifierType));
                        if (calculator != null)
                        {
                            var typedParams = GetTypedParameters(modifier);
                            calculator.Apply(craftingContext, typedParams);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Ошибка при применении модификатора: ModifierType={ModifierType}", modifier.ModifierType);
                        return false;
                    }
                }
                
                var chance = Random.Shared.NextDouble();
                if (craftingContext.ChanceOfSuccess < chance)
                {
                    // При провале всё равно списываем материалы
                    var consumed = await _craftingRepository.ConsumeMaterials(userId, craftingRecipeId);
                    return false; // Возвращаем true, если материалы списаны
                }
                
                var isConsume =  await _craftingRepository.ConsumeMaterials(userId, craftingRecipeId);
                if (!isConsume)
                {
                    return false;
                }
                
                await _craftingRepository.AddCraftedItemByContext(userId, craftingContext);
    
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }
        
        private BaseModifierParameters GetTypedParameters(Modifier modifier)
        {
            if (modifier.ModifierType == ModifierTypes.YieldMultiplier)
            {
                var param = JsonSerializer.Deserialize<YieldMultiplierParameters>(modifier.Param);
                return param;
            }

            if (modifier.ModifierType == ModifierTypes.ChanceMultiplier)
            {
                var param = JsonSerializer.Deserialize<ChanceMultiplierParameters>(modifier.Param);
                return param;
            }
            if (modifier.ModifierType == ModifierTypes.ChanceToDropExtraItem && !string.IsNullOrEmpty(modifier.Param))
            {
                var param = JsonSerializer.Deserialize<ChanceToDropExtraItemParameters>(modifier.Param);
                return param;
            }
            if (modifier.ModifierType == ModifierTypes.ResourceSaveChance && !string.IsNullOrEmpty(modifier.Param))
            {
                var param = JsonSerializer.Deserialize<ResourceSaveChanceParameters>(modifier.Param);
                return param;
            }

            return null;
        }

        public async Task<List<CraftingRecipeDTO>> GetRecipes()
        {
            return await _craftingRepository.GetRecipes();
        }
    }
}