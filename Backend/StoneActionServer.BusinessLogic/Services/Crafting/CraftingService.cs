using System.Text.Json;
using StoneActionServer.BusinessLogic.Models.Crafting;
using StoneActionServer.BusinessLogic.Models.Modifiers;
using StoneActionServer.DAL;
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
        private readonly IInventoryService _inventoryService;
        private readonly IUnitOfWork _unitOfWork;
        
        public CraftingService(ICraftingRepository craftingRepository,
            IModifierRepository modifierRepository,
            IEnumerable<IModifierCalculator> calculators,
            IInventoryService inventoryService,
            IUnitOfWork unitOfWork)
        {
            _calculators = calculators;
            _craftingRepository = craftingRepository;
            _modifierRepository = modifierRepository;
            _inventoryService = inventoryService;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<bool> CanCraftRecipe(int userId, Dictionary<int,int> requiredItems)
        {
            var items = await _inventoryService.GetUserItemsAsync(userId);
            if (items == null)
            {
                throw new InvalidOperationException($"Инвентарь пользователя не найден.");
            }

            bool canCraft = requiredItems.All(requiredItem => 
                items.Any(userItem => 
                    userItem.Id == requiredItem.Key && 
                    userItem.Quantity >= requiredItem.Value)
            );

            return canCraft;

        }
        
        public async Task<bool> TryConsumeMaterials(int userId, Dictionary<int,int> requiredItems)
        {
            var userItems = await _inventoryService.GetUserItemsAsync(userId);
            
            if (userItems == null)
            {
                throw new Exception("Инвентарь не найден");
            }
            
            bool canCraft = requiredItems.All(requiredItem => 
                userItems.Any(userItem => 
                    userItem.Id == requiredItem.Key && 
                    userItem.Quantity >= requiredItem.Value)
            );

            if (!canCraft)
            {
                return false;
            }

            foreach (var req in requiredItems)
            {
                await _inventoryService.RemoveItem(userId, req.Key, req.Value);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        
        private async Task<CraftingContext?> PrepareCraftingContextAsync(
            int userId,
            int craftingRecipeId)
        {
            var recipe = await _craftingRepository
                .GetCraftingRecipeAsync(craftingRecipeId);

            if (recipe == null)
                return null;

            var context = new CraftingContext
            {
                UserId = userId,
                RecipeId = craftingRecipeId,
                ChanceOfSuccess = recipe.ChanceOfSuccess,
                ResultItemId = recipe.ResultItemId,
                ResultQuantity = recipe.ResultQuantity,
                BaseRequiredMaterials = recipe.RequiredItems
                    .ToDictionary(x => x.ItemId, x => x.Quantity),
                FinalRequiredMaterials = recipe.RequiredItems
                    .ToDictionary(x => x.ItemId, x => x.Quantity)
            };

            var activeModifiers = await _modifierRepository
                .GetActiveModifiersAsync(userId, craftingRecipeId);

            foreach (var modifier in activeModifiers)
            {
                var calculator = _calculators
                    .FirstOrDefault(x => x.CanHandle(modifier.ModifierType));

                if (calculator == null)
                    continue;

                var typedParams = GetTypedParameters(modifier);
                calculator.Apply(context, typedParams);
            }

            return context;
        }

        public async Task<bool> PerformCrafting(int userId, int craftingRecipeId)
        {
            try
            {
                var craftingContext = await PrepareCraftingContextAsync(userId, craftingRecipeId);
                if (craftingContext == null)
                    return false;
                
                var isConsume =  await TryConsumeMaterials(userId, craftingContext.FinalRequiredMaterials);
                
                if (!isConsume)
                    return false;
                
                var chance = Random.Shared.NextDouble();
                if (craftingContext.ChanceOfSuccess < chance)
                {
                    return false; 
                }
                
                if (!isConsume)
                {
                    return false;
                }
                
                await _craftingRepository.AddCraftedItemByContext(userId, craftingContext);
    
                return true;
            }
            catch (Exception e)
            {
                throw;
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

        public async Task<List<CraftingRecipeDTO>> GetAllRecipesAsync()
        {
            return await _craftingRepository.GetAllRecipesAsync();
        }
    }
}