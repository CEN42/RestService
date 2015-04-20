using System.Data;

namespace EZ
{
    public interface IServiceAPI
    {
        void CreateNewAccount(string firstName, string lastName, string email, string password, double phonenumber);
        DataTable GetUserDetails(string email);
        bool UserAuthentication(string email, string passsword);

        void SetReservation(string email, string stylist, string service, string sdate, string edate, string year);
        void CancelReservation(string email);
        DataTable FindReservations(string email);
        DataTable DisplayServices();
        DataTable GetStylistNames();

    }
}