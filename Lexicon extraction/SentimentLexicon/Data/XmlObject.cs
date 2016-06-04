using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SentimentLexicon
{
    [XmlRoot("grabo")]
    public class Grabo
    {
        [XmlElement("business")]
        public List<Business> Businesses { get; set; }
    }

    public class Business
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("category")]
        public string Category { get; set; }

        [XmlElement("reviews")]
        public ReviewList ReviewList { get; set; }
    }

    public class ReviewList
    {
        [XmlElement("review")]
        public List<Review> Reviews { get; set; }
    }

    public class Review
    {
        private string _rawMessage;
        private string _rawRating;

        [XmlElement("rating")]
        public string Rating {
            get { return _rawRating; }
            set
            {
                _rawRating = value;
                var parsedInt = 0;
                int.TryParse(_rawRating, out parsedInt);
                RatingInt = parsedInt;

            }
        }

        public int RatingInt { get; set; }

        [XmlElement("message")]
        public string Message {
            get { return _rawMessage; }
            set
            {
                _rawMessage = value;
                TokenizedMessage = _rawMessage.Tokenize();
            }
        }

        public List<string> TokenizedMessage { get; set; }

    }
}
