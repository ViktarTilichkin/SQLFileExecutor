using SqlLauncherApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLauncherApp.Services
{
    public interface ISQLFileService
    {
        // описываем метдоты работы  
        // типо формирование листа файлов которые нужно выполнить
        // выполнение этого листа 
        // пуш имен в бд
       Task LaunchQuery();

    }
}
