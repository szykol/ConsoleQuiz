using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleQuiz
{
    /// <summary>
    /// Manages questions returned from QuizAPI class
    /// Counts the score. 
    /// </summary>
    public class QuestionManager
    {
        QuizQuestion[] questions;
        int currentQuestionIndex = 0;
        int lastAnsweredQuestionIndex = 0;
        int score = 0;

        public int Count { get { return questions.Length; } }
        public int Score { get { return score; } }

        /// <summary>
        /// Gets initialized with question array
        /// </summary>
        /// <param name="questionsList">Array of questions</param>
        public QuestionManager(QuizQuestion[] questionsList)
        {
            questions = questionsList;
        }

        /// <summary>
        /// Returns current question
        /// </summary>
        /// <returns>Returns the question that is currently managed by QuestionManager</returns>
        QuizQuestion CurrentQuestion()
        {
            return questions[currentQuestionIndex - 1];
        }

        /// <summary>
        /// Returns next question
        /// </summary>
        /// <returns>Next question from the question array</returns>
        public QuizQuestion NextQuestion()
        {
            return questions[currentQuestionIndex++];
        }

        /// <summary>
        /// Checks if has a next question
        /// </summary>
        /// <returns>Returns true if there is at least one question left</returns>
        public bool HasNextQuestion()
        {
            return currentQuestionIndex < Count;
        }

        /// <summary>
        /// Checks if the answer is correct. Updates the score
        /// </summary>
        /// <param name="answer">answer string</param>
        /// <returns>True if answer is correct</returns>
        public bool CheckAnswer(string answer)
        {
            if (currentQuestionIndex >= 0 && lastAnsweredQuestionIndex != currentQuestionIndex && CurrentQuestion().correct_answer == answer)
            {
                score++;
                lastAnsweredQuestionIndex = currentQuestionIndex;
                return true;
            }

            return false;
        }

    }
}
