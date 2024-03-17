namespace Edge.Services.Interfaces
{
    public interface IPasswordEncryptionService
    {
        string EncryptPassword(string password);
        string DecryptPassword(string encryptedPassword);
    }
}
