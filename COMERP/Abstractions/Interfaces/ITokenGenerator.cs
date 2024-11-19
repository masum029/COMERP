namespace COMERP.Abstractions.Interfaces
{
    public interface ITokenGenerator
    {
        public string GenerateJWTToken((string userId, string userName, string FName, string LName, string email, IList<string> roles) userDetails);
    }
}
