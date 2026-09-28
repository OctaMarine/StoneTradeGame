using Microsoft.EntityFrameworkCore;

namespace StoneActionServer.DAL.Repositories.Item;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;
    
    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Models.Item?> GetByIdAsync(int itemId)
    {
        return await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);
    }
}