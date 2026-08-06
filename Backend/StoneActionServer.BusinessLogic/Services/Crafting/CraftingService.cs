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
            IModifierRepository modifierRepository)
        {
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
                if (chance > craftingContext.ChanceOfSuccess)
                {
                    // При провале всё равно списываем материалы
                    var consumed = await _craftingRepository.ConsumeMaterials(userId, craftingRecipeId);
                    return consumed; // Возвращаем true, если материалы списаны
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
    public class CraftingServiceasd : ICraftingService
    {
        private readonly ICraftingRepository _craftingRepository;
        private readonly ISkillModifierRepository _modifierRepository;
        private readonly IEnumerable<IModifierCalculator> _calculators;
        private readonly ILogger<CraftingServiceasd> _logger;
        private readonly Random _random;

        public CraftingServiceasd(
            ICraftingRepository craftingRepository,
            ISkillModifierRepository modifierRepository,
            IEnumerable<IModifierCalculator> calculators,
            ILogger<CraftingServicedsa> logger,
            Random random = null)
        {
            _craftingRepository = craftingRepository;
            _modifierRepository = modifierRepository;
            _calculators = calculators;
            _logger = logger;
            _random = random ?? new Random();
        }

        public async Task<bool> PerformCrafting(int userId, int craftingRecipeId)
        {
            _logger.LogInformation("Начало крафта: UserId={UserId}, RecipeId={RecipeId}", userId, craftingRecipeId);

            try
            {
                // 1. Проверяем возможность крафта (материалы, доступность рецепта)
                var canCraft = await _craftingRepository.CanCraftRecipe(userId, craftingRecipeId);
                if (!canCraft)
                {
                    _logger.LogWarning("Крафт невозможен: UserId={UserId}, RecipeId={RecipeId}", userId, craftingRecipeId);
                    return false;
                }

                // 2. Создаём контекст крафта для модификаторов
                var context = await InitializeContextAsync(userId, craftingRecipeId);
                if (context == null)
                {
                    _logger.LogWarning("Не удалось создать контекст крафта: RecipeId={RecipeId}", craftingRecipeId);
                    return false;
                }

                // 3. Проверяем шанс успеха крафта
                if (_random.NextDouble() > context.ChanceOfSuccess)
                {
                    _logger.LogInformation("Крафт провалился: UserId={UserId}, RecipeId={RecipeId}", userId, craftingRecipeId);
                    
                    // При провале всё равно списываем материалы (или нет - зависит от геймдизайна)
                    var consumed = await _craftingRepository.ConsumeMaterials(userId, craftingRecipeId);
                    return consumed; // Возвращаем true, если материалы списаны
                }

                // 4. Получаем активные модификаторы
                var activeModifiers = await _modifierRepository.GetActiveModifiersAsync(userId, craftingRecipeId);

                // 5. Применяем модификаторы
                foreach (var modifier in activeModifiers)
                {
                    try
                    {
                        var calculator = _calculators.FirstOrDefault(c => c.CanHandle(modifier.ModifierType));
                        if (calculator != null)
                        {
                            var typedParams = modifier.GetTypedParameters();
                            calculator.Apply(context, typedParams, _random);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка при применении модификатора: ModifierType={ModifierType}", modifier.ModifierType);
                    }
                }

                // 6. Выполняем транзакцию
                await using var transaction = await _craftingRepository.BeginTransactionAsync();
                try
                {
                    // Списываем материалы (с учётом экономии из модификаторов)
                    var isConsumed = await _craftingRepository.ConsumeMaterials(
                        userId, 
                        context.FinalRequiredMaterials);
                    
                    if (!isConsumed)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    // Добавляем основной предмет
                    var finalQuantity = (int)Math.Floor(context.FinalQuantity);
                    if (finalQuantity > 0)
                    {
                        await _craftingRepository.AddItemToInventory(
                            userId,
                            context.FinalResultItemId,
                            finalQuantity);
                    }

                    // Добавляем бонусные предметы
                    foreach (var bonusItemId in context.BonusItemIds)
                    {
                        await _craftingRepository.AddItemToInventory(userId, bonusItemId, 1);
                    }

                    // Обновляем прогресс навыков
                    await _craftingRepository.UpdateSkillProgress(userId, craftingRecipeId);

                    await transaction.CommitAsync();

                    _logger.LogInformation("Крафт успешен: UserId={UserId}, RecipeId={RecipeId}, MainItem={ItemId}, Quantity={Quantity}",
                        userId, craftingRecipeId, context.FinalResultItemId, finalQuantity);

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Ошибка при исполнении крафта: UserId={UserId}", userId);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Критическая ошибка при крафте: UserId={UserId}, RecipeId={RecipeId}", userId, craftingRecipeId);
                return false;
            }
        }
        
    }
}
}