using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace ConsoleQuiz
{
    static class QuizAPI
    {
        static List<CategoriesList.Category> categoriesList = null;
        static QuizQuestion[] questionList = null;
        static string url = $"https://opentdb.com/api.php?";

        static int categoryIndex = 0;
        static int questionAmount = 10;
        static int difficultyIndex = 0;

        static List<string> difficulties = new List<string> { "Any", "Easy", "Medium", "Hard" };

        public static List<string> GetCategoryList()
        {
            if (categoriesList == null)
            {
                categoriesList = DownloadCategories().trivia_categories;
                categoriesList.Insert(0, new CategoriesList.Category() { name = "Any" });
            }

            return categoriesList.Select(category => category.name).ToList();
        }

        public static void SetCategory(int categoryListIndex)
        {
            if (categoryListIndex > 0 && categoryListIndex <= categoriesList.Count)
            {
                categoryIndex = categoryListIndex;
            }
        }

        public static int GetQuestionAmount()
        {
            return questionAmount;
        }

        public static void SetQuestionAmount(int amount)
        {
            if (amount > 0)
            {
                questionAmount = amount;
            }
        }

        public static List<string> GetDifficultiesList()
        {
            return difficulties;
        }

        public static void SetDifficulty(int difficultyListIndex)
        {
            if (difficultyListIndex > 0 && difficultyListIndex <= difficulties.Count)
            {
                difficultyIndex = difficultyListIndex;
            }
        }

        public static void ExitApp(params string[] messages)
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

        public static QuizQuestion[] GetQuestions()
        {
            questionList = DownloadQuestions();
            return questionList;
        }

        static CategoriesList DownloadCategories()
        {
            var json = FetchJSON("https://opentdb.com/api_category.php");
            var categories = JsonConvert.DeserializeObject<CategoriesList>(json);

           
            return categories;
        }

        static QuizQuestion[] DownloadQuestions()
        {
            url = GenerateAPIUrl();
            var json = FetchJSON(url);
            var response = JsonConvert.DeserializeObject<QuizResponse>(json);

            if (response.response_code != 0)
            {
                QuizAPI.ExitApp("Invalid response. Questions could not be downloaded.");
            }

            return response.results;
        }

        
        static string GenerateAPIUrl()
        {
            var category = categoriesList[categoryIndex];

            var url = QuizAPI.url;
            if ( category.name != "Any")
            {
                url += $"&category={category.id}";
            }

            var difficulty = difficulties[difficultyIndex];

            if (difficulty != "Any")
            {
                url += $"&difficulty={difficulty.ToLower()}";
            }

            url += $"&amount={questionAmount}";

            return url;
        }

        static string FetchJSON(string url)
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
    }
}
