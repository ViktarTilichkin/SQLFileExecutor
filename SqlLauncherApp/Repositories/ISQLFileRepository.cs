using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SqlLauncherApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SqlLauncherApp.Repositories
{
    public interface ISQLFileRepository<T> where T : DbConnection
    {
        Task<List<string>> GetAll(T connection);
        Task LaunchSqript(T connection, List<FileSQL> ListSqript);
        void AddNameFile(T connection, List<FileSQL> NewFile);
        void CreateTable(T connection);
    }
}
