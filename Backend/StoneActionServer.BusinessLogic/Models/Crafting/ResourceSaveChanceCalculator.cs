using StoneActionServer.BusinessLogic.Models.Modifiers;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.DAL.Models.Modifiers;
using StoneActionServer.DAL.Repositories.Crafting.Models;

namespace StoneActionServer.BusinessLogic.Models.Crafting;

public class ResourceSaveChanceCalculator : IModifierCalculator
{
    public bool CanHandle(string modifierType) => 
        modifierType == ModifierTypes.ResourceSaveChance;

    public void Apply(CraftingContext context, BaseModifierParameters parameters)
    {
        var saveParams = (ResourceSaveChanceParameters)parameters;
        
        if (context.FinalRequiredMaterials.TryGetValue(saveParams.ResourceItemId, out int quantity))
        {
            if (Random.Shared.NextDouble() <= saveParams.SaveChance)
            {
                // Экономим 1 единицу ресурса
                context.FinalRequiredMaterials[saveParams.ResourceItemId] = Math.Max(0, quantity - 1);
            }
        }
    }
}
