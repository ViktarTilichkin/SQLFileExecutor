using MySql.Data.MySqlClient;
using SqlLauncherApp.Models;
using SqlLauncherApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLauncherApp.Repositories
{
    internal class MySQLFileRepository : ISQLFileRepository<MySqlConnection>
    {
        private const string SQL_selectItems = "select * from Used_Files";
        private const string SQL_createTable = "CREATE TABLE IF NOT EXISTS Used_Files(ID int primary key auto_increment, FileName varchar(255) not null, Date datetime default NOW());";
        private readonly string SQL_insertItem = "insert into Used_Files(FileName) values {0}";
        public MySQLFileRepository()
        {

        }
        public void AddNameFile(MySqlConnection connection, List<string> NewFile)
        {
            if (NewFile == null) throw new ArgumentNullException(nameof(NewFile));
            if (NewFile.Count == 0) return;
            try
            {
                string[] insert = new string[NewFile.Count];
                for (int i = 0; i < NewFile.Count; i++)
                {
                    insert[i] = $"(@name{i})";
                }
                var sql = string.Format(SQL_insertItem, string.Join(",", insert));
                MySqlCommand command = new MySqlCommand(sql, connection);
                for (int i = 0; i < NewFile.Count; i++)
                {
                    string file = NewFile[i];
                    command.Parameters.AddWithValue($"@name{i}", file);
                }
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public void CreateTable(MySqlConnection connection)
        {
            try
            {
                MySqlCommand createTableCmd = new MySqlCommand(SQL_createTable, connection);
                createTableCmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task<List<string>> GetAll(MySqlConnection connection)
        {
            MySqlCommand command = new MySqlCommand(SQL_selectItems, connection);
            var reader = await command.ExecuteReaderAsync();
            List<string> files = new List<string>();
            while (reader.Read())
            {
                files.Add(reader.GetString(1));
            }
            connection.Close();
            return files;
        }
    }
}
