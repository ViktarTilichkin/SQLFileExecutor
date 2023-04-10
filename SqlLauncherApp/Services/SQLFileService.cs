using MySql.Data.MySqlClient;
using SqlLauncherApp.Models;
using SqlLauncherApp.Repositories;
using SqlLauncherApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            MySqlConnection connection = Connection();
            List<string> fileinDb = await m_Repository.GetAll(connection);
            List<string> files = Directory.EnumerateFiles(AppSettingHelper.PathFolder, "*.sql", SearchOption.TopDirectoryOnly).ToList();
            for (int i = 0; i < files.Count; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }
            List<string> file = files.Except(fileinDb).ToList();
            connection.Open();
            if (LaunchSqript(connection, file))
            {
                connection.Open();
                AddSqriptName(connection, file);
            }
            else
            {
                // add log errors
            }
        }

        private bool LaunchSqript(MySqlConnection connection, List<string> ListSqript)
        {
            if (ListSqript == null) throw new ArgumentNullException(nameof(ListSqript));
            if (ListSqript.Count == 0) return false;
            var transaction = connection.BeginTransaction();
            try
            {
                for (int i = 0; i < ListSqript.Count; i++)
                {
                    MySqlCommand command = new MySqlCommand(File.ReadAllText(Path.Combine(AppSettingHelper.PathFolder, ListSqript[i])), connection);
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
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
        private void AddSqriptName(MySqlConnection connection, List<string> ListSqript)
        {
            if (ListSqript == null) throw new ArgumentNullException(nameof(ListSqript));
            if (ListSqript.Count == 0) return;
            try
            {
                m_Repository.AddNameFile(connection, ListSqript);
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
