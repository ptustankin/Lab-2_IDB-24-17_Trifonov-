using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_Task2_Async
{
    internal static class Task2Async
    {
        private static readonly HttpClient Http = new HttpClient();

        private static readonly string[] Urls =
        {
            "https://jsonplaceholder.typicode.com/todos/1",
            "https://api.agify.io/?name=michael",
            "https://api.nationalize.io/?name=michael"
        };

        public static async Task Run()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                Task<string> task1 = GetJsonAsync(Urls[0]);
                Task<string> task2 = GetJsonAsync(Urls[1]);
                Task<string> task3 = GetJsonAsync(Urls[2]);

                string[] responses = await Task.WhenAll(task1, task2, task3);

                for (int i = 0; i < responses.Length; i++)
                {
                    Console.WriteLine($"Ответ {i + 1}:");
                    Console.WriteLine(responses[i]);
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

        private static async Task<string> GetJsonAsync(string url)
        {
            using (HttpResponseMessage response = await Http.GetAsync(url))
            {
                string body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(
                        $"Сервер вернул ошибку {(int)response.StatusCode} ({response.ReasonPhrase}) для {url}. {body}");
                }

                return body;
            }
        }
    }
}