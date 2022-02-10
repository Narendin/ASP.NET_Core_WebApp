using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lesson1
{
    internal class Program
    {
        private static void Main()
        {
            var tasks = new List<Task<Post>>();
            for (var i = 4; i <= 13; i++)
            {
                tasks.Add(GetPostAsync(i));
            }

            var results = Task.WhenAll(tasks);

            var fs = new FileStream("results.txt", FileMode.OpenOrCreate);

            using (var sw = new StreamWriter(fs))
            {
                foreach (var result in results.Result)
                {
                    sw.WriteLine(result);
                }
            }

            Console.WriteLine("Посты сохранены в файле result.txt");
            Console.WriteLine("Для выхода из программы нажмите любую клавишу...");
            Console.ReadKey(true);
        }

        private static async Task<Post> GetPostAsync(int postNum)
        {
            using var client = new HttpClient();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(10000);
            var uri = new Uri($"https://jsonplaceholder.typicode.com/posts/{postNum}");
            var postString = await client.GetStringAsync(uri, cts.Token);
            Console.WriteLine($"Пост номер {postNum} получен.");
            return new Post(postString);
        }
    }
}