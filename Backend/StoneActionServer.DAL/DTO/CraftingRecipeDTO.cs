namespace StoneActionServer.DAL.DTO;

public class CraftingRecipeDTO
{
    public int Id { get; set; }
    public int ResultItemId { get; set; }
    public int ResultQuantity { get; set; }
    public double ChanceOfSuccess { get; set; }
    public int CraftingTimeSeconds { get; set; }
    public int CraftingType { get; set; }
    public List<CraftingIngredientDTO> RequiredItems { get; set; }

}