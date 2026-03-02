namespace StoneActionServer.DAL.Models;

public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<SlotInventory> Slots { get; set; }
    public ICollection<TradeSlot> TradeSlots { get; set; }
}