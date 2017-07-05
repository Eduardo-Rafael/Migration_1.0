using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Migration_1._0
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                if (args.Length >= 3)
                {
                    if(args[0].EndsWith("claims"))
                    {
                        Process_Claim(args[0],args[1],args[2]);
                    }  
                    else if(args[0].EndsWith("documents"))
                    {
                        Process_Policies_and_Agents(args[0], args[1], args[2]);
                    }
                    else if(args[0].EndsWith("precerts"))
                    {
                        Process_Precerts(args[0] , args[1] , args[2]);
                    }
                }
                else
                    throw new Exception(String.Format("Tiene que pasar los parametros de entrada:\n{0}\n{1}\n{2}", "Directorio de los archivos", "Archivo con el listado de archivos migrados", "Directorio Destino"));

            }
            catch(Exception error)
            {
                Console.WriteLine(error.Message);
            }
            finally
            {

            }
        }

        static void Process_Claim(string a, string b , string c)
        {
            //Open and Read the File .log
            var migrated_documentos = File.ReadAllLines(b);
            int migrated_documents_total = Convert.ToInt32(migrated_documentos[migrated_documentos.Length - 2].Split(' ')[4]);
            int current_document = 0;

            foreach (var item in migrated_documentos)
            {
                if (item.Contains("File to migrate:"))
                {
                    int substring_count = item.Split(' ').Length;
                    string File_to_Find = a + @"\" + item.Split(' ')[substring_count - 1];

                    try
                    {
                        FileInfo file = new FileInfo(File_to_Find);
                        if (file.Exists)
                        {
                            current_document++;
                            string archivo_destino = c + @"\" + file.Name;
                            Console.WriteLine("File to move : {0}", file.Name);
                            file.MoveTo(archivo_destino);
                            Console.WriteLine("The file has been moved");
                            Console.WriteLine("{0}%", current_document * 100 / migrated_documents_total);
                        }
                        else
                        {
                            Console.WriteLine("The file : {0} doesn't exist", file.Name);
                        }
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine(error.Message);
                    }
                }
            }
            Console.WriteLine("{0} Migrated Files From {1}", current_document, migrated_documents_total);
            Console.WriteLine("Finish");
            Console.ReadLine();
        }
        static void Process_Policies_and_Agents(string a, string b, string c)
        {
            var migrated_documentos = File.ReadAllLines(b);
            int migrated_documents_total = Convert.ToInt32(migrated_documentos[migrated_documentos.Length - 2].Split(' ')[4]);
            int current_document = 0;

            foreach (var item in migrated_documentos)
            {
                if(item.Contains("successfully uploaded to DMS"))
                {
                    string temp = item.Split(' ')[5];
                    string File_Name = temp.Substring(1, temp.Length - 2);
                    string File_to_Find = a + @"\" + File_Name;

                    try
                    {
                        FileInfo file = new FileInfo(File_to_Find);
                        if(file.Exists)
                        {
                            current_document++;
                            string archivo_destino = c + @"\" + file.Name;
                            Console.WriteLine("File to move : {0}", file.Name);
                            file.MoveTo(archivo_destino);
                            Console.WriteLine("The file has been moved");
                            Console.WriteLine("{0}%", current_document * 100 / migrated_documents_total);

                        }
                        else
                        {
                            Console.WriteLine("The file : {0} doesn't exist", file.Name);
                        }
                    }
                    catch(Exception error)
                    {
                        Console.WriteLine(error.Message);
                    }
                }
            }
            Console.WriteLine("{0} Migrated Files From {1}", current_document, migrated_documents_total);
            Console.WriteLine("Finish");
            Console.ReadLine();
        }
        static void Process_Precerts(string a , string b , string c)
        {
            var migrated_documentos = File.ReadAllLines(b);
            string temp_1 = migrated_documentos[migrated_documentos.Length - 4];
            string temp_2 = temp_1.Split(' ')[6];
            int migrated_documents_total = Convert.ToInt32(temp_2);
            int current_document = 0;

            foreach (var item in migrated_documentos)
            {
                if(item.Contains("Loading file:"))
                {
                    string temp = item.Split(' ')[item.Split(' ').Length - 1];
                    string File_Name = temp.Substring(1, temp.Length - 2);
                    string File_to_Find = a + @"\" + File_Name;

                    try
                    {
                        FileInfo file = new FileInfo(File_to_Find);
                        if(file.Exists)
                        {
                            current_document++;
                            string archivo_destino = c + @"\" + file.Name;
                            Console.WriteLine("File to move : {0}", file.Name);
                            file.MoveTo(archivo_destino);
                            Console.WriteLine("The file has been moved");
                            Console.WriteLine("{0}%", current_document * 100 / migrated_documents_total);
                        }
                        else
                        {
                            Console.WriteLine("The file : {0} doesn't exist", file.Name);
                        }

                    }
                    catch(Exception error)
                    {
                        Console.WriteLine(error.Message);
                    }
                }
            }

            Console.WriteLine("{0} Migrated Files From {1}", current_document, migrated_documents_total);
            Console.WriteLine("Finish");
            Console.ReadLine();

        }
    }
}
