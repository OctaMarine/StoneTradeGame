namespace StoneActionServer.DAL.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentSkillId { get; set; }

    public Skill? ParentSkill { get; set; }

    public List<Skill> ChildrenSkills { get; set; } = new();
}
