namespace StoneActionServer.DAL.Repositories.Item;

public interface IItemRepository
{
    public Task<Models.Item?> GetByIdAsync(int itemId);
}