using SharedTrip.ViewModels.Users;

namespace SharedTrip.Services.Users
{
    public interface IUserService
    {
        string GetUserId(string username, string password);

        void Create(UserInputModel model);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}
