namespace StoneActionServer.DAL.Repositories.Crafting.Models;

public class CraftingContext
{
    public int UserId { get; set; }
    public int RecipeId { get; set; }
    public float ChanceOfSuccess { get; set; }
    
    public int ResultItemId { get; set; }
    public int ResultQuantity { get; set; }
    public Dictionary<int, int> BaseRequiredMaterials { get; set; } = new();
    
    public Dictionary<int, int> FinalRequiredMaterials { get; set; } = new();
    public List<int> ExtraItemIds { get; set; } = new();
}
