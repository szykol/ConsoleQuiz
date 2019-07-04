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
        public string Category {get; set; }
        public string Type { get; set; }
        public string Difficulty {get; set; }
        public string Question {get; set; }
        public string CorrectAnswer {get; set; }
        public string[] IncorrectAnswers {get; set; }
    }
}
