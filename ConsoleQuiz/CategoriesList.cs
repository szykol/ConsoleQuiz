using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleQuiz
{
    struct CategoriesList
    {
        public struct Category
        {
            public string id;
            public string name;
        }

        public List<Category> trivia_categories;
    }
}
