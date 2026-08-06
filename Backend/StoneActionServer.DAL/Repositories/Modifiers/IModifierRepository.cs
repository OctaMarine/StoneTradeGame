using StoneActionServer.DAL.Models.Modifiers;

namespace StoneActionServer.DAL.Repositories.Modifiers;

public interface IModifierRepository
{
    Task<List<Modifier>> GetActiveModifiersAsync(int userId, int recipeId);
}
