namespace UserDataManagingService.Models.Repositories
{
    public interface IUserRepository
    {
        User CreateUser(string newName, string newLastName, string newNickName, string password, string personalCode, string phoneNr, string email);

        //Task<User> UserById(Guid id);

        Task<Guid> GetUserIdByNickname(string nickName);
        Task<User> GetFullUserByNickname(string nickName);
    }
}
