namespace StoneActionServer.DAL.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<SlotInventory> Slots { get; set; }
    public ICollection<TradeSlot> TradeSlots { get; set; }
    public ICollection<CraftingIngredient> CraftingIngredient { get; set; }
}