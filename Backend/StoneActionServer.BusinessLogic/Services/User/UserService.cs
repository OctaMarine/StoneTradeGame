using StoneActionServer.DAL;
using StoneActionServer.DAL.DTO;

namespace StoneActionServer.BusinessLogic.Services.Player;

using Microsoft.Extensions.Logging;
using StoneActionServer.DAL.Models;
using StoneActionServer.DAL.Repositories;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task AddAsync(User user)
    {
        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }
    
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _userRepository.GetByUserNameAsync(userName);
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int userId)
    {
        var isDeleted = await _userRepository.DeleteAsync(userId);
        await _unitOfWork.SaveChangesAsync();
        return isDeleted;
    }
}
