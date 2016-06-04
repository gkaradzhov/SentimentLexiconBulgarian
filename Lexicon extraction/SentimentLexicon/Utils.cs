using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BulStem;
using SentimentLexicon.Crossvalidation;
using SentimentLexicon.Data;

namespace SentimentLexicon
{
    static class Utils
    {
        public static string Stem(this string input)
        {
            var stemmer = new BulStem.Stemmer();
            string stemLevel2 = stemmer.Stem(input);
            return stemLevel2;
        }

        public static List<string> Tokenize(this string input)
        {
            var result = input.Split(new char[] { ' ', '.', ',', '?' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            result = result.Select(str => str.Stem().ToLower()).ToList();
            result.RemoveAll(s => s.StopWord());
            return result;
        }

        public static DetailedAnalysis ProcessReview(this List<string> input, List<Term> lexicon)
        {
            var processedWords = 0;
            decimal currentScore = 0;
            var detailedAnalyses = new DetailedAnalysis();

            foreach (var token in input)
            {

                var emoji = ParseEmoji(token);
                if (emoji != 0)
                {
                    detailedAnalyses.PositiveEmoji += emoji > 0 ? 1 : 0;
                    detailedAnalyses.NegativeEmoji += emoji < 0 ? 1 : 0;
                }
                else
                {
                    var tempScore = GetScore(token, lexicon);
                    if (tempScore != 0)
                    {
                        detailedAnalyses.Tokens.Add(new Term()
                        {
                            Sentiment = (double)tempScore,
                            Text= token
                        });
                    }
                }
            }


            return detailedAnalyses;
        }

        public static decimal GetScore(this string token, List<Term> lexicon)
        {
            var lexiconToken = lexicon.FirstOrDefault(s => s.Text == token);
            if (lexiconToken != null)
            {
                return (decimal)lexiconToken.Sentiment;
            }
            return 0;
        }

        public static decimal ParseEmoji(this string input)
        {
            return Regex.Matches(input, @"(?<a>:[\)+\}+\]+\>+])|(?<b>:[\(+\{+\[+\<+|+])")
                 .Cast<Match>()
                 .Select(m => m.Groups["a"].Success ? 5 : -5)
                 .Sum();
        }

        public static void SaveDictionary(List<DictionaryItem> dictionary)
        {
            using (StreamWriter outputFile = new StreamWriter("myData.txt"))
            {
                foreach (var line in dictionary)
                    outputFile.WriteLine(string.Format("{0},{1}", line.Word, line.NumberOfOccurances));
            }
        }

        public static List<string> GetStopWords()
        {
            var stopWords = new List<string>();
            var path = "stopwords_bg.txt";
            stopWords = File.ReadAllText(path).Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            return stopWords;
        }

        public static bool StopWord(this string token)
        {
            return Program.StopWords.Contains(token);
        }

        internal static List<DictionaryItem> ReadDictionary()
        {
            var dictionary = new List<DictionaryItem>();
            var path = "myData.txt";
            dictionary = File.ReadAllText(path).Split(new string[] { "\r\n" }, StringSplitOptions.None).Select(s => s.Split(',')[0]).Select(s => new DictionaryItem{Word = s}).ToList();
            return dictionary;
        }
    }
}
