using System;
using System.Linq;
using System.Text;

namespace MasterMind
{
    public class MasterMindBll
    {
        #region private fields
        private readonly int _lowRange;
        private readonly int _highRange;
        private readonly int _numberOfAllowedGuesses;
        private int _numberOfGuessesMade;   // Trying to avoid UI having logic, but it is a little awkward here.
        private readonly int _numberOfDigitsInAnswer;
        private readonly int _answer;       // In hindsight not sure if int or int[] makes more sense.
        private bool _correctAnswerGuessed; // Also awkward here, but this and guess count need to be tracked together.
        #endregion private fields

        #region properties
        public string LowRange
        {
            get { return _lowRange.ToString(); }
        }
        public string HighRange
        {
            get { return _highRange.ToString(); }
        }
        public bool AnotherGuessAllowed
        {
            get { return (_numberOfGuessesMade < _numberOfAllowedGuesses) && !_correctAnswerGuessed; }
        }
        public string NumOfDigits
        {
            get { return _numberOfDigitsInAnswer.ToString(); }
        }
        #endregion properties

        #region constructor
        public MasterMindBll(int lowRange, int highRange, int numberOfAllowedGuesses, int numberOfDigitsInAnswer)
        {
            if (lowRange >= highRange)
            {
                throw new ArgumentException("lowRange must be lower than highRange.");
            }
            if (numberOfAllowedGuesses < 1)
            {
                throw new ArgumentException("numberOfAllowedGuesses must be greater than 0.");
            }
            if (numberOfDigitsInAnswer < 1)
            {
                throw new ArgumentException("numberOfDigitsInAnswer must be greater than 0.");
            }
            _lowRange = lowRange;
            _highRange = highRange;
            _numberOfAllowedGuesses = numberOfAllowedGuesses;
            _numberOfDigitsInAnswer = numberOfDigitsInAnswer;
            _answer = GenerateNewAnswer();
        }
        #endregion constructor

        #region public methods
        public bool IsValidGuess(string guess)
        {
            return CorrectNumberOfDigits(guess) && AllDigitsAreValid(guess);
        }

        public bool IsCorrectGuess(string guess)
        {
            _correctAnswerGuessed = guess == _answer.ToString();
            return _correctAnswerGuessed;
        }

        // I should have have UI handle the +- part instead of this class, but could not decide what made most sense for a return value in that case.
        public string GuessResult(string guess)
        {   
            if (!IsValidGuess(guess))
            {
                throw new ArgumentException($"'{guess}' is not a valid guess and you must validate prior to using GuessResult!");
            }
            else if (IsCorrectGuess(guess))
            {
                throw new ArgumentException($"'{guess}' was the correct answer and you need to validate that prior to using GuessResult!");
            }
            _numberOfGuessesMade++; // awkward to have this side-effect here, but it seems to be best place.
            return GenerateGuessResult(guess);
        }
        #endregion public methods

        #region private helper methods
        private int GenerateNewAnswer()
        {
            Random rnd = new Random();
            int newAnswer = 0;

            for (int i = 0; i < _numberOfDigitsInAnswer; i++)
            {
                newAnswer = newAnswer * 10 + rnd.Next(_lowRange, _highRange + 1);   // +1 since maxValue is excluded from Next
            }
            return newAnswer;
        }

        private bool CorrectNumberOfDigits(string guess)
        {
            return guess.Length == _numberOfDigitsInAnswer;
        }

        private bool AllDigitsAreValid(string guess)
        {
            for (int i = 0; i < guess.Length; i++)
            {
                if (!IsDigitValid(guess[i].ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsDigitValid(string guess)
        {
            bool isValid;

            if (int.TryParse(guess, out int digit))
            {
                isValid = (digit >= _lowRange) && (digit <= _highRange);
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

        private string GenerateGuessResult(string guess)
        {
            int exactMatches = NumberOfExactMatches(guess);
            int correctDigit = NumberOfCorrectDigits(guess);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < exactMatches; i++)
            {
                builder.Append("+");
            }
            for (int i = 0; i < correctDigit; i++)
            {
                builder.Append("-");
            }
            return builder.ToString();
        }

        private int NumberOfCorrectDigits(string guess)
        {
            int correctDigit = 0;
            string tempAnswer = _answer.ToString();
            for (int i = tempAnswer.Length - 1; i >= 0; i--)
            {
                if (guess.Contains(tempAnswer[i]) && tempAnswer[i] != guess[i])
                {
                    tempAnswer = tempAnswer.Remove(i, 1);
                    correctDigit++;
                }
            }

            return correctDigit;
        }

        private int NumberOfExactMatches(string guess)
        {
            int exactMatches = 0;
            for (int i = 0; i < _numberOfDigitsInAnswer; i++)
            {
                if (guess[i] == _answer.ToString()[i])
                {
                    exactMatches++;
                }
            }

            return exactMatches;
        }
        #endregion private helper methods
    }
}
