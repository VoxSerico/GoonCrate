using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoonCrates;

namespace GoonCratesMetrics
{
    class Program
    {
        static void Main(string[] args)
        {
            GoonCrates.Program.BuildWordList();
            string allwords = GoonCrates.Properties.Resources.password_pool;
            allwords = allwords.Replace("five@=", "");
            allwords = allwords.Replace("seven@=", "");
            allwords = allwords.Replace("nine@=", "");
            allwords = allwords.Replace("\r\n", "@,");
            string[] wordsList = allwords.ToUpper().Split(new string[] { "@," }, StringSplitOptions.RemoveEmptyEntries);
            List<int> testResultsRaw = new List<int>();

            foreach(string word in wordsList)
            {
                Console.Write(word);
                WordSize wordSize;
                char?[] knownLettersWithPosition;
                List<char> excludedLetters = new List<char>();
                if (word.Length == 5)
                {
                    wordSize = WordSize.FIVE;
                    knownLettersWithPosition = new char?[5];
                }
                else if (word.Length == 7)
                {
                    wordSize = WordSize.SEVEN;
                    knownLettersWithPosition = new char?[7];
                }
                else if (word.Length == 9)
                {
                    wordSize = WordSize.NINE;
                    knownLettersWithPosition = new char?[9];
                }
                else
                {
                    throw new InvalidOperationException();
                }

                int guesses = 0;
                Console.Write(" (");
                do
                {
                    guesses++;
                    char letterToTry = GoonCrates.Program.GetSuggestedLetter(wordSize, knownLettersWithPosition, excludedLetters.ToArray());
                    Console.Write(letterToTry);
                    int index = word.IndexOf(letterToTry);
                    if(index == -1)
                    {
                        excludedLetters.Add(letterToTry);
                        continue;
                    }

                    knownLettersWithPosition[index] = letterToTry;

                    index = word.IndexOf(letterToTry, index + 1);
                    while ( index != -1)
                    {
                        //Console.Write(letterToTry);
                        knownLettersWithPosition[index] = letterToTry;
                        index = word.IndexOf(letterToTry, index + 1);
                    }

                } while (GoonCrates.Program.GetPossibleWords(wordSize, knownLettersWithPosition, excludedLetters.ToArray()).Count > 1);
                Console.Write(")");
                Console.WriteLine(" after " + guesses + " guesses.");
                testResultsRaw.Add(guesses);
            }
            Console.WriteLine("DONE!");
            Console.WriteLine();
            Dictionary<int, int> testResults = new Dictionary<int, int>();
            testResultsRaw.Sort();
            foreach (int i in testResultsRaw.Distinct())
            {
                testResults.Add(i, testResultsRaw.Count(guesscount => guesscount == i));
            }

            foreach(KeyValuePair<int,int> entry in testResults)
            {
                Console.WriteLine($"Solved in {entry.Key} guess(es): {entry.Value}");
            }

            Console.WriteLine("Average: " + (double)testResultsRaw.Sum()/testResultsRaw.Count + " guesses");
            if (testResultsRaw.Count % 2 == 0)
            {
                Console.WriteLine("Median: " + testResultsRaw[testResultsRaw.Count/2] + " guesses");
            }
            else
            {
                Console.WriteLine("Median*: " + (double)(testResultsRaw[testResultsRaw.Count / 2] + testResultsRaw[testResultsRaw.Count / 2 + 1])/2 + " guesses");
            }
            
            Console.ReadLine();
        }
        
    }
}
