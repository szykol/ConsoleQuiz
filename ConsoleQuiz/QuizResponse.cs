using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleQuiz
{
    /// <summary>
    /// Struct used for parsing the json data from API
    /// </summary>
    struct QuizResponse
    {
        public int response_code;
        public QuizQuestion[] results;
    }
}
