using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MasterMind;

namespace MasterMindTests
{
    [TestClass]
    public class UnitTest1
    {
        private int lowRange = 1;
        private int highRange = 2;
        private int singleDigitNumberOfAllowedGuesses = 4;
        private int singleDigitNumberOfDigitsInAnswer = 1;

        private MasterMindBll CreateSingleDigitNewMasterMindBll()
        {
            return new MasterMindBll(lowRange, highRange, singleDigitNumberOfAllowedGuesses, singleDigitNumberOfDigitsInAnswer);
        }

        [TestMethod]
        public void AnotherGuessAllowed_FalseAfterCorrectGuess()
        {
            // arrange
            MasterMindBll masterMindBll  = CreateSingleDigitNewMasterMindBll();

            // act
            masterMindBll.IsCorrectGuess(lowRange.ToString());  // 50% being right
            if (masterMindBll.AnotherGuessAllowed)
            {
                masterMindBll.IsCorrectGuess(highRange.ToString()); // Since must be either "1" or "2"...
            }

            // assert
            Assert.IsFalse(masterMindBll.AnotherGuessAllowed);
        }

        [TestMethod]
        public void AnotherGuessAllowed_FalseAfterAllGuessesUsed()
        {
            // arrange
            MasterMindBll masterMindBll = CreateSingleDigitNewMasterMindBll();

            string wrongAnswer = lowRange.ToString();
            while (masterMindBll.IsCorrectGuess(wrongAnswer))   // 50% of being "1", so make new until is "2"
            {
                masterMindBll = new MasterMindBll(lowRange, highRange, singleDigitNumberOfAllowedGuesses, singleDigitNumberOfDigitsInAnswer);
            }

            // act
            for (int i = 0; i < singleDigitNumberOfAllowedGuesses; i++)
            {
                masterMindBll.GuessResult(wrongAnswer);
            }

            // assert
            Assert.IsFalse(masterMindBll.AnotherGuessAllowed);
        }

        [TestMethod]
        public void AnotherGuessAllowed_TrueBeforeAllGuessesUsed()
        {
            // arrange
            MasterMindBll masterMindBll = CreateSingleDigitNewMasterMindBll();

            string wrongAnswer = lowRange.ToString();
            while (masterMindBll.IsCorrectGuess(wrongAnswer))   // 50% of being "1" and 50% of being "2", so make new until is "2"
            {
                masterMindBll = new MasterMindBll(lowRange, highRange, singleDigitNumberOfAllowedGuesses, singleDigitNumberOfDigitsInAnswer);
            }

            // act
            masterMindBll.GuessResult(wrongAnswer);

            // assert
            Assert.IsTrue(masterMindBll.AnotherGuessAllowed);
        }

        [TestMethod]
        public void IsValidGuess_LettersInvalid()
        {
            // arrange
            MasterMindBll masterMindBll = CreateSingleDigitNewMasterMindBll();

            // act
            bool isValidGuess = masterMindBll.IsValidGuess("a");

            // assert
            Assert.IsFalse(isValidGuess);
        }

        [TestMethod]
        public void IsValidGuess_WrongNumberOfDigitsInvalid()
        {
            // arrange
            MasterMindBll masterMindBll = CreateSingleDigitNewMasterMindBll();

            // act
            bool isValidGuess = masterMindBll.IsValidGuess("123456");

            // assert
            Assert.IsFalse(isValidGuess);
        }

        // Ideally I would also be testing GuessResult, but WAY too much time spent on this already...
    }
}
