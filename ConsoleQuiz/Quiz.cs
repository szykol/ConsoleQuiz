using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleQuiz
{
    class Quiz
    {
        Random random = new Random();
        int questionNumber = 1;

        public delegate object toPrint<T>(T obj);

        public void Start()
        {
            var quizApi = new QuizAPI();
            var categories = quizApi.GetCategoryList();

            Console.WriteLine("Choose your category");
            var categoryIndex = FetchIndexFromList(categories);
            quizApi.SetCategory(categoryIndex);

            Console.Clear();
            Console.Write("Amount of questions: ");
            int amount;
            while (!int.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Enter a valid number!");
                Console.Write("Amount of questions: ");
            }
            quizApi.SetQuestionAmount(amount);

            var diffs = Enum.GetValues(typeof(QuizAPI.Difficulty)).Cast<QuizAPI.Difficulty>().ToList();
            Console.Clear();
            Console.WriteLine("Choose your difficulty");
            var diffIndex = FetchIndexFromList(diffs);

            quizApi.SetDifficulty((QuizAPI.Difficulty)diffIndex);

            var response = quizApi.GetQuestions();

            foreach (QuizQuestion question in response)
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

        
    }
}
