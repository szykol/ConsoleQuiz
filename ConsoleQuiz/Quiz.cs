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

        public delegate object toPrint<T>(T obj);

        public void Start()
        {
            var categories = DownloadCategories().trivia_categories;
            Console.WriteLine("Choose your category");
            var categoryIndex = FetchIndexFromList(categories, (cat) => cat.name);
            var categoryID = categories[categoryIndex].id;

            var url = $"https://opentdb.com/api.php?amount=51&category={categoryID}";
            var response = DownloadQuestions(url);

            if (response.response_code != 0)
            {
                ExitApp("Invalid response. Questions could not be downloaded.");
            }

            foreach (QuizQuestion question in response.results)
            {
                Console.Clear();

                Console.WriteLine($"Question #{questionNumber++}: {question.question}");

                var answers = CreateResultsList(question);

                var index = FetchIndexFromList(answers);

                if (answers[index] == question.correct_answer)
                    Console.WriteLine("Correct!");
                else
                    Console.WriteLine("Incorrect!");

                Console.ReadLine();
            }

            Console.ReadLine();
        }
        QuizResponse DownloadQuestions(string url)
        {
            var json = FetchJSON(url);
            var m = JsonConvert.DeserializeObject<QuizResponse>(json);

            return m;
        }

        CategoriesList DownloadCategories()
        {
            var json = FetchJSON("https://opentdb.com/api_category.php");
            var categories = JsonConvert.DeserializeObject<CategoriesList>(json);

            return categories;
        }

        List<string> CreateResultsList(QuizQuestion q)
        {
            var answers = new List<string>();
            answers.AddRange(q.incorrect_answers);
            answers.Insert(random.Next(0, answers.Count() + 1), q.correct_answer);

            return answers;
        }

        int FetchIndexFromList<T>(List<T> list, toPrint<T> f = null)
        {
            var i = 0;
            foreach (var element in list)
            {
                if (f != null)
                    Console.WriteLine($"{++i}. {f(element)}");
                else
                    Console.WriteLine($"{++i}. {element}");
            }

            return FetchFromConsole(list);
        }

        int FetchFromConsole<T>(List<T> list)
        {
            Console.Write("> ");
            int index;
            while (!int.TryParse(Console.ReadLine(), out index) || !(index > 0 && index <= list.Count))
            {
                Console.WriteLine("Enter a correct number!");
                Console.Write("> ");
            }

            return index - 1;
        }

        string FetchJSON(string url)
        {
            try
            {
                var json = new WebClient().DownloadString(url);
                return json;
            }
            catch (WebException we)
            {
                ExitApp(we.Message);
            }

            return null;
        }

        void ExitApp(params string[] messages)
        {
            Console.Clear();
            foreach (var msg in messages)
            {
                Console.WriteLine(msg);
            }
            Console.WriteLine("An error has occured");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            Environment.Exit(-1);
        }
    }
}
