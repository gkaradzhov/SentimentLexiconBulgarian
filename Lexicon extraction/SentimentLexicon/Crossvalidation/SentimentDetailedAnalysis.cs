using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentLexicon.Crossvalidation
{
    public class DetailedAnalysis
    {
        private const decimal POLARITY_COEFICIENT = 0.65m;
        private const decimal EMOJI_FACTOR = 1m;

        public DetailedAnalysis()
        {
            Tokens = new List<Term>();
        }

        public List<Term> Tokens { get; set; }
        public Term BestScore
        {
            get
            {
                if (Tokens.Count >= 2)
                {
                    return Tokens.FirstOrDefault(t => t.Sentiment == Tokens.Max(s => s.Sentiment));
                }
                else
                {
                    return new Term();
                }
            }
        }
        public Term WorstScore
        {
            get
            {
                if (Tokens.Count >= 2)
                {
                    return Tokens.FirstOrDefault(t => t.Sentiment == Tokens.Min(s => s.Sentiment));
                }
                else
                {
                    return new Term();
                }
            }
        }
        public int PositiveEmoji { get; set; }
        public int NegativeEmoji { get; set; }
        public decimal AvarageScore
        {
            get
            {
                if (Tokens.Count > 0)
                {
                    return (decimal)Tokens.Average(s => s.Sentiment);
                }
                return 0;
            }
        }
        public decimal AbsoluteSentiment
        {
            get
            {
                decimal tempScore = 0;
                tempScore += (decimal)(BestScore.Sentiment + WorstScore.Sentiment) * POLARITY_COEFICIENT;
                if (PositiveEmoji != NegativeEmoji)
                {
                    tempScore += PositiveEmoji > NegativeEmoji ? EMOJI_FACTOR : -EMOJI_FACTOR;
                }
                tempScore += AvarageScore * (1 - POLARITY_COEFICIENT);

                return tempScore;
            }
        }

        public string DetailsSummary
        {
            get
            {
                var resultString = string.Empty;

                resultString += string.Format("Number of recognized tokens: {0}, {1}", Tokens.Count, Environment.NewLine);
                resultString += string.Format("Most positive: {0} : {1} {2}", BestScore.Text, BestScore.Sentiment, Environment.NewLine);
                resultString += string.Format("Most Negative: {0} : {1} {2}", WorstScore.Text, WorstScore.Sentiment, Environment.NewLine);
                resultString += string.Format("Average: {0} {1}", AvarageScore, Environment.NewLine);
                resultString += string.Format("PositiveEmojis: {0}, Negative Emojis : {1} {2}", PositiveEmoji, NegativeEmoji, Environment.NewLine);
                resultString += string.Format("All evaluated tokens: {0}", Environment.NewLine);
                resultString += Tokens.Select(t => string.Format("{0} : {1} {2}", t.Text, t.Sentiment, Environment.NewLine)).Aggregate("", (x, y) => x + y);

                return resultString;
            }
        }
    }
}
