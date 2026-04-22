using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace Lab2_Task2_Sync
{
    internal static class Task2Sync
    {
        private static readonly string[] Urls =
        {
            "https://jsonplaceholder.typicode.com/todos/1",
            "https://api.agify.io/?name=michael",
            "https://api.nationalize.io/?name=michael"
        };

        public static void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                for (int i = 0; i < Urls.Length; i++)
                {
                    string json = GetJsonSync(Urls[i]);

                    Console.WriteLine($"Ответ {i + 1}:");
                    Console.WriteLine(json);
                    Console.WriteLine();
                }

                stopwatch.Stop();
                Console.WriteLine($"Общее время работы: {stopwatch.Elapsed}");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Console.WriteLine($"Ошибка выполнения: {ex.Message}");
            }
        }

        private static string GetJsonSync(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "application/json";
            request.UserAgent = "CSharpLab/1.0";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    string errorBody = string.Empty;

                    Stream errorStream = errorResponse.GetResponseStream();
                    if (errorStream != null)
                    {
                        using (errorStream)
                        using (StreamReader errorReader = new StreamReader(errorStream))
                        {
                            errorBody = errorReader.ReadToEnd();
                        }
                    }

                    throw new InvalidOperationException(
                        $"Сервер вернул ошибку {(int)errorResponse.StatusCode} ({errorResponse.StatusCode}) для {url}. {errorBody}");
                }

                throw new InvalidOperationException($"Не удалось выполнить запрос к {url}: {ex.Message}");
            }
        }
    }
}