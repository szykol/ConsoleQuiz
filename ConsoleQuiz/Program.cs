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
            var random = new Random();
            foreach (QuizQuestion question in m.results)
            {
                Console.WriteLine(question.question);

                var answers = new List<string>();
                answers.AddRange(question.incorrect_answers);
                answers.Insert(random.Next(0, answers.Count() + 1),question.correct_answer);
                
                var i = 0;
                foreach(var answer in answers)
                {
                    Console.WriteLine($"{++i}. {answer}");
                }

                Console.Write("> ");
                var index = int.Parse(Console.ReadLine());

                if(answers[index - 1] == question.correct_answer)
                {
                    Console.WriteLine("Correct!");
                }
                else
                {
                    Console.WriteLine("Incorrect!");
                }
            }

            // Bad Boys
            Console.ReadLine();
        }
    }
}
