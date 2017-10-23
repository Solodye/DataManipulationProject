using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManipulateMainPgPic
{
    public class ConnectionHelper
    {
        public static string GetConnectionStr()
        {
            return ConfigurationManager.AppSettings["ITI_TouchDB"];
        }

    }
        
}
