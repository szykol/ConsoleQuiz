using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace ConsoleQuiz
{
    class QuizAPI
    {
        List<CategoriesList.Category> categoriesList = null;
        QuizQuestion[] questionList = null;
        string url = $"https://opentdb.com/api.php?";

        int categoryIndex = 0;
        int questionAmount = 10;
        int difficultyIndex = 0;

        static List<string> difficulties = new List<string> { "Any", "Easy", "Medium", "Hard" };

        public List<string> GetCategoryList()
        {
            if (categoriesList == null)
            {
                categoriesList = DownloadCategories().trivia_categories;
                categoriesList.Insert(0, new CategoriesList.Category() { name = "Any" });
            }

            return categoriesList.Select(category => category.name).ToList();
        }

        public void SetCategory(int categoryListIndex)
        {
            if (categoryListIndex > 0 && categoryListIndex <= categoriesList.Count)
            {
                categoryIndex = categoryListIndex;
            }
        }

        public int GetQuestionAmount()
        {
            return questionAmount;
        }

        public void SetQuestionAmount(int amount)
        {
            if (amount > 0)
            {
                questionAmount = amount;
            }
        }

        public List<string> GetDifficultiesList()
        {
            return difficulties;
        }

        public void SetDifficulty(int difficultyListIndex)
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

        public QuizQuestion[] GetQuestions()
        {
            questionList = DownloadQuestions();
            return questionList;
        }

        CategoriesList DownloadCategories()
        {
            var json = FetchJSON("https://opentdb.com/api_category.php");
            var categories = JsonConvert.DeserializeObject<CategoriesList>(json);

           
            return categories;
        }

        QuizQuestion[] DownloadQuestions()
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

        
        string GenerateAPIUrl()
        {
            var category = categoriesList[categoryIndex];

            var url = this.url;
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
    }
}
