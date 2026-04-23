namespace StoneActionServer.DAL.Models;

public class CraftingIngredient
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    
    public int CraftingRecipeId { get; set; }
    public CraftingRecipe CraftingRecipe { get; set; }
    public Item Item { get; set; }
    
}