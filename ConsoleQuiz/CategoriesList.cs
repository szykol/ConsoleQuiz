using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleQuiz
{
    /// <summary>
    /// Struct used to parse the json.
    /// Holds the list of categories with their name
    /// and their ids.
    /// </summary>
    struct CategoriesList
    {
        public struct Category
        {
            public string id;
            public string name;
        }

        public List<Category> Categories { set; get; }
    }
}
