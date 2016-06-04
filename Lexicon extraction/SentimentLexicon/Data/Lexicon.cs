using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SentimentLexicon.Data;

namespace SentimentLexicon
{
    class Lexicon
    {
        public Lexicon()
        {

        }

        public Lexicon(List<DictionaryItem> dictionary, Grabo graboData)
        {
            Terms = new List<Term>();
            var reviews = graboData.Businesses.Select(ss => ss.ReviewList).SelectMany(m => m.Reviews).ToList();
            dictionary.ForEach(s => Terms.Add(CalculatePMI(s,reviews)));
        }

        public Lexicon(List<DictionaryItem> dictionary, Grabo graboData, string forCategory)
        {

        }

        public void SaveLexicon(string fileName)
        {
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                foreach (var line in Terms)
                    outputFile.WriteLine(string.Format("{0}, {1}", line.Text, line.Sentiment));
            }
        }

        public List<Term> Terms { get; set; }

        private Term CalculatePMI(DictionaryItem term, List<Review> graboData)
        {
            var goodReviews = graboData.Where(s => s.RatingInt >= 4);
            var badRevies = graboData.Where(s => s.RatingInt > 0 && s.RatingInt <= 2);
            var totalDocumentCount = graboData.Count;
            var goodCount = goodReviews.Select(s => s.TokenizedMessage).SelectMany(str => str).Count(s => s == term.Word) + 0.0001;
            var badCount = badRevies.Select(s => s.TokenizedMessage).SelectMany(str => str).Count(s => s == term.Word) + 0.0001;
            var totalGood = goodReviews.Count();
            var totalBad = badRevies.Count();

            var equation = (goodCount*totalBad)/(badCount*totalGood);


            var pmi = Math.Log(equation, 2);

            return new Term { Sentiment = pmi, Text = term.Word };
        }
    }

}
