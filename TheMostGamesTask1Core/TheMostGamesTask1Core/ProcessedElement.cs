using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace TheMostGamesTask1
{

    /*
     * This class is responsible for keeping data on received string and automatically process them, 
     * counting amount of words and vowels in that string.
     */
    class ProcessedElement
    {
        private
        StringBuilder _textData = new StringBuilder();
        int _wordAmount;
        int _vowelAmount;

        /*
         * Processing string.
         */
        bool ProcessString()
        {
            try
            {
                if (_textData != null)
                {
                    WordAmount = CountWordsFromPunctuationallyCorrectString();
                    VowelAmount = CountVowelsInString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return false;
            }

            return true;
        }

        /*
         * Replacing punctuation characters with single "space" character, including double spacing,
         * then counting amount of words.
         */
        int CountWordsFromPunctuationallyCorrectString()
        {

            string text = _textData.ToString();

            string[] punctuationAwayV2 = new string[] { ".",",", ",", "!", ";", "?", ":", "\'", "\"","—","  "};
            foreach (string punctuation in punctuationAwayV2)
            {
               text.Replace(punctuation, " ");
            }

            string[] textArray = text.Split(new char[] { ' ' });

            return textArray.Length;
        }

        /*
         * Counting amount of vowels through appliance of regular expression, 
         * that filters anything, that is not vowerls of russian and european languages.
         */
        int CountVowelsInString()
        {
            
            int count = Regex.Matches(_textData.ToString(), @"[ауоыиэяюёеaeiouyåäöéαεϵιουæøåíiéeáaóoúuiueɛɔaāēīūąęyųūüöąęóĩɪ̃ẽẽ̞ɛũʊ̃õõ̞ɔəɐə̃ɐ̃äaɐ̞ăâáäéíóôúýåäöøeɛɛ̃œəœ̃ɔɑɔ̃ɑ̃áéěíóúůýõäöüӧі]", RegexOptions.IgnoreCase).Count;

            return count;
        }

        /*
         * Property for text from JSON.
         */
        public String TextData
        {
            get
            {
                return _textData.ToString();
            }
            set
            {
                if (value != null)
                {
                    _textData.Append(value);
                }

            }
        }

        /*
         * Property for amount of words in text from JSON.
         */
        public int WordAmount
        {
            get
            {
                return _wordAmount;
            }
            set
            {
                _wordAmount = value;
            }
        }

        /*
         * Property for amount of vowels in text from JSON.
         */
        public int VowelAmount
        {
            get
            {
                return _vowelAmount;
            }
            set
            {
                _vowelAmount = value;
            }
        }

        /*
         * Constructor, that automatically launches string processing.
         */
        public ProcessedElement(string incomingData)
        {
            TextData = incomingData;
            ProcessString();
        }

    }
}
