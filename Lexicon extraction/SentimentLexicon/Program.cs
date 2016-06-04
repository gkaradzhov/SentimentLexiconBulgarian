using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SentimentLexicon.Data;

namespace SentimentLexicon
{
    class Program
    {
        public static List<string> StopWords; 
        static void Main(string[] args)
        {
            StopWords = Utils.GetStopWords();
            var data = XmlReader.ReadData();

            //var dictionary = CreateDictionary(data);
            //Utils.SaveDictionary(dictionary);

            var dictionary = Utils.ReadDictionary();
            Lexicon lexicon = new Lexicon(dictionary, data);
            lexicon.Terms.OrderBy(s => s.Sentiment);
            lexicon.SaveLexicon("graboLexicon.txt");
            Console.WriteLine();
        }

        private static List<DictionaryItem> CreateDictionary(Grabo data)
        {
            var words = data.Businesses
                .Select(s => s.ReviewList)
                .SelectMany(s => s.Reviews)
                .Select(s => s.TokenizedMessage)
                .SelectMany(str => str)
                .GroupBy(g => g)
                .Select(gr => new DictionaryItem
                {
                    Word = gr.Key,
                    NumberOfOccurances = gr.Count()
                })
                .Where(w => w.NumberOfOccurances >= 5)
                .ToList();

            return words;    
        }
    }
}
