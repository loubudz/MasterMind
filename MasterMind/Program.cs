using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMind
{
    class Program
    {
        static void Main(string[] args)
        {        
            int lowRange = 1;
            int highRange = 6;
            int numberOfAllowedGuesses = 10;
            int numberOfDigitsInAnswer = 4;

            MasterMindBll masterMindBll = new MasterMindBll(lowRange, highRange, numberOfAllowedGuesses, numberOfDigitsInAnswer);
            IntroductionToGame(masterMindBll);
            while (masterMindBll.AnotherGuessAllowed)
            {
                PlayAnotherRound(masterMindBll);
            }
            Console.WriteLine("Thank you for playing. Program will close when you hit 'Enter'");
            Console.ReadLine();
        }

        private static void PlayAnotherRound(MasterMindBll masterMindBll)
        {
            Console.WriteLine();
            Console.WriteLine("Please enter your guess below.");
            string guess = Console.ReadLine();

            while (!masterMindBll.IsValidGuess(guess))
            {
                guess = PromptForNewGuess(masterMindBll, guess);
            }

            DisplayGuessResult(masterMindBll, guess);

            if (!masterMindBll.AnotherGuessAllowed && !masterMindBll.IsCorrectGuess(guess))
            {
                Console.WriteLine($"Sorry, but that was your final guess and it was incorrect.");
            }
        }

        private static void DisplayGuessResult(MasterMindBll masterMindBll, string guess)
        {
            if (masterMindBll.IsCorrectGuess(guess))
            {
                Console.WriteLine($"Correct! {guess} was the answer!");
            }
            else
            {
                Console.WriteLine($"{masterMindBll.GuessResult(guess)}");
            }
        }

        private static string PromptForNewGuess(MasterMindBll masterMindBll, string guess)
        {
            Console.WriteLine($"'{guess}' is an invalid guess.");
            Console.WriteLine($"Please enter a guess with {masterMindBll.NumOfDigits} digits in length.");
            Console.WriteLine($"Also each digit within range of {masterMindBll.LowRange} through {masterMindBll.HighRange}.");
            guess = Console.ReadLine();
            return guess;
        }

        private static void IntroductionToGame(MasterMindBll masterMindBll)
        {
            Console.WriteLine("Welcome to a new game of Master Mind!");
            Console.WriteLine($"A new answer has been generated which is {masterMindBll.NumOfDigits} digits in length");
            Console.WriteLine($"The digits will be within range of {masterMindBll.LowRange} through {masterMindBll.HighRange}");
        }
    }
}
