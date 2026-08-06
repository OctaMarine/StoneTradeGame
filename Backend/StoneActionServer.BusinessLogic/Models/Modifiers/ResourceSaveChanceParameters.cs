namespace StoneActionServer.BusinessLogic.Models.Modifiers;

public class ResourceSaveChanceParameters : BaseModifierParameters
{
    public int ResourceItemId { get; set; }
    public float SaveChance { get; set; } // 0.1 = 10% шанс не потратить ресурс
}
