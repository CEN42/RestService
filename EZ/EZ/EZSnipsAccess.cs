using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace EZ
{
    public class EZSnipsAccess 
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static SqlConnection Conn;

        public static SqlConnection getConnection()
        {
            Conn = new SqlConnection(connectionString);
            return Conn;


        }

        public static DataTable GetReservations(string email)
        {
            DataTable result = new DataTable();
            Conn = getConnection();
            using (SqlCommand command = Conn.CreateCommand())
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
            return result;
        }

        public static DataTable GetServices()
        {
            DataTable result = new DataTable();
            Conn = getConnection();
            using (SqlCommand command = Conn.CreateCommand())
            {
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;

                command.CommandText = "get_services_sp";         //"get_services_sp" is stored proc
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = command;

                    if (command.Connection.State != ConnectionState.Open)
                        command.Connection.Open();

                    adapter.Fill(result);

                    command.Connection.Close();
                }
            }
            return result;
        }


        public static void InsertReservation(int userid, int tempid, int serviceid, int stylistid, DateTime startDate, DateTime endDate)
        {
            DataTable result = new DataTable();
            Conn = getConnection();
            using (SqlCommand command = Conn.CreateCommand())
            {
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddRange(new SqlParameter[] {        //params for stored proc
                        new SqlParameter("@userid", userid),
                        new SqlParameter("@tempid", tempid),
                        new SqlParameter("@serviceid", serviceid),
			new SqlParameter("@stylistid", stylistid),
			new SqlParameter("@rStart", startDate),
			new SqlParameter("@rEnd", endDate)
                    });
                command.CommandText = "Insert_Reservation_sp";         //"Insert_Reservation_sp" is stored proc
                
                command.Connection.Close();
                }
        }
    }
}