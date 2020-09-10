using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram
{
    public class AppSettings
    {
        public static string Domain { get { return ConfigurationManager.AppSettings["Domain"]; } }
        public static string Container { get { return ConfigurationManager.AppSettings["Container"]; } }
        public static string Admin { get { return ConfigurationManager.AppSettings["Admin"]; } }
        public static string AdminPassword
        {
            get { return ConfigurationManager.AppSettings["AdminPassword"]; }
        }

        public static string AttributeOne { get { return ConfigurationManager.AppSettings["AttributeOne"]; } }
        public static string AttributeTwo { get { return ConfigurationManager.AppSettings["AttributeTwo"]; } }

    }
}
