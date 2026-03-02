namespace StoneActionServer.DAL.Models;

public class SlotInventory
{
    public Guid Id { get; set; }
    public Guid InventoryId { get; set; }
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    
    public Inventory Inventory { get; set; }
    public Item Item { get; set; }
}