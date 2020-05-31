using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Net;

namespace homework
{
    class Program
    {
        static List<string> booksNames = new List<string>();

        /// <summary>
        /// Словарь правил транслитерации. 
        /// </summary>
        static Dictionary<char, string> replacer = new Dictionary<char, string>()
        {
            { 'a', "а"},
            { 'b', "б"},
            { 'v', "в"},
            { 'g', "г"},
            { 'd', "д"},
            { 'e', "е"},
            { 'j', "ж"},
            { 'z', "з"},
            { 'i', "и"},
            { 'k', "к"},
            { 'l', "л"},
            { 'm', "м"},
            { 'n', "н"},
            { 'o', "о"},
            { 'p', "п"},
            { 'r', "р"},
            { 's', "с"},
            { 't', "т"},
            { 'u', "у"},
            { 'f', "ф"},
            { 'h', "х"},
            { 'c', "ц"},
            { 'q', "ку"},
            { 'w', "у"},
            { 'x', "кс"},
            { 'y', "ы"}
        };

        /// <summary>
        /// Считывание всех книг из директории.
        /// </summary>
        /// <param name="folderPath"> Путь до директории. </param>
        /// <returns> Возвращает все txt файлы лежащие по заданному пути. </returns>
        private static List<string> ReadAllBooks(string folderPath)
        {
            List<string> books = new List<string>();
            string book;

            try
            {
                booksNames = Directory.GetFiles(folderPath, "*.txt").ToList();
                ClearPaths();

                foreach (string file in Directory.EnumerateFiles(folderPath, "*.txt"))
                {
                    book = File.ReadAllText(file);
                    books.Add(book);
                }

            }
            catch (Exception e) when
            (e is IOException || e is UnauthorizedAccessException
            || e is DirectoryNotFoundException || e is System.Security.SecurityException)
            {
                Console.WriteLine("Ошибка доступа к файлам: " + e.Message);
                Console.WriteLine("Завершение работы прораммы");
                Environment.Exit(0);
            }
            catch (Exception)
            {
                Console.WriteLine("Непредвиденная ошибка");
                Console.WriteLine("Завершение работы прораммы");
                Environment.Exit(0);
            }
            return books;
        }

        /// <summary>
        /// Удаление полного пути из полученных названий файлов.
        /// </summary>
        private static void ClearPaths()
        {
            int pathSize = 15;

            try
            {
                for (int i = 0; i < booksNames.Count; i++)
                    booksNames[i] = booksNames[i].Substring(pathSize, booksNames[i].Length - pathSize);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Ошибка очистки полного пути, завершение работы программы.");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Метод, заменяющий английские буквы на созвучные им русскуие сочетания.
        /// </summary>
        /// <param name="book"> Текст для транслитерации. </param>
        /// <returns> Возвращает преобразованный текст. </returns>
        private static string Replacer(string book)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < book.Length; i++)
            {
                // Проверка на то, что символ буква из юникода.
                if (Char.IsLetter(book[i]))
                {
                    // Проверка на то, что буква не латинская.
                    if ((Char.IsUpper(book[i]) && (book[i] < 'A' || book[i] > 'Z')) ||
                        (Char.IsLower(book[i]) && (book[i] < 'a' || book[i] > 'z')))
                        continue;
                    else
                    {
                        // Проверка "размера" буквы.
                        if (Char.IsUpper(book[i]))
                            builder.Append(replacer[Char.ToLower(book[i])]);
                        else
                            builder.Append(replacer[book[i]]);
                    }
                }
                else
                    // Добавление всех остальных символов.
                    builder.Append(book[i]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Синхронная транслитерация массива книг.
        /// </summary>
        /// <param name="books"> Книги для транслитерации. </param>
        private static void Transliterator(List<string> books)
        {
            int startValue, endValue;

            for (int i = 0; i < books.Count; i++)
            {
                // Сохраняем начальное кол-во символов
                startValue = books[i].Length;
                books[i] = Replacer(books[i]);
                // Сохраняем конченое кол-во символов
                endValue = books[i].Length;

                Console.WriteLine($"book {booksNames[i]}: {startValue} -> {endValue}");
            }
        }

        /// <summary>
        /// Асинхронная транслитерация массива книг.
        /// </summary>
        /// <param name="books"> Книги для транслитерации. </param>
        private static void TransliteratorAsync(List<string> books)
        {
            Parallel.For(0, books.Count, i =>
            {
                int startValue, endValue;

                // Сохраняем начальное кол-во символов
                startValue = books[i].Length;
                books[i] = Replacer(books[i]);
                // Сохраняем конченое кол-во символов
                endValue = books[i].Length;
                Console.WriteLine($"book {booksNames[i]}: {startValue} -> {endValue}");
            });

        }

        /// <summary>
        /// GET запрос на сервер.
        /// </summary>
        /// <returns> возвращает ответ сервера. </returns>
        private static string GET()
        {
            string url = "https://www.gutenberg.org/files/1342/1342-0.txt";
            string response = "";

            using (var webClient = new WebClient())
            {
                try
                {
                    response = webClient.DownloadString(url);
                }
                catch (WebException)
                {
                    Console.WriteLine("Ошибка доступа к серверу, завершение работы программы");
                    Environment.Exit(0);
                }
                catch (Exception)
                {
                    Console.WriteLine("Непредвиденаня ошибка, завершение работы программы");
                    Environment.Exit(0);
                }
            }

            return response;
        }


        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();

            // 1 часть синхронно.
            List<string> books = ReadAllBooks("../../../Книги");
            Console.WriteLine("sync run\n");
            timer.Start();
            Transliterator(books);
            timer.Stop();
            Console.WriteLine("Run time: " + timer.Elapsed.TotalSeconds);

            // 1 часть асинхронно
            books = ReadAllBooks("../../../Книги");
            Console.WriteLine("\nasync run\n");
            timer.Reset();
            timer.Start();
            TransliteratorAsync(books);
            timer.Stop();
            Console.WriteLine("Run time: " + timer.Elapsed.TotalSeconds);

            // Запись результата работы 1 части.
            for (int i = 0; i < books.Count; i++)
                FileIO.FileWrite($"../../../new_Книги/new_{booksNames[i]}", books[i]);

            // 2 часть.
            string onlineBook = GET();
            timer.Reset();
            timer.Start();
            int startValue = onlineBook.Length;
            onlineBook = Replacer(onlineBook);
            int endValue = onlineBook.Length;
            timer.Stop();
            Console.WriteLine($"\nnew_book_from_web: {startValue} -> {endValue}");
            Console.WriteLine("Run time: " + timer.Elapsed.TotalSeconds);

            // Запись результата работы 2 части.
            FileIO.FileWrite("../../../new_Книги/new_book_from_web", onlineBook);

            Console.ReadLine();
        }
    }
}
