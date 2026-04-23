namespace StoneActionServer.DAL.Models;

public class TradeSlot
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public int Price { get; set; }
    
    public Item Item { get; set; }
    public User User { get; set; }
}