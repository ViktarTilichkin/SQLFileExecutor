using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLauncherApp.Services
{
    public class SQLFileService : ISQLFileService
    {
        private readonly ISQLFileService m_Repository;
        public SQLFileService(ISQLFileService repository) 
        {
            m_Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
    }
}
