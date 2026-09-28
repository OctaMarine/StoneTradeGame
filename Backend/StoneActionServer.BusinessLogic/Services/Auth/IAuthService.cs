using StoneActionServer.DAL.Models;

namespace StoneActionServer.BusinessLogic.Services;

public interface IAuthService
{
    public Task<bool> RegisterAsync(string userName, string password, string email, CancellationToken cancellationToken = default);

    public Task<string> LoginAsync(string userName, string password);
    public Task<List<User>> GetAllUsersAsync();
}