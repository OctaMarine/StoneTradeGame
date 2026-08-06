using StoneActionServer.BusinessLogic.Models.Modifiers;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.DAL.Models.Modifiers;
using StoneActionServer.DAL.Repositories.Crafting.Models;

namespace StoneActionServer.BusinessLogic.Models.Crafting;

public class ChanceMultiplierCalculator : IModifierCalculator
{
    public bool CanHandle(string modifierType) => 
        modifierType == ModifierTypes.ChanceMultiplier;

    public void Apply(CraftingContext context, BaseModifierParameters parameters)
    {
        var param = (ChanceMultiplierParameters)parameters;
        context.ChanceOfSuccess *= param.Multiplier;
    }
}