using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z1
{
    internal class Database
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server = localhost; Port = 5432; Database = Test me; User Id = postgres; Password= assaq123");

        public void openConnection()
        {
            conn.Open();
        }

        public void closeConnection() 
        {
            conn.Close();   
        }

        public NpgsqlConnection getConnection()
        {
            return conn;
        }
    }
}
