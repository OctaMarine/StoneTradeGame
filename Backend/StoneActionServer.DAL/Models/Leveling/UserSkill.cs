using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Models;

public class UserSkill
{
    public int UserId { get; set; }
    public int SkillId { get; set; }

    public bool IsOpen { get; set; }
    public bool IsAvailable { get; set; }
    public int CurrentLevel { get; set; }
    public float Progress { get; set; }
    
    public User? User { get; set; }
    public Skill? Skill { get; set; }
}
