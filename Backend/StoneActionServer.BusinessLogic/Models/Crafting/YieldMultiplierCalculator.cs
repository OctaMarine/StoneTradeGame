using StoneActionServer.BusinessLogic.Models.Modifiers;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.DAL.Models.Modifiers;
using StoneActionServer.DAL.Repositories.Crafting.Models;

namespace StoneActionServer.BusinessLogic.Models.Crafting;

public class YieldMultiplierCalculator : IModifierCalculator
{
    public bool CanHandle(string modifierType) => 
        modifierType == ModifierTypes.YieldMultiplier;

    public void Apply(CraftingContext context, BaseModifierParameters parameters)
    {
        var yieldParams = (YieldMultiplierParameters)parameters;
        float preQuantity = (float)context.ResultQuantity;
        preQuantity *= yieldParams.Multiplier;
        context.ResultQuantity = (int) MathF.Round(preQuantity, 0, MidpointRounding.AwayFromZero);
    }
}
