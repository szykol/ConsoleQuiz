using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleQuiz
{
    /// <summary>
    /// Basic struct that holds all question data.
    /// </summary>
    public struct QuizQuestion
    {
        public string category;
        public string type;
        public string difficulty;
        public string question;
        public string correct_answer;
        public string[] incorrect_answers;
    }
}
