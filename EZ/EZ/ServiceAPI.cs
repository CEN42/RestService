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

            string query = "INSERT INTO [Login.tbl] VALUES ('" + email + "','" + password +"');";

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

            string query = "SELECT firstName,lastName FROM Customers where email='"+email+"';";

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

        
    }
}