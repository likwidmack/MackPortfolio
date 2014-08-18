using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MackPortfolio.Prototyping
{
    class TextGenerator
    {
        private string getLoremIpsum(int count)
        {
            var _string = String.Empty;
            var words = Resources.Lorem.LoremIpsum.Split(Config.Separator);

            var numWords = RandomNumber.Next(count) + 1;
            for (int w = 0; w < numWords; w++)
            {
                if (w > 0) { _string += " "; }
                _string += words[RandomNumber.Next(words.Length)];
            }

            return _string;
        }
        private string getRandomWord(int count)
        {
            var _string = String.Empty;
            var words = Resources.Lorem.Words.Split(Config.Separator);

            var numWords = RandomNumber.Next(count) + 1;
            for (int w = 0; w < numWords; w++)
            {
                if (w > 0) { _string += " "; }
                _string += words[RandomNumber.Next(words.Length)];
            }

            return _string;
        }

    }
    public static class Lorem
    {
        /// <summary>
        /// Get a random collection of words.
        /// </summary>
        /// <param name="count">Number of words required</param>
        /// <returns></returns>
        public static IEnumerable<string> Ipsum(int count)
        {
            if (count <= 0) count=1;

            return count.Times(x => Resources.Lorem.LoremIpsum.Split(Config.Separator).Random());
        }

        /// <summary>
        /// Get a random collection of words.
        /// </summary>
        /// <param name="count">Number of words required</param>
        /// <returns></returns>
        public static IEnumerable<string> Words(int count)
        {
            if (count <= 0) count = 1; ;

            return count.Times(x => Resources.Lorem.Words.Split(Config.Separator).Random());
        }

        /// <summary>
        /// Get the first word of the random word collection. Useful for unit tests.
        /// </summary>
        /// <returns></returns>
        public static string GetFirstWord()
        {
            return Resources.Lorem.LoremIpsum.Split(Config.Separator).First();
        }

        /// <summary>
        /// Generates a Capitalized Title of random lorem ipsum words.
        /// </summary>
        /// <param name="minWordCount">Minimum number of words required</param>
        /// <returns></returns>
        public static string Title(int minWordCount, int maxWordCount = 6)
        {
            if (minWordCount <= 0) minWordCount = 1;
            if (maxWordCount < minWordCount) maxWordCount = minWordCount + 1 * 2;

            return string.Join(" ", Ipsum(minWordCount + RandomNumber.Next(maxWordCount)).ToArray()).Capitalise(true);
        }

        public static string Title()
        {
            return Title(4);
        }

        /// <summary>
        /// Generates a simple phrase of random words.
        /// </summary>
        /// <param name="minWordCount">Minimum number of words required</param>
        /// <returns></returns>
        public static string Phrase(int minWordCount)
        {
            if (minWordCount <= 0) minWordCount = 1;

            return string.Join(" ", Words(minWordCount + RandomNumber.Next(6)).ToArray());
        }

        public static string Phrase()
        {
            return Phrase(4);
        }

        /// <summary>
        /// Generates a capitalised sentence of random words.
        /// </summary>
        /// <param name="minWordCount">Minimum number of words required</param>
        /// <returns></returns>
        public static string Sentence(int minWordCount)
        {
            if (minWordCount <= 0) minWordCount = 1;

            return string.Join(" ", Words(minWordCount + RandomNumber.Next(6)).ToArray()).Capitalise() + ".";
        }

        public static string Sentence()
        {
            return Sentence(4);
        }

        public static IEnumerable<string> Sentences(int sentenceCount)
        {
            if (sentenceCount <= 0) sentenceCount = 1;

            return sentenceCount.Times(x => Sentence());
        }

        public static string Paragraph(int minSentenceCount)
        {
            if (minSentenceCount <= 0) minSentenceCount = 1;

            return string.Join(" ", Sentences(minSentenceCount + RandomNumber.Next(3)).ToArray());
        }

        public static string Paragraph()
        {
            return Paragraph(3);
        }

        public static IEnumerable<string> Paragraphs(int paragraphCount)
        {
            if (paragraphCount <= 0) paragraphCount = 1;

            return paragraphCount.Times(x => Paragraph());
        }
    }

}
