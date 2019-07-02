using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace ConsoleQuiz
{
    class Quiz
    {
        Random random = new Random();
        int questionNumber = 1;

        public void Start()
        {
            var url = "https://opentdb.com/api.php?amount=10&category=18";
            var response = DownloadData(url);

            foreach (QuizQuestion question in response.results)
            {
                Console.WriteLine($"Question #{questionNumber++}: {question.question}");

                var answers = CreateResultsList(question);

                var i = 0;
                foreach (var answer in answers)
                {
                    Console.WriteLine($"{++i}. {answer}");
                }

                var index = FetchAnswerIndex(answers);

                if (answers[index] == question.correct_answer)
                    Console.WriteLine("Correct!");
                else
                    Console.WriteLine("Incorrect!");

                Console.ReadLine();
                Console.Clear();
            }

            Console.ReadLine();
        }
        QuizResponse DownloadData(string url)
        {
            var json = new WebClient().DownloadString(url);
            var m = JsonConvert.DeserializeObject<QuizResponse>(json);

            return m;
        }
        List<string> CreateResultsList(QuizQuestion q)
        {
            var answers = new List<string>();
            answers.AddRange(q.incorrect_answers);
            answers.Insert(random.Next(0, answers.Count() + 1), q.correct_answer);

            return answers;
        }

        int FetchAnswerIndex(List<string> answers)
        {
            Console.Write("> ");
            int index;
            while (!int.TryParse(Console.ReadLine(), out index) || answers.ElementAtOrDefault(index - 1) == null)
            {
                Console.WriteLine("Enter a correct number!");
                Console.Write("> ");
            }

            return index - 1;
        }
    }
}
