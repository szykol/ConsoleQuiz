using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {

            var json = new WebClient().DownloadString("https://opentdb.com/api.php?amount=10&category=18");
            var m = JsonConvert.DeserializeObject<QuizResponse>(json);

            foreach (QuizQuestion question in m.results)
            {
                Console.WriteLine(question.question);
            }

            // Bad Boys
            Console.ReadLine();
        }
    }
}
