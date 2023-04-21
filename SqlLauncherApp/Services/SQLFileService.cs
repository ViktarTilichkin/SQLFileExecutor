using MySql.Data.MySqlClient;
using SqlLauncherApp.Repositories;
using SqlLauncherApp.Utilities;

namespace SqlLauncherApp.Services
{
    public class SQLFileService : ISQLFileService
    {
        private readonly ISQLFileRepository<MySqlConnection> m_Repository;
        public SQLFileService(ISQLFileRepository<MySqlConnection> repository)
        {
            m_Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task LaunchQuery()
        {
            var fileinDb = await m_Repository.GetAll(Connection());
            Console.WriteLine(string.Join(",", fileinDb));
            var filesFolder = Directory.EnumerateFiles(AppSettingHelper.PathFolder, "*.sql", SearchOption.TopDirectoryOnly).Select(x => Path.GetFileName(x));
            var file = filesFolder.Except(fileinDb).ToList();
            if (LaunchSqript(Connection(), file))
            {
                AddSqriptName(Connection(), file);
            }
            else
            {
                Console.WriteLine("Wait for updates with logging");
            }
        }

        private bool LaunchSqript(MySqlConnection connection, List<string> listSqript)
        {
            if (listSqript == null) throw new ArgumentNullException(nameof(listSqript));
            if (listSqript.Count == 0) return false;
            var transaction = connection.BeginTransaction();
            try
            {
                for (int i = 0; i < listSqript.Count; i++)
                {
                    var query = File.ReadAllText(Path.Combine(AppSettingHelper.PathFolder, listSqript[i]));
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                Console.WriteLine("Sqript successful:   " + string.Join(", ", listSqript));
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        private void AddSqriptName(MySqlConnection connection, List<string> listSqript)
        {
            if (listSqript == null) throw new ArgumentNullException(nameof(listSqript));
            if (listSqript.Count == 0) return;
            try
            {
                m_Repository.AddNameFile(connection, listSqript);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
