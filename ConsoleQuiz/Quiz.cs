using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleQuiz
{
    /// <summary>
    /// Handles the interaction with the user.
    /// Uses the QuizAPI to get questions
    /// </summary>
    class Quiz
    {
        Random random = new Random();
        int questionNumber = 1;

        public delegate object toPrint<T>(T obj);

        /// <summary>
        /// Starts the quiz game
        /// </summary>
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

        /// <summary>
        /// Gets all answers and generates a list of them
        /// </summary>
        /// <param name="q">QuizQuestion struct used for holding the question data</param>
        /// <returns>List of all possible answers</returns>
        List<string> CreateResultsList(QuizQuestion q)
        {
            var answers = new List<string>();
            answers.AddRange(q.incorrect_answers);
            answers.Insert(random.Next(0, answers.Count() + 1), q.correct_answer);

            return answers;
        }

        /// <summary>
        /// Helper method that prints contents of list
        /// and uses another helper method to get the
        /// validated index from the user
        /// </summary>
        /// <typeparam name="T">Any type that is held in the List</typeparam>
        /// <param name="list">List</param>
        /// <param name="f">Function that returns what should be print in the console</param>
        /// <returns>Returns the list index</returns>
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

        /// <summary>
        /// Helper method that gets correct index of 
        /// the list
        /// </summary>
        /// <typeparam name="T">Any type that is held in the List</typeparam>
        /// <param name="list">List</param>
        /// <returns>Returns the list index</returns>
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
