using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLauncherApp.Models
{
    public class File
    {
        public string Name { get; set; }
        public string Сontent { get; set; }
        public File()
        {

        }
        public File(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
