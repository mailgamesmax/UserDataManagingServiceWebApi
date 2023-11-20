namespace UserDataManagingService.Models.Repositories
{
    public interface IUserRepository
    {
        Task<(bool, User user)> UserNickNameExistAlready(string inputedNickName);
        User CreateUser(string newName, string newLastName, string newNickName, string password, string personalCode, string phoneNr, string email);
        Task<User> AvatarIdMappingToUserId(Guid userId, Avatar avatar);
        //Task<User> UserById(Guid id);

        Task<Guid> GetUserIdByNickname(string nickName);
        Task<User> GetFullUserByNickname(string nickName);
    }
}
