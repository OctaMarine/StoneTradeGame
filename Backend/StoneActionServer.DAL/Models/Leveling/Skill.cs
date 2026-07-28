namespace StoneActionServer.DAL.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconFileName { get; set; }
    
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int MaxLevel { get; set; } = 1;
    
    public int? ParentSkillId { get; set; }
    public Skill? ParentSkill { get; set; }
    public List<Skill> ChildrenSkills { get; set; } = new();
    
    public List<SkillCraftRecipe> CraftRecipes { get; set; } = new();
}