namespace StoneActionServer.DAL.Models;

public class SkillCraftRecipe
{
    public int CraftRecipeId { get; set; }
    public int SkillId { get; set; }
    
    public float LevelProgressReward { get; set; }
    
    public CraftingRecipe? CraftingRecipe { get; set; }
    public Skill? Skill { get; set; }
}
