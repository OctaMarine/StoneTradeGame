namespace StoneActionServer.DAL.DTO.Leveling;

public class UserSkillNodeDTO
{
    public int SkillId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconFileName { get; set; } // Название файла, например "hammer.png"
    public int? ParentSkillId { get; set; }
    public int CurrentLevel { get; set; }
    public int MaxLevel { get; set; }
    public float Progress { get; set; } // 0.0 to 100.0
    public bool IsOpen { get; set; }
    public bool IsAvailable { get; set; }
    public int? PositionX { get; set; }
    public int? PositionY { get; set; }
    
    public List<UserSkillNodeDTO> Children { get; set; } = new();
}