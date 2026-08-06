using StoneActionServer.BusinessLogic.Models.Modifiers;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.DAL.Models.Modifiers;
using StoneActionServer.DAL.Repositories.Crafting.Models;

namespace StoneActionServer.BusinessLogic.Models.Crafting;

public class ChanceToDropExtraItemCalculator : IModifierCalculator
{
    public bool CanHandle(string modifierType) => 
        modifierType == ModifierTypes.ChanceToDropExtraItem;

    public void Apply(CraftingContext context, BaseModifierParameters parameters)
    {
        var dropParams = (ChanceToDropExtraItemParameters)parameters;
        
        if (Random.Shared.NextSingle() <= dropParams.Chance)
        {
            context.ExtraItemIds.Add(dropParams.ItemId);
        }
    }
}
