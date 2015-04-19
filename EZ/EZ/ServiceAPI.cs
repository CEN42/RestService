using System;
using System.Data;
using System.Data.SqlClient;

namespace EZ
{

    public class ServiceAPI : IServiceAPI
    {
        SqlConnection dbConnection;

        public ServiceAPI()
        {
            dbConnection = DBConnect.getConnection();
        }

        public void CreateNewAccount(string firstName, string lastName, string email, string password, double phonenumber)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "INSERT INTO [Login.tbl] VALUES ('" + email + "','" + password + "');";

            SqlCommand command = new SqlCommand(query, dbConnection);
            command.ExecuteNonQuery();

            string query2 = "INSERT INTO Customers VALUES ('" + lastName + "','" + firstName + "','" + email + "','" + ' ' + "', '" + phonenumber + "');";


            SqlCommand command2 = new SqlCommand(query2, dbConnection);
            command2.ExecuteNonQuery();

            dbConnection.Close();
        }


        public bool UserAuthentication(string email, string passsword)
        {
            bool auth = false;

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT email FROM [Login.tbl] WHERE email='" + email + "' AND password='" + passsword + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                auth = true;
            }

            reader.Close();
            dbConnection.Close();

            return auth;

        }

        public DataTable GetUserDetails(string email)
        {
            //declare table name - Customers
            DataTable customersTable = new DataTable();
            customersTable.Columns.Add(new DataColumn("firstName", typeof(String)));
            customersTable.Columns.Add(new DataColumn("lastName", typeof(String)));

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT firstName,lastName FROM Customers where email='" + email + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    customersTable.Rows.Add(reader["firstName"], reader["lastName"]);
                }
            }

            reader.Close();
            dbConnection.Close();
            return customersTable;

        }

        public void SetReservation(string email, string stylist, string service, string sdate, string edate, string year)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT userid from Customers  where email='" + email + "';";
            Int32 userid = (Int32)cmd.ExecuteScalar();

            cmd.CommandText = "SELECT id from StylistProfileInfoes where FirstName='" + stylist + "';";
            Int32 stylistid = (Int32)cmd.ExecuteScalar();

            cmd.CommandText = "SELECT serviceid from StylistProfileInfoes where servicedesc='" + service + "';";
            Int32 serviceid = (Int32)cmd.ExecuteScalar();

            DateTime date1;
            DateTime.TryParseExact(sdate, new string[] { "yyyy/MM/DD/  HH:mm" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date1);

            DateTime date2;
            DateTime.TryParseExact(edate, new string[] { "yyyy/MM/DD/  HH:mm" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date2);


            string query = "INSERT INTO Reservations VALUES('" + userid + "','" + 2 + "','" + date1 + "','" + date2 + "','" + serviceid + "','" + stylistid + "');";

            SqlCommand command = new SqlCommand(query, dbConnection);
            command.ExecuteNonQuery();

            dbConnection.Close();

        }

        public void CancelReservation(string email)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT userid from Customers  where email='" + email + "';";
            Int32 userid = (Int32)cmd.ExecuteScalar();

            string query = " DELETE reser_id FROM Reservations where userid ='" + userid + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            command.ExecuteNonQuery();

            dbConnection.Close();

        }

        public DataTable FindReservations(string email)
        {

            DataTable dt = new DataTable();

            dt = EZ.EZSnipsAccess.GetReservations(email);

            return dt;
        }
    }
}