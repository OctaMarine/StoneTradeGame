using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoneActionServer.DAL.Models.Modifiers;
using StoneActionServer.DAL.Repositories.Modifiers;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories.Modifiers;

public class ModifierRepository : IModifierRepository
{
    private readonly AppDbContext _context;

    public ModifierRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Modifier>> GetActiveModifiersAsync(int userId, int recipeId)
    {
        var modifiers = await _context.Modifiers.AsNoTracking()
            .Include(m => m.Skill)
            .Include(m => m.CraftRecipe)
            .Where(m => m.CraftRecipeId == null || m.CraftRecipeId == recipeId)
            .Where(m => _context.UserSkills.Any(us => 
                us.UserId == userId && 
                us.SkillId == m.SkillId && 
                us.CurrentLevel >= m.RequiredSkillLevel))
            .ToListAsync();

        return modifiers;
    }
}
