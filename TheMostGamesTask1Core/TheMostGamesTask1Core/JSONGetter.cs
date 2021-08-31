using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TheMostGamesTask1
{
    /*
     * Class for getting JSONs from server.
     */
    class JSONGetter
    {
        private static readonly HttpClient client = new HttpClient();
        string _errorMessage;

        public JSONGetter()
        {
            /*
             * Adding right headers to requests by default at the start of the program.
             */
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("TMG-Api-Key", "0J/RgNC40LLQtdGC0LjQutC4IQ==");

        }

        /*
         * Getting and deserializing JSONs.
         */
        public async Task<ClassForTextReceiving> GetJSONAsync(string url, int strtingID)
        {
            try
            {
                var streamTask = client.GetStreamAsync(url); //Creating a task of getting JSON.

                return await JsonSerializer.DeserializeAsync<ClassForTextReceiving>(await streamTask); //Getting JSON and deserializing it into
                                                                                                       //ClassForTextReceiving object.
            }
            catch(Exception e) //If something went wrong, return null string.
            {
                _errorMessage = e.Message.ToString();
                Debug.WriteLine(e.Message.ToString());
                return null;
            }
            
        }

        /*
         * Property for getting an error message.
         */
        public string ErrorMessage 
        {
            get
            {
                return _errorMessage; 
            }
        }

    }
}