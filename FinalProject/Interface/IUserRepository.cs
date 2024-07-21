using FinalProject.Entities;

namespace FinalProject.Interface
{
    public interface IUserRepository
    {
        Task<User> FindUserByEmail(string email);
        Task ResetPassword(User user, string newPassword);
    }
}
