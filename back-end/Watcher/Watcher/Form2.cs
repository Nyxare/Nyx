using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Watcher
{
    public partial class Form2 : Form
    {
        bool pasa = false;
        public string user;
        string contrase;
        public int id;
        Nyx form = new Nyx();
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = Conexion.Conecta(); ;
            MySqlCommand comando;
            string sql = $"SELECT * FROM `nyxdb`.`usuario`";
            try
            {
                conn.Open();
                comando = new MySqlCommand(sql, conn);
                MySqlDataReader dataReader = comando.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if ((string)dataReader.GetValue(1) == txtUser.Text)
                        {
                            pasa = true;
                            Console.WriteLine("EL WEY SÍ EXISTE");
                            Conexion.Instance.user = (int)dataReader.GetValue(0);
                            contrase = (string)dataReader.GetValue(2);
                            break;
                        }
                        else
                        {
                            pasa = false;
                        }
                    }
                    dataReader.Close();
                    // Intentar ejecutar el comando
                    try
                    {
                        comando.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Fallo de solicitud con error " + ex);
                    }
                    if (pasa)
                    {
                        Console.WriteLine("El usuario ya existe en la base de datos");
                        if (contrase == txtPass.Text)
                        {
                            Console.WriteLine("Contrasena coincide");
                            form.Show();
                            this.Hide();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("No coincide la pass");
                            MessageBox.Show("La contraseña no coincide");
                            txtPass.Text = "";
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Crear nuevo usuario");
                        if (MessageBox.Show("El usuario no existe, crear nuevo?", "", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                        {
                            button1.Hide();
                            btnNew.Show();
                            return;
                        }
                        else
                        {
                            txtPass.Text = "";
                            txtUser.Text = "";
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No hay filas");
                    if (MessageBox.Show("El usuario no existe, crear nuevo?", "", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        button1.Hide();
                        btnNew.Show();
                        return;
                    }
                    else
                    {
                        txtPass.Text = "";
                        txtUser.Text = "";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fallo de conexion con error " + ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Crud crud = new Crud();
            crud.AgregarUsuario($"'{txtUser.Text}'", $"'{txtPass.Text}'");
            form.Show();
            this.Hide();
        }
    }
}
