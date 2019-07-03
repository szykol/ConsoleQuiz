using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleQuiz
{
    public class QuestionManager
    {
        QuizQuestion[] questions;
        int currentQuestionIndex = 0;
        int lastAnsweredQuestionIndex = 0;
        int score = 0;

        public int Count { get { return questions.Length; } }
        public int Score { get { return score; } }

        public QuestionManager(QuizQuestion[] questionsList)
        {
            questions = questionsList;
        }

        QuizQuestion CurrentQuestion()
        {
            return questions[currentQuestionIndex - 1];
        }

        public QuizQuestion NextQuestion()
        {
            return questions[currentQuestionIndex++];
        }

        public bool HasNextQuestion()
        {
            return currentQuestionIndex < Count;
        }

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
