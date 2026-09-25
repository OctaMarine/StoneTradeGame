using StoneActionServer.DAL.Models;

namespace StoneActionServer.BusinessLogic.Services.Player;

public interface IUserService
{
    public Task AddAsync(User user);
    
    public Task<User?> GetByIdAsync(int userId);
    
    public Task<User?> GetByUserNameAsync(string userName);
    
    public List<User> GetAllUsers();
    
    public Task UpdateAsync(User user);
    
    public Task<bool> DeleteAsync(int userId);
}