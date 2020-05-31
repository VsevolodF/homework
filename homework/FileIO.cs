using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace homework
{
    public class FileIO
    {
        Encoding defaultEnc = Encoding.UTF8;

        /// <summary>
        /// Метод, считывающий данные из файла.
        /// </summary>
        /// <param name="path"> Путь к файлу. </param>
        /// <returns> Возвращает массив строк из файла. </returns>
        public static string FileRead(string path)
        {
            string res = "Error";
            Encoding enc = Encoding.UTF8;

            try
            {

                using (var reader = new StreamReader(path, true))
                {
                    var currentEncoding = reader.CurrentEncoding;
                    enc = reader.CurrentEncoding;
                }

                // Считывание из файла.
                res = File.ReadAllText(path, enc);
            }
            // Улавливание возможных ошибок.
            catch (IOException e)
            {
                Console.WriteLine("IO Exception: ", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Unauthorized Access Exception: ", e.Message);
            }
            catch (System.Security.SecurityException e)
            {
                Console.WriteLine("Security Exception: ", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: ", e.Message);
            }

            return res;
        }

        /// <summary>
        /// Метод, записывающий данные в файл.
        /// </summary>
        /// <param name="path"> Путь к файлу. </param>
        public static void FileWrite(string path, string data)
        {
            Encoding enc = Encoding.UTF8;

            try
            {
                // Запись в файл построчно.
                File.WriteAllText(path, data, enc);
            }
            // Улавливание возможных ошибок.
            catch (IOException e)
            {
                Console.WriteLine("IO Exception: ", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Unauthorized Access Exception: ", e.Message);
            }
            catch (System.Security.SecurityException e)
            {
                Console.WriteLine("Security Exception: ", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: ", e.Message);
            }
        }

        /// <summary>
        /// Метод, удаляющий файл.
        /// </summary>
        /// <param name="path"> Путь к файлу. </param>
        public static void FileDelete(string path)
        {
            try
            {
                // Удаление файла, если он существует.
                if (File.Exists(path))
                    File.Delete(path);
            }
            // Улавливание возможных ошибок.
            catch (IOException e)
            {
                Console.WriteLine("IO Exception: ", e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Unauthorized Access Exception: ", e.Message);
            }
            catch (System.Security.SecurityException e)
            {
                Console.WriteLine("Security Exception: ", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: ", e.Message);
            }
        }
    }
}
