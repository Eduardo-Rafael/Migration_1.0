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
            Console.WriteLine("Finish");
            Console.ReadLine();
        }

        static void Process_Claim(string origin, string logfile , string destination)
        {
            //Open and Read the File .log
            var migrated_documentos = File.ReadAllLines(logfile);

            //To get the Total of migrated files
            int migrated_documents_total = Convert.ToInt32(migrated_documentos[migrated_documentos.Length - 2].Split(' ')[4]);
            int current_document = 0;

            //To get the migrated file Names
            var query = (from documents in migrated_documentos
                         where documents.Contains("File to migrate:")
                         select documents).ToArray();

            foreach (var item in query)
            {
                int substring_count = item.Split(' ').Length;
                string File_to_Find = origin + @"\" + item.Split(' ')[substring_count - 1];

                try
                {
                    Move_File(destination, migrated_documents_total, ref current_document, File_to_Find);
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.Message);
                }

            }

            if (query.Length > 0)
            {
                Console.WriteLine("{0} Migrated Files From {1}", current_document, migrated_documents_total);
            }
            else
                Console.WriteLine("There is no files to migrate");
        }
        static void Process_Policies_and_Agents(string origin, string logfile, string destination)
        {
            //Open and Read the File .log
            var migrated_documentos = File.ReadAllLines(logfile);

            //To get the Total of migrated files
            int migrated_documents_total = Convert.ToInt32(migrated_documentos[migrated_documentos.Length - 2].Split(' ')[4]);
            int current_document = 0;

            //To get the migrated file Names
            var query = (from documents in migrated_documentos
                         where documents.Contains("successfully uploaded to DMS")
                         select documents).ToArray();

            foreach (var item in query)
            {
                string temp = item.Split(' ')[5];
                string File_Name = temp.Substring(1, temp.Length - 2);
                string File_to_Find = origin + @"\" + File_Name;

                try
                {
                    Move_File(destination, migrated_documents_total, ref current_document, File_to_Find);
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.Message);
                }
            }

            if (query.Length > 0)
            {
                Console.WriteLine("{0} Migrated Files From {1}", current_document, migrated_documents_total);
            }
            else
                Console.WriteLine("There is no files to migrate");
        }
        static void Process_Precerts(string origin , string logfile , string destination)
        {
            //Open and Read the File .log
            var migrated_documentos = File.ReadAllLines(logfile);

            //To get the Total of migrated files
            int migrated_documents_total = Convert.ToInt32(migrated_documentos[migrated_documentos.Length - 4].Split(' ')[6]);
            int current_document = 0;

            //To get the migrated file Names
            var query = (from documents in migrated_documentos
                         where documents.Contains("Loading file:")
                         select documents).ToArray();

            foreach (var item in query)
            {
                string temp = item.Split(' ')[item.Split(' ').Length - 1];
                string File_Name = temp.Substring(1, temp.Length - 2);
                string File_to_Find = origin + @"\" + File_Name;

                try
                {
                    Move_File(destination, migrated_documents_total, ref current_document, File_to_Find);
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.Message);
                }
            }

            
            if (query.Length > 0)
            {
                Console.WriteLine("{0} Migrated Files From {1}", current_document, migrated_documents_total);
            }
            else
                Console.WriteLine("There is no files to migrate");
        }

        private static void Move_File(string destination, int migrated_documents_total,ref int current_document, string File_to_Find)
        {
            FileInfo file = new FileInfo(File_to_Find);
            if (file.Exists)
            {
                current_document++;
                string archivo_destino = destination + @"\" + file.Name;
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
    }
}
