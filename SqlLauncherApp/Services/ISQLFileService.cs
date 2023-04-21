using MySql.Data.MySqlClient;
using SqlLauncherApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLauncherApp.Services
{
    public interface ISQLFileService
    {
        Task LaunchQuery();
    }
}
