using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TheMostGamesTask1
{
    /*
     * Class for transforming JSON into object to work with.
     */
    class ClassForTextReceiving
    {
        [JsonPropertyName("text")]
        public String Text { get; set; }
    }
}
