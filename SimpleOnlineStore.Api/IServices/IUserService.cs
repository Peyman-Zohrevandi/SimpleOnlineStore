namespace SimpleOnlineStore.Api.IServices
{
    public interface IUserService
    {
        Task ValidateUserAsync(int userId, CancellationToken cancellationToken);
    }
}
