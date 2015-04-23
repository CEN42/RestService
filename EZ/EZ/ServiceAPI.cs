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

            string query2 = "INSERT INTO Customers VALUES ('" + lastName + "','" + firstName + "','" + email + "','" +  0  + "', '" + phonenumber + "');";


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

        public void SetReservation(string email, string stylist, string service, string fdate, string edate)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }
         

            string query1 = "SELECT userid from Customers where email= 'n';";
            SqlCommand cmd = new SqlCommand(query1, dbConnection);
            Int32 userid = Convert.ToInt32(cmd.ExecuteScalar());

            string query2 = "SELECT id from StylistProfileInfoes where FirstName='" + stylist + "';";
            SqlCommand cm = new SqlCommand(query2, dbConnection);
            Int32 stylistid = Convert.ToInt32(cm.ExecuteScalar());

            string query3 = "SELECT serviceid from Services where servicetitle='" + service + "';";
            SqlCommand cms = new SqlCommand(query3, dbConnection);
            Int32 serviceid = Convert.ToInt32(cms.ExecuteScalar());

             string query = "INSERT INTO Reservations VALUES('" + userid + "','" + 1 + "','" + Convert.ToDateTime(fdate) + "','" + Convert.ToDateTime(edate) + "','" + serviceid + "','" + stylistid + "'); ";
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

            string query1 = "SELECT userid from Customers  where email='" + email + "';";
            SqlCommand cmd = new SqlCommand(query1, dbConnection);
            Int32 userid = Convert.ToInt32(cmd.ExecuteScalar()); ;

            string query = " DELETE FROM Reservations where userid ='" + userid + "';";

            SqlCommand command = new SqlCommand(query, dbConnection);
            command.ExecuteNonQuery();

            dbConnection.Close();

        }

        public DataTable GetStylistNames()
        {

            //declare table name - Customers
            DataTable stylistTable = new DataTable();
            stylistTable.Columns.Add(new DataColumn("FirstName", typeof(String)));
            stylistTable.Columns.Add(new DataColumn("LastName", typeof(String)));

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT FirstName,LastName FROM StylistProfileInfoes;";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    stylistTable.Rows.Add(reader["FirstName"], reader["LastName"]);
                }
            }

            reader.Close();
            dbConnection.Close();
            return stylistTable;
        }

        public DataTable FindReservations(string email)
        {

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            DataTable result = new DataTable();
           // Conn = getConnection();
            using (SqlCommand command = dbConnection.CreateCommand())
            {
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(new SqlParameter[] {        //params for stored procs
                        new SqlParameter("@Email", email),
                    });
                command.CommandText = "cGetReservations_sp";         //"ManagerGetReservations" is stored proc
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = command;

                    if (command.Connection.State != ConnectionState.Open)
                        command.Connection.Open();

                    adapter.Fill(result);

                    command.Connection.Close();
                }
            }
            dbConnection.Close();

            foreach(DataRow dr in result.Rows) 
            {
                if(dr["resStart"] != DBNull.Value)
                {
                    dr["resStart"] = Convert.ToDateTime(dr["resStart"]).ToString("MM-dd-yyyy hh:mm tt");
                }
            }
            return result;
        }

        public DataTable DisplayServices()
        {
            //declare table name - Customers
            DataTable serviceTable = new DataTable();
            serviceTable.Columns.Add(new DataColumn("servicetitle", typeof(String)));
            serviceTable.Columns.Add(new DataColumn("amount", typeof(Double)));

            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            string query = "SELECT servicetitle, amount FROM Services;";

            SqlCommand command = new SqlCommand(query, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    serviceTable.Rows.Add(reader["servicetitle"], reader["amount"]);
                }
            }

            reader.Close();
            dbConnection.Close();
            return serviceTable;

        }

      

    }
}