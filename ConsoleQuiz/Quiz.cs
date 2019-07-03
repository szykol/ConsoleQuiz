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

            var qManager = quizApi.GetQuestionManager();
            while(qManager.HasNextQuestion())
            {
                Console.Clear();

                var question = qManager.NextQuestion();
                Console.WriteLine($"Question #{questionNumber++}: {question.question} | Score [{qManager.Score}/{qManager.Count}]");

                var answers = CreateResultsList(question);
                var index = FetchIndexFromList(answers);

                if (qManager.CheckAnswer(answers[index]))
                    Console.WriteLine("Correct!");
                else
                    Console.WriteLine("Incorrect");

                Console.ReadLine();

            }

            Console.Clear();
            Console.WriteLine($"Good Game. Your score is {qManager.Score}");
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
