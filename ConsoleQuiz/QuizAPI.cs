using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace ConsoleQuiz
{
    public class QuizAPI
    {
        public enum Difficulty { ANY = 0, EASY, MEDIUM, HARD };
        List<CategoriesList.Category> categoriesList = null;
        QuizQuestion[] questionList = null;
        string url = $"https://opentdb.com/api.php?";

        int categoryIndex = 0;
        int questionAmount = 10;
        Difficulty difficulty = Difficulty.ANY;

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
            if (categoriesList != null && categoryListIndex > 0 && categoryListIndex <= categoriesList.Count)
            {
                categoryIndex = categoryListIndex;
            }
        }

        public int GetCategory()
        {
            return categoryIndex;
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

        public void SetDifficulty(Difficulty d)
        {
            difficulty = d;
        }

        public Difficulty GetDifficulty()
        {
            return difficulty;
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
            var url = this.url;
            if (categoriesList != null)
            {
                var category = categoriesList[categoryIndex];
                if ( category.name != "Any")
                    url += $"&category={category.id}";
            }

            if (difficulty != Difficulty.ANY)
                url += $"&difficulty={difficulty.ToString().ToLower()}";

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
