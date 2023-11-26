namespace UserDataManagingService.Models.Repositories
{
    public interface IUserRepository
    {
        Task<(bool, User user)> UserNickNameExistAlready(string inputedNickName);
        //Task<(bool, User user)> UserIdExist(Guid id);
        User CreateUser(string newName, string newLastName, string newNickName, string password, string personalCode, string phoneNr, string email);
        Task<User> AvatarIdMappingToUserId(Guid userId, Avatar avatar);
        //Task<User> UserById(Guid id);

        Task<Guid> GetUserIdByNickname(string nickName);
        Task<User> GetFullUserByNickname(string nickName);
        Task<User> GetFullUserById(Guid userId);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> DeactivateUser(Guid userId);
        Task<bool> UserDataAreNotNullOrWihteSpaceAndMapped(User user);
        Task<bool> GetUserActiveStatusByUserId(Guid userId);
        Task<string> GetUserRoleById(Guid userId);
        Guid ConvertStringToGuid(string anyString);
        (byte[], byte[]) CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}
