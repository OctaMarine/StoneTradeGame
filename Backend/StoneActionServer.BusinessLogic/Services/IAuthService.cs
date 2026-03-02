using StoneActionServer.DAL.Models;

namespace StoneActionServer.BusinessLogic.Services;

public interface IAuthService
{
    public Task<bool> Register(string userName, string password, string email, CancellationToken cancellationToken = default);

    public Task<string> Login(string userName, string password);
    public Task<List<User>> GetAllUsers();
}