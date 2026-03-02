using System.ComponentModel.DataAnnotations.Schema;

namespace StoneActionServer.DAL.Models;

public class Inventory
{
    public Guid Id { get; set; }
    public int Coins { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<SlotInventory> Slots { get; set; }
}