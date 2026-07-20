namespace StoneActionServer.DAL.DTO.Leveling;

public class UserSkillNodeDTO
{
    public int SkillId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public int? ParentSkillId { get; set; }
    public int CurrentLevel { get; set; }
    public List<UserSkillNodeDTO> Children { get; set; } = new();
}