using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Watcher
{
    public partial class Nyx : Form
    {
        #region Variables
        public string route;
        List<string> dirFile = new List<string>();
        public List<string> quitarComasDir = new List<string>();
        public List<string> quitarComasFile = new List<string>();
        public int numComasDir = 0;
        public int numComasFile = 0;
        public string toJson;
        public List<string> toJsonDir = new List<string>();
        public List<string> toJsonFile = new List<string>();
        public static String textoInfo = "";
        public static bool existe = false;
        #endregion
            
        public Nyx()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // File Info para ver las propiedades de algun archivo

        #region Button Click
        /// <summary>
        /// Open a window where you select the folder you want to post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWatch_Click(object sender, EventArgs e)
        {
            
            // Separador
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Selecciona la carpeta a compartir";
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblInfo.Text = "Directorio: " + fbd.SelectedPath;
                watcher.Path = fbd.SelectedPath;
                if (Directory.Exists(fbd.SelectedPath) == false)
                {
                    txtInfo.Text = "Ruta inexistente o imposible de acceder a ella";
                    return;
                }
                WriteInJson(fbd);
                
            }
        }
        #endregion
        #region Get Sub Directories
        public JsonDocument WriteInJson(FolderBrowserDialog browserDialog)
        {
            string selectedPath = browserDialog.SelectedPath;
            txtInfo.Text = textoInfo;
            // Get the directory where the app is running
            DirectoryInfo directoryName = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
            // Goes up to the app folder and get the path, then get the json file
            string pathPruebas = $"{directoryName.Parent.Parent.Parent.Parent.FullName}\\data.json";
            File.WriteAllText(pathPruebas, "");
            txtInfo.Text = "Se ha iniciado el watcher";
            // Get a list of all subdirectories that are present in the selected folder
            string[] subDirectories = Directory.GetDirectories(selectedPath);
            // Loop for fill the array of files
            string[] helper = Directory.GetFiles(selectedPath);
            for (int i = 0; i < helper.Length; i++)
            {
                dirFile.Add(helper[i]);
            }
            // Get the files of the directory
            // Loop al the subdirectories to see if they have other subdirectories
            foreach (string subDir in subDirectories)
            {
                // Call a method that is recursive
                RecursiveDirs(subDir);
            }
            // Call the recursive method for the files
            RecursiveFiles(dirFile.ToArray());
            // Pruebas para quitar la ultima coma
            File.AppendAllText(pathPruebas, "{\n\"Directories\":[" + Environment.NewLine);
            File.AppendAllText(pathPruebas, string.Join(",\n", quitarComasDir));
            File.AppendAllText(pathPruebas, "\n],\n\"Files\":[\n");
            File.AppendAllText(pathPruebas, string.Join(",\n", quitarComasFile));
            File.AppendAllText(pathPruebas, "\n]\n}");
            // ================================================ Prueba cosas de json =============================
            //Se pasa el archivo del que se recibe informacion
            string entrada = File.ReadAllText($"{directoryName.Parent.Parent.Parent.Parent.FullName}\\data.json");
            //Opciones para el escritor
            var writerOptions = new JsonWriterOptions
            {
                Indented = true
            };
            //Opciones para el documento
            var documentOptions = new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip
            };
            //Archivo al que se va a escribir la informacion
            FileStream fs = File.Create($"{directoryName.Parent.Parent.Parent.Parent.FullName}\\test.json");
            //Se crea el escritor
            var writer = new Utf8JsonWriter(fs, options: writerOptions);
            //Se crea el documento
            JsonDocument document = JsonDocument.Parse(entrada, documentOptions);
            //Se obtiene el inicio del documento
            JsonElement root = document.RootElement;
            //Se evalua si tiene un elemento en el interior
            if (root.ValueKind == JsonValueKind.Object)
            {
                //Se declara el inicio del escritor
                writer.WriteStartObject();
            }
            else { return null; }//Retorna si no hay nada
            //Para cada elemento en el documento
            foreach (JsonProperty property in root.EnumerateObject())
            {
                //Se escribe la propiedad con el escritor
                property.WriteTo(writer);
            }
            //Se declara el final del escritor
            writer.WriteEndObject();
            //Se finaliza con el escritor
            writer.Flush();
            //Ejecutar insercion en base de datos
            Crud crud = new Crud();
            crud.AgregarDirectorios($"{directoryName.Parent.Parent.Parent.Parent.FullName}\\data.json", textBox1.Text);
            //Devuelvo el objeto json
            return document;
        }
        #endregion
        #region Recursive Directories
        public void RecursiveDirs(string dirs)
        {
            string[] helper = Directory.GetFiles(dirs);
            for (int i = 0; i < helper.Length; i++)
            {
                dirFile.Add(helper[i]);
            }
            // Write the previous string into the file that is in the path that we aleady define "{Directory:{"+
            string nuevaLinea = $"\"{dirs.Replace(@"\", @"\\")}\"";
            quitarComasDir.Add(nuevaLinea);
            toJsonDir.Add(dirs);
            //File.AppendAllText(path, "}," + Environment.NewLine);
            // Get a list of all subdirectories that are present in the entry directory
            string[] subDirectories = Directory.GetDirectories(dirs);
            // Loop al the subdirectories to see if they have other subdirectories
            foreach (string subDir in subDirectories)
            {
                // Call to himself
                RecursiveDirs(subDir);
            }
        }
        #endregion
        #region Recursive Files
        public void RecursiveFiles(string[] directory)
        {
            // Write the path of all the files
            foreach (string file in directory)
            {
                string nuevoFile = $"\t\"{file.Replace(@"\", @"\\")}\"";
                quitarComasFile.Add(nuevoFile);
                toJsonFile.Add(file);
                // Write the previous string into the file that is in the path that we aleady define
            }
        }
        #endregion
        #region File System Watcher
        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            txtInfo.Text = $"Se ha cambiado el archivo \"{e.Name}\" en la ruta {e.FullPath}";
        }

        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            txtInfo.Text = $"Se ha creado el archivo \"{e.Name}\" en la ruta {e.FullPath}";
        }

        private void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            txtInfo.Text = $"Se ha borrado el archivo \"{e.Name}\" en la ruta {e.FullPath}";
        }

        private void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            txtInfo.Text = $"Se ha cambiado el nombre del archivo \"{e.OldName}\" a \"{e.Name}\" en la ruta {e.FullPath}";
        }
        #endregion

        private void Nyx_Load(object sender, EventArgs e)
        {

        }

        private void Nyx_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form2 form = new Form2();
            form.Close();
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
