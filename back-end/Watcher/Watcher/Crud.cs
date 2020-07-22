using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Watcher
{
    class Crud
    {
        public string userId;
        string sql;
        MySqlCommand comando;
        #region Usuario
        public void AgregarUsuario(string username, string password)
        {
            // Pedir conexion
            MySqlConnection connection = Conexion.Conecta();
            // El comando a ejecutar
            sql = $"INSERT INTO `nyxdb`.`usuario`(username, password) VALUES ({username}, {password})";
            try
            {
                connection.Open();
                comando = new MySqlCommand(sql, connection);
                // Intentar ejecutar el comando
                try
                {
                    comando.ExecuteNonQuery();
                    MySqlCommand com = new MySqlCommand("SELECT * FROM `nyxdb`.`usuario`", connection);
                    MySqlDataReader dataReader = com.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Console.WriteLine((string)dataReader.GetValue(1));
                        Console.WriteLine(username);
                        if ($"'{(string)dataReader.GetValue(1)}'" == username)
                        {
                            Console.WriteLine((int)dataReader.GetValue(0));
                            Conexion.Instance.user = (int)dataReader.GetValue(0);
                        }
                    }
                    dataReader.Close();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fallo de insercion con error " + ex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Finalizado con error " + ex);
            }
        }
        #endregion
        #region Directorios
        public void AgregarDirectorios(string entrada, string nombre)
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!" + Conexion.Instance.user);
            // Pedir conexion
            MySqlConnection connection = Conexion.Conecta();
            string document = GetInfo(entrada);
            // El comando a ejecutar


            try
            {
                connection.Open();
                comando = new MySqlCommand("INSERT INTO `nyxdb`.`directories` VALUES (@userid, @nombre, @document)", connection);
                comando.Parameters.Add("@userid", MySqlDbType.Int32, 11).Value = Conexion.Instance.user;
                comando.Parameters.Add("@nombre", MySqlDbType.VarChar, 200).Value = nombre;
                comando.Parameters.Add("@document", MySqlDbType.LongBlob).Value = document;
                // Intentar ejecutar el comando
                try
                {
                    comando.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fallo de insercion con error " + ex);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Finalizado con error " + ex);
            }
        }
        #endregion
        #region Actualizar
        public void ActualizarDirectorios(string entrada)
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!" + Conexion.Instance.user);
            // Pedir conexion
            MySqlConnection connection = Conexion.Conecta();
            string document = GetInfo(entrada);
            // El comando a ejecutar


            try
            {
                connection.Open();
                comando = new MySqlCommand("UPDATE `nyxdb`.`directories` SET userJSON=@document WHERE userID=@userid", connection);
                comando.Parameters.Add("@userid", MySqlDbType.Int32, 11).Value = Conexion.Instance.user;
                comando.Parameters.Add("@document", MySqlDbType.LongBlob).Value = document;
                // Intentar ejecutar el comando
                try
                {
                    comando.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fallo de insercion con error " + ex);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Finalizado con error " + ex);
            }
        }
        #endregion
        public static string GetInfo(string path)
        {
            string doc = File.ReadAllText(path);
            return doc;
        }
    }
}
