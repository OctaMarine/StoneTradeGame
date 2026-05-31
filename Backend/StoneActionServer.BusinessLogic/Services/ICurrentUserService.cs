namespace StoneActionServer.BusinessLogic.Services;

public interface ICurrentUserService
{
    int UserId { get; }
    bool IsAuthenticated { get; }
}