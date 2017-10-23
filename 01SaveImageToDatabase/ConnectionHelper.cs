using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace _01SaveImageToDatabase
{
    public class ConnectionHelper
    {
        public static string GetConnectionStr()
        {
            return ConfigurationManager.AppSettings["ITI_TouchDB"];
        }
        
    }
}
