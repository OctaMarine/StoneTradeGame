using Microsoft.EntityFrameworkCore;
using StoneActionServer.DAL.DTO.Leveling;
using StoneActionServer.DAL.Models;

namespace StoneActionServer.DAL.Repositories.Leveling;

public class LevelingRepository : ILevelingRepository
{
    private readonly AppDbContext _context;

    public LevelingRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Возвращает плоский список навыков, которые пользователь видит (IsOpen = true),
    /// вместе с его текущим прогрессом.
    /// </summary>
    public async Task<List<UserSkillNodeDTO>> GetUserSkillsFlatAsync(int userId)
    {
        // 1. Сначала получаем только прогресс пользователя (быстрый запрос)
        var userSkills = await _context.Set<UserSkill>()
            .AsNoTracking()
            .Where(us => us.UserId == userId && us.IsOpen)
            .ToListAsync();

        if (!userSkills.Any())
        {
            return new List<UserSkillNodeDTO>();
        }

        // 2. Получаем только те навыки, которые нужны
        var skillIds = userSkills.Select(us => us.SkillId).ToList();
        var skills = await _context.Skills
            .AsNoTracking()
            .Where(s => skillIds.Contains(s.Id))
            .ToListAsync();

        // 3. Собираем результат в памяти
        var skillDict = skills.ToDictionary(s => s.Id);
        var result = userSkills.Select(us => new UserSkillNodeDTO
        {
            SkillId = us.SkillId,
            SkillName = skillDict[us.SkillId].Name,
            ParentSkillId = skillDict[us.SkillId].ParentSkillId,
            CurrentLevel = us.CurrentLevel
        }).ToList();

        return result;
    }

    /// <summary>
    /// Повышает уровень навыка пользователя на 1.
    /// Проверяет, что навык доступен (IsAvailable = true) и пререквизиты выполнены.
    /// </summary>
    public async Task<bool> UpgradeSkillAsync(int userId, int skillId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. Получаем навык
            var skill = await _context.Skills
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == skillId);

            if (skill == null)
            {
                return false;
            }

            // 2. Получаем или создаем запись UserSkill
            var userSkill = await _context.Set<UserSkill>()
                .FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId);

            if (userSkill == null)
            {
                // Навык еще не открыт у пользователя
                return false;
            }

            // 3. Проверяем, что навык доступен для прокачки
            if (!userSkill.IsAvailable)
            {
                return false;
            }

            // 4. Проверяем пререквизит: если у навыка есть родитель, его уровень должен быть >= 1
            if (skill.ParentSkillId.HasValue)
            {
                var parentUserSkill = await _context.Set<UserSkill>()
                    .FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skill.ParentSkillId.Value);

                if (parentUserSkill == null || parentUserSkill.CurrentLevel < 1)
                {
                    return false;
                }
            }

            // 5. Повышаем уровень
            userSkill.CurrentLevel += 1;
            _context.Set<UserSkill>().Update(userSkill);

            // 6. Сохраняем изменения
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}