namespace StoneActionServer.BusinessLogic.Models.Modifiers;

public class ChanceToDropExtraItemParameters : BaseModifierParameters
{
    public int ItemId { get; set; }
    public float Chance { get; set; } // 0.05 = 5% шанс
}
