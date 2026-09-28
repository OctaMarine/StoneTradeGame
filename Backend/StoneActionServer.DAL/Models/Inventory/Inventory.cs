using System.ComponentModel.DataAnnotations.Schema;

namespace StoneActionServer.DAL.Models;

public class Inventory
{
    public int Id { get; set; }
    public int Coins { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<SlotInventory> Slots { get; set; }
}