using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoonCrates
{
    enum WordSize
    {
        FIVE, SEVEN, NINE
    }
    static class Program
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

        private static void BuildWordList()
        {
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

        internal static List<string> GetPossibleWords(WordSize wordSize, char?[] knownLettersWithPosition, char[] excludedLetters)
        {
            if (wordSize == WordSize.FIVE && knownLettersWithPosition.Length != 5)
                throw new ArgumentException("wordSize and knownLettersWithPosition mismatch");
            if (wordSize == WordSize.SEVEN && knownLettersWithPosition.Length != 7)
                throw new ArgumentException("wordSize and knownLettersWithPosition mismatch");
            if (wordSize == WordSize.NINE && knownLettersWithPosition.Length != 9)
                throw new ArgumentException("wordSize and knownLettersWithPosition mismatch");

            List<string> possibleWords = new List<string>();
            List<string> possibleWordsRefined = new List<string>();
            excludeWordsBasedOnLetters(wordSize, excludedLetters, possibleWords);
            foreach(string word in possibleWords)
            {
                if (matchesPattern(word, knownLettersWithPosition))
                {
                    possibleWordsRefined.Add(word);
                }
            }
            return possibleWordsRefined;
        }

        internal static char GetSuggestedLetter(WordSize wordSize, char?[] knownLettersWithPosition, char[] excludedLetters)
        {
            List<string> words = GetPossibleWords(wordSize, knownLettersWithPosition, excludedLetters);
            int[] letterCounter = new int[91];
            foreach(string word in words)
            {
                foreach(char letter in word)
                {
                    letterCounter[new string(new char[] { letter }).ToUpper()[0]]++;
                }
            }
            char suggestedLetter = ' ';
            int count = 0;
            for(int i = 0; i < letterCounter.Length; i++)
            {
                if(letterCounter[i] > count && knownLettersWithPosition.Contains((char)i) == false)
                {
                    count = letterCounter[i];
                    suggestedLetter = (char)i;
                }
            }
            return suggestedLetter;
        }

        private static bool matchesPattern(string word, char?[] pattern)
        {
            bool result = true;
            for (int position = 0; position < word.Length && position < pattern.Length; position++)
            {
                char? letter = pattern[position];
                if (letter == null || letter < 'A' || letter > 'Z')
                {
                    continue;
                }
                if(word.ToUpper()[position] != letter)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private static void excludeWordsBasedOnLetters(WordSize wordSize, char[] excludedLetters, List<string> possibleWords)
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
