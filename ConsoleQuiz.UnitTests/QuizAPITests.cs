using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleQuiz.UnitTests
{
    [TestClass]
    public class QuizAPITests
    {
        [TestMethod]
        public void DefaultSettingsAfterInit()
        {
            QuizAPI api = new QuizAPI();

            Assert.AreEqual(10, api.GetQuestionAmount());
            Assert.AreEqual(0, api.GetCategory());
            Assert.AreEqual(QuizAPI.Difficulty.ANY, api.GetDifficulty());
        }

        [TestMethod]
        public void GetQuestions_WithDefaultSettings_ReturnsData()
        {
            QuizAPI api = new QuizAPI();

            var questions = api.GetQuestions();
            Assert.IsNotNull(questions);
            Assert.IsTrue(questions.Length > 0);
        }

        [TestMethod]
        public void SetCategory_BeforeFetchingCategories_ReturnsDefault()
        {
            QuizAPI api = new QuizAPI();

            api.SetCategory(-1);
            Assert.AreEqual(0, api.GetCategory());
        }

        [TestMethod]
        public void SetCategory_WithIncorrectValue_ReturnsDefault()
        {
            QuizAPI api = new QuizAPI();

            api.GetCategoryList();

            api.SetCategory(-1);
            Assert.AreEqual(0, api.GetCategory());

            api.SetCategory(80);
            Assert.AreEqual(0, api.GetCategory());
        }

        
    }
}
