using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLauncherApp.Repositories
{
    internal interface ISQLFileRepository
    {
        private const string SQL_selectItems = "select * from Used_Files";
        private const string SQL_insertItem = "insert into Used_Files(FileName) values {0}";
        private List<string> files = new List<string>();
        public FileSQL()
        {
            CreateDBandTable();
        }
        public List<string> GetAll()
        {
            MySqlConnection connection = Connection();
            MySqlCommand command = new MySqlCommand(SQL_selectItems, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ////File file = new File();
                //file.Name = reader.GetString(1);
                //files.Add(file.Name);
            }
            connection.Close();
            return files;
        }
        public void LaunchSqript(Dictionary<int, string> ListSqript)
        {
            if (ListSqript == null) throw new ArgumentNullException(nameof(ListSqript));
            if (ListSqript.Count == 0) return;
            MySqlConnection connection = Connection();
            try
            {
                for (int i = 0; i < ListSqript.Count; i++)
                {
                    var sql = ListSqript[i];
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
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
        public void AddNameFile(List<string> NewFile)
        {
            if (NewFile == null) throw new ArgumentNullException(nameof(NewFile));
            if (NewFile.Count == 0) return;
            MySqlConnection connection = Connection();
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


                // bulk 

                //MySqlCommand command = new MySqlCommand("insert into Used_Files(FileName) values (@FileName);", connection);
                //command.Parameters.Add("@FileName", MySqlDbType.VarChar);
                //DataTable dataTable = new DataTable();
                //dataTable.Columns.Add("FileName", typeof(string));
                //for (int i = 0; i < NewFile.Count; i++)
                //{
                //    DataRow dataRow = dataTable.NewRow();
                //    dataRow["FileName"] = NewFile[i].Name;
                //    dataTable.Rows.Add(dataRow);
                //}
                //MySqlBulkCopy bulkCopy = new MySqlBulkCopy(connection);
                //bulkCopy.DestinationTableName = "Used_Files";
                //bulkCopy.WriteToServer(dataTable);
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

        public void LaunchSqriptTransaction(Dictionary<int, string> ListSqript)
        {
            if (ListSqript == null) throw new ArgumentNullException(nameof(ListSqript));
            if (ListSqript.Count == 0) return;
            MySqlConnection connection = Connection();
            var transaction = connection.BeginTransaction();
            try
            {
                for (int i = 0; i < ListSqript.Count; i++)
                {
                    MySqlCommand command = new MySqlCommand(ListSqript[i], connection);
                    command.ExecuteNonQuery();
                }
                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
            }
        }
        private void CreateDBandTable()
        {
            MySqlConnection connection = Connection();
            try
            {
                MySqlCommand createTableCmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS Used_Files(ID int primary key auto_increment, FileName varchar(255) not null, Date datetime default NOW());", connection);
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
        private MySqlConnection Connection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(_connectionString);
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
