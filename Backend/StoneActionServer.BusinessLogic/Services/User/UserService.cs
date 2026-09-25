using StoneActionServer.DAL.DTO;

namespace StoneActionServer.BusinessLogic.Services.Player;

using Microsoft.Extensions.Logging;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task AddAsync(User user)
    {
        await _userRepository.Add(user);
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }
    
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _userRepository.GetByUserName(userName);
    }

    public List<User> GetAllUsers()
    {
        return _userRepository.GetAllUsers();
    }

    public async Task UpdateAsync(User user)
    {
        await _userRepository.Update(user);
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        var deleted = await _userRepository.Delete(userId);
        return deleted;
    }
}
