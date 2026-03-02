namespace StoneActionServer.DAL.Models;

public class TradeSlot
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ItemId { get; set; }
    public int Price { get; set; }
    
    public Item Item { get; set; }
    public User User { get; set; }
}