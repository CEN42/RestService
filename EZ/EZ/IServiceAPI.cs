using System.Data;

namespace EZ
{
    public interface IServiceAPI
    {
        void CreateNewAccount(string firstName, string lastName, string email, string password, double phonenumber);
        DataTable GetUserDetails(string email);
        bool UserAuthentication(string email, string passsword);
    }
}