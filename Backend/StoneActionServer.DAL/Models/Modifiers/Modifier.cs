namespace StoneActionServer.DAL.Models.Modifiers;

public class Modifier
{
    public int Id { get; set; }
    public int SkillId { get; set; }
    public Skill Skill { get; set; }
    
    public int? CraftRecipeId { get; set; }
    public CraftingRecipe? CraftRecipe { get; set; }
    
    public int RequiredSkillLevel { get; set; } = 1;
    public string ModifierType { get; set; } = string.Empty;
    public string? Param { get; set; }
}
