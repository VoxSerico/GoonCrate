using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoonCrates
{
    public enum WordSize
    {
        FIVE, SEVEN, NINE
    }
    public static class Program
    {
        static string[] wordsSize5;
        static string[] wordsSize7;
        static string[] wordsSize9;
        static string wordGroupSeparator = "@=";
        static string wordSeparator = "@,";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            BuildWordList();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void BuildWordList()
        {
            //string[] wordGroups = Properties.Resources.password_pool.ToUpper().Split(new string[] { "\r\n" }, 
            string[] wordGroups = Properties.Resources.password_pool.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < wordGroups.Length; i++)
            {
                extractWords(ref wordsSize5, wordGroups, "five", i);
                extractWords(ref wordsSize7, wordGroups, "seven", i);
                extractWords(ref wordsSize9, wordGroups, "nine", i);
            }
        }

        private static void extractWords(ref string[] wordsholder, string[] wordGroups, string word, int i)
        {
            int index = wordGroups[i].IndexOf(word + wordGroupSeparator);
            if (index != -1)
            {
                wordsholder = wordGroups[i].Substring(word.Length + wordGroupSeparator.Length).Split(new string[] { wordSeparator }, StringSplitOptions.RemoveEmptyEntries);
                return;
            }
        }

        public static List<string> GetPossibleWords(WordSize wordSize, char?[] knownLettersWithPosition, char[] excludedLetters)
        {
            if (wordSize == WordSize.FIVE && knownLettersWithPosition.Length != 5)
                throw new ArgumentException("wordSize and knownLettersWithPosition mismatch");
            if (wordSize == WordSize.SEVEN && knownLettersWithPosition.Length != 7)
                throw new ArgumentException("wordSize and knownLettersWithPosition mismatch");
            if (wordSize == WordSize.NINE && knownLettersWithPosition.Length != 9)
                throw new ArgumentException("wordSize and knownLettersWithPosition mismatch");

            List<string> possibleWords = new List<string>();
            List<string> possibleWordsRefined = new List<string>();
            GetAllWordsAndExcludeWordsBasedOnLetters(wordSize, excludedLetters, possibleWords);
            foreach(string word in possibleWords)
            {
                if (matchesPattern(word, knownLettersWithPosition))
                {
                    possibleWordsRefined.Add(word);
                }
            }
            return possibleWordsRefined;
        }

        public static char GetSuggestedLetter(WordSize wordSize, char?[] knownLettersWithPosition, char[] excludedLetters)
        {
            List<string> words = GetPossibleWords(wordSize, knownLettersWithPosition, excludedLetters);
            if(words.Count <= 1)
            {
                return ' ';
            }
            int[] letterCounter = CountLettersInWords(words);
            char suggestedLetter = GetMostCommmonLetter(knownLettersWithPosition, letterCounter);

            int index = -1;
            bool similar = true;
            foreach (string word in words)
            {
                if (index == -1)
                {
                    index = word.IndexOf(suggestedLetter);
                    continue;
                }

                if (word.IndexOf(suggestedLetter) != index)
                {
                    similar = false;
                    break;
                }
            }

            if (similar)
            {
                bool[] areSimilarLetters = new bool[knownLettersWithPosition.Length];
                string firstWord = null;
                foreach (string word in words)
                {
                    if (string.IsNullOrEmpty(firstWord))
                    {
                        firstWord = word;
                        continue;
                    }

                    for (int pos = 0; pos < word.Length; pos++)
                    {
                        areSimilarLetters[pos] = word[pos] == firstWord[pos];
                    }
                }
                List<char> similarLetters = new List<char>();
                for (int pos = 0; pos < firstWord.Length; pos++)
                {
                    if (areSimilarLetters[pos])
                    {
                        similarLetters.Add(firstWord.ToUpper()[pos]);
                    }
                }
                letterCounter = CountLettersInWords(words);
                foreach (char letter in similarLetters)
                {
                    letterCounter[letter - 'A'] = 0;
                }
                suggestedLetter = GetMostCommmonLetter(knownLettersWithPosition, letterCounter);
            }

            return suggestedLetter;
        }

        private static char GetMostCommmonLetter(char?[] knownLettersWithPosition, int[] letterCounter)
        {
            char suggestedLetter = ' ';

            // Select the most common letter
            int count = 0;
            for (int i = 0; i < letterCounter.Length; i++)
            {
                // Don't pick letters we already know
                if (letterCounter[i] > count && knownLettersWithPosition.Contains((char)('A' + i)) == false)
                {
                    count = letterCounter[i];
                    suggestedLetter = (char)('A' + i);
                }
            }

            return suggestedLetter;
        }

        private static int[] CountLettersInWords(List<string> words)
        {
            // Count the occurances of letters in the entire word list
            int[] letterCounter = new int['Z' - 'A' + 1];
            foreach (string word in words)
            {
                string uppercaseWord = word.ToUpper();
                foreach (char letter in uppercaseWord)
                {
                    letterCounter[letter - 'A']++;
                }
            }

            return letterCounter;
        }

        private static bool matchesPattern(string word, char?[] pattern)
        {
            word = word.ToUpper();
            bool matchesPatternSoFar = true;

            var knownLetters = pattern.Distinct().ToList();

            for (int position = 0; position < word.Length && position < pattern.Length; position++)
            {
                char? letterFromPattern = pattern[position];

                if (letterFromPattern == null || letterFromPattern < 'A' || letterFromPattern > 'Z')
                {
                    // Pattern shows empty, but the word has a known letter in it. E.g. ****es* (dankest) sadness
                    if (knownLetters.Contains(word[position]))
                    {
                        matchesPatternSoFar = false;
                        break;
                    }
                    continue; // Ignore non-letters
                }

                // Letter in the word does not match letter in the pattern
                if (word[position] != letterFromPattern)
                {
                    matchesPatternSoFar = false;
                    break;
                }
            }
            return matchesPatternSoFar;
        }

        private static void GetAllWordsAndExcludeWordsBasedOnLetters(WordSize wordSize, char[] excludedLetters, List<string> possibleWords)
        {
            switch (wordSize)
            {
                case WordSize.FIVE:
                    {
                        foreach (string word in wordsSize5)
                        {
                            if (wordContainsLetter(excludedLetters, word))
                            {
                                continue;
                            }
                            else
                            {
                                possibleWords.Add(word);
                            }
                        }
                        break;
                    }
                case WordSize.SEVEN:
                    foreach (string word in wordsSize7)
                    {
                        if (wordContainsLetter(excludedLetters, word))
                        {
                            continue;
                        }
                        else
                        {
                            possibleWords.Add(word);
                        }
                    }
                    break;
                case WordSize.NINE:
                    foreach (string word in wordsSize9)
                    {
                        if (wordContainsLetter(excludedLetters, word))
                        {
                            continue;
                        }
                        else
                        {
                            possibleWords.Add(word);
                        }
                    }
                    break;
                default:
                    throw new Exception("Invalid wordSize");
            }
        }

        private static bool wordContainsLetter(char[] excludedLetters, string word)
        {
            bool result = false;
            foreach (char letter in excludedLetters)
            {
                if (word.ToUpper().Contains(letter))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
