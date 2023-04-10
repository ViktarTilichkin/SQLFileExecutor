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
        void AddNameFile(T connection, List<string> NewFile);
        void CreateTable(T connection);
    }
}
