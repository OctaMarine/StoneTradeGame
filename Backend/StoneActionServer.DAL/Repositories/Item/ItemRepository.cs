using Microsoft.EntityFrameworkCore;

namespace StoneActionServer.DAL.Repositories.Item;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;
    
    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Models.Item?> GetById(int itemId)
    {
        return _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);
    }
}