using MySql.Data.MySqlClient;
using SqlLauncherApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLauncherApp.Repositories
{
    internal class MySQLFileRepository : ISQLFileRepository
    {
        private MySqlConnection Connection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(AppSettingHelper.ConnectionString);
                connection.Open();
                return connection;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("connection error");
            }
        }
    }
}
