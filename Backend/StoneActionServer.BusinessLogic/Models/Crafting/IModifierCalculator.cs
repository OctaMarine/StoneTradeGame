using StoneActionServer.BusinessLogic.Models.Modifiers;
using StoneActionServer.BusinessLogic.Services;
using StoneActionServer.DAL.Repositories.Crafting.Models;

namespace StoneActionServer.BusinessLogic.Models.Crafting;

public interface IModifierCalculator
{
    bool CanHandle(string modifierType);
    void Apply(CraftingContext context, BaseModifierParameters parameters);
    }

