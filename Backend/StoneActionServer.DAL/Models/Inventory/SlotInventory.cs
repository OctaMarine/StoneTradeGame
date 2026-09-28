namespace StoneActionServer.DAL.Models;

public class SlotInventory
{
    public int Id { get; set; }
    public int InventoryId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    
    public Inventory Inventory { get; set; }
    public Item Item { get; set; }
}