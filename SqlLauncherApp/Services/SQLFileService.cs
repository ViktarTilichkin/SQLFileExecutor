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

namespace SqlLauncherApp.Services
{
    public class SQLFileService : ISQLFileService
    {
        private readonly ISQLFileRepository<MySqlConnection> m_Repository;
        public SQLFileService(ISQLFileRepository<MySqlConnection> repository)
        {
            m_Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task  LaunchQuery()
        {
            MySqlConnection connection = Connection();
            List<string> fileinDb = await m_Repository.GetAll(connection);
            List<FileSQL> newfiles = new List<FileSQL>();
            List<string> files = Directory.EnumerateFiles(AppSettingHelper.PathFolder, "*.sql", SearchOption.TopDirectoryOnly).ToList();
            //newfiles = files.Except(fileinDb);
            for (int i = 0; i < files.Count; i++)
            {
                string name = Path.GetFileName(files[i]);
                //if (fileinDb.Where(x => x.Name == name) != null)
                //{
                //    continue;
                //}
                FileSQL file = new FileSQL();
                file.Name = name;
                file.Сontent = File.ReadAllText(Path.Combine(AppSettingHelper.PathFolder, name));
                newfiles.Add(file);
            }
            await m_Repository.LaunchSqript(connection, newfiles);
            m_Repository.AddNameFile(connection, newfiles);
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
        //public void LaunchSqript(List<FileSQL> ListSqript)
        //{
        //    if (ListSqript == null) throw new ArgumentNullException(nameof(ListSqript));
        //    if (ListSqript.Count == 0) return;
        //    MySqlConnection connection = Connection();
        //    try
        //    {
        //        for (int i = 0; i < ListSqript.Count; i++)
        //        {
        //            var sql = ListSqript[i].Сontent;
        //            Console.WriteLine(ListSqript[i]);
        //            //MySqlCommand command = new MySqlCommand(sql, connection);
        //            // command.ExecuteNonQuery();
        //        }
        //    }
        //    catch (MySqlException ex)
        //    {
        //        Console.WriteLine(ex);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}
    }
}
