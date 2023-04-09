using SqlLauncherApp.Repositories;
using SqlLauncherApp.Services;
using System;

namespace SqlLauncherApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ISQLFileService fileControl = new SQLFileService(new MySQLFileRepository());
            fileControl.LaunchQuery().Wait();
            Task task = fileControl.LaunchQuery();
            Task.WaitAll(task);
        }
    }
}