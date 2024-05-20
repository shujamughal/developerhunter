namespace CompanyProfile.Services
{
    public interface ICreateUserCookie
    {
        bool SetUserCookie(string email, string name);
    }
}
