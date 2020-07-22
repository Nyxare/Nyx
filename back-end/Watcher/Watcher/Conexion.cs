using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher
{
    class Conexion
    {
        #region Static Instance
        private static Conexion instance;
        public static Conexion Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Conexion();
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }
        #endregion

        public int user;
        static string cnnString = "Server=localhost;Port=3306; Database=nyxdb; Uid=root; Pwd=root";
        private static MySqlConnection connection;

        public static MySqlConnection Conecta()
        {
            connection = new MySqlConnection(cnnString);
            return connection;
        }
    }
}
