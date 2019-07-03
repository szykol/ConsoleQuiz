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

        [TestMethod]
        public void SetDifficulty_ChangesState()
        {
            QuizAPI api = new QuizAPI();

            api.SetDifficulty(QuizAPI.Difficulty.HARD);
            Assert.AreEqual(QuizAPI.Difficulty.HARD, api.GetDifficulty());
        }

        [TestMethod]
        public void SetQuestionAmount_ChangesState()
        {
            QuizAPI api = new QuizAPI();

            api.SetQuestionAmount(15);
            Assert.AreEqual(15, api.GetQuestionAmount());
        }

        [TestMethod]
        public void SetQuestionAmount_WithIncorrectValue_DoesntChangeState()
        {
            QuizAPI api = new QuizAPI();

            api.SetQuestionAmount(-5);
            Assert.AreEqual(10, api.GetQuestionAmount());
        }

        [TestMethod]
        public void GetQuestions_WithAmount_DownloadsCorrectAmountOfQuestions()
        {
            QuizAPI api = new QuizAPI();

            api.SetQuestionAmount(20);
            var data = api.GetQuestions();

            Assert.AreEqual(20, data.Length);

            api.SetQuestionAmount(5);
            data = api.GetQuestions();

            Assert.AreEqual(5, data.Length);
        }
    }
}