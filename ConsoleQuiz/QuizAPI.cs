using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// Gets all categories from online API and returns them
        /// in a list of strings
        /// </summary>
        /// <returns>List of Quiz Categories</returns>
        public List<string> GetCategoryList()
        {
            if (categoriesList == null)
            {
                categoriesList = DownloadCategories().Categories;
                categoriesList.Insert(0, new CategoriesList.Category() { name = "Any" });
            }

            return categoriesList.Select(category => category.name).ToList();
        }

        /// <summary>
        /// Sets the question category
        /// </summary>
        /// <param name="categoryListIndex">An index of category list returned from <c>GetCategoryList()</c></param>
        public void SetCategory(int categoryListIndex)
        {
            if (categoriesList != null && categoryListIndex > 0 && categoryListIndex <= categoriesList.Count)
            {
                categoryIndex = categoryListIndex;
            }
        }

        /// <summary>
        /// Returns the category index
        /// </summary>
        /// <returns>Index of category list returned from <c>GetCategoryList()</c></returns>
        public int GetCategory()
        {
            return categoryIndex;
        }

        /// <summary>
        /// Returns the question amount
        /// </summary>
        /// <returns>Amount of question that will be downloaded from the API</returns>
        public int GetQuestionAmount()
        {
            return questionAmount;
        }

        /// <summary>
        /// Sets the question amount
        /// </summary>
        /// <param name="amount">Amount of question that will be downloaded from the API</param>
        public void SetQuestionAmount(int amount)
        {
            if (amount > 0)
            {
                questionAmount = amount;
            }
        }

        /// <summary>
        /// Sets the question difficulty
        /// </summary>
        /// <param name="d">New difficulty</param>
        public void SetDifficulty(Difficulty d)
        {
            difficulty = d;
        }

        /// <summary>
        /// Returns the question difficulty
        /// </summary>
        /// <returns>The difficulty of question that will be downloaded from the API</returns>
        public Difficulty GetDifficulty()
        {
            return difficulty;
        }

        /// <summary>
        /// Exits the app with some string messages
        /// </summary>
        /// <param name="messages">Optional strings to describe the error</param>
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

        /// <summary>
        /// Downloads and returns array of questions
        /// </summary>
        /// <returns>Array of questions</returns>
        public QuizQuestion[] GetQuestions()
        {
            questionList = DownloadQuestions();
            return questionList;
        }

        /// <summary>
        /// Returns QuestionManager and initializes it with question array
        /// </summary>
        /// <returns>QuestionManager that holds all questions and counts score</returns>
        public QuestionManager GetQuestionManager()
        {
            return new QuestionManager(DownloadQuestions());
        }

        /// <summary>
        /// Helper function that downloads categories from the API
        /// </summary>
        /// <returns>Categories list</returns>
        CategoriesList DownloadCategories()
        {
            var json = FetchJSON("https://opentdb.com/api_category.php");
            var categories = JObject.Parse(json);

            JArray array = (JArray)categories["trivia_categories"];

            return new CategoriesList()
            {
                Categories = array.Select(cat => new CategoriesList.Category()
                {
                    id = (string)cat["id"],
                    name = WebUtility.HtmlDecode((string)cat["name"])
                }).ToList()
            };
        }

        /// <summary>
        /// Helper function that downloads question from the API
        /// </summary>
        /// <returns>Array of questions</returns>
        QuizQuestion[] DownloadQuestions()
        {
            Func<string, string> decode = s => WebUtility.HtmlDecode((string)s);

            url = GenerateAPIUrl();
            var json = FetchJSON(url);
            var response = JObject.Parse(json);


            if ((int)response["response_code"] != 0)
            {
                QuizAPI.ExitApp("Invalid response. Questions could not be downloaded.");
            }

            JArray questionArray = (JArray)response["results"];
            return questionArray.Select(q => new QuizQuestion()
            {
                Category = decode((string)q["category"]),
                Type = decode((string)q["type"]),
                Difficulty = decode((string)q["difficulty"]),
                Question = decode((string)q["question"]),
                CorrectAnswer = decode((string)q["correct_answer"]),
                IncorrectAnswers = ((JArray)q["incorrect_answers"]).Select(cat => decode((string)cat)).ToArray()
            }).ToArray();
        }
        
        /// <summary>
        /// Helper method that generates API url
        /// </summary>
        /// <returns>Url for the API</returns>
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

        /// <summary>
        /// Downloads JSON from the API. 
        /// If an error occurs, exits the app for now
        /// </summary>
        /// <param name="url">url for the API</param>
        /// <returns>JSON</returns>
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
