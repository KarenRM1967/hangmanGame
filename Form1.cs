// Karen Rees-Milton. COMP60 
//6 April 2018. Assignment2
//A program that plays the game of Hangman

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReesMilton
{
    public partial class Hangman : Form
    {
        //declare data fields to be accessed by class functions
        string[] wordArray;
        char[] lettersGuessed;
        string word;
        string dashedWord;
        int determineImage = 1;
        int totalGuessCounter;
        int losses = 0, wins = 0;
        string[] usedWordArray;
        int noGames = 0;
        const int SIZE = 10;

        public Hangman()
        {
            InitializeComponent();
            //populate labels with text
            targetLabel.Text = "Target";
            guessLabel.Text = "Guesses";
            currentGuessLabel.Text = "Enter a letter";
            buttonNewGame.Text = "New Game";
            wonLabel.Text = "Won";
            lostLabel.Text = "Lost";

            //populate text and image for opening interface
            wonTextBox.Text = "0";
            lostTextBox.Text = "0";
            pictureBoxHangman.Image = ReesMilton.Properties.Resources.hangman1;

            //instantiate arrays
            wordArray = new string[SIZE] {"windsurfing", "cycling", "skiing", "snowboarding",
                                       "skating", "rollerblading", "soccer", "golf",
                                        "horseriding", "yachting"};

            usedWordArray = new string[SIZE];
            lettersGuessed = new char[26];
        }

        //function to generate new random word
        private void GenerateRandomWord()
        {
            Random random = new Random();
            int randomNum = random.Next(0, 10);
            word = wordArray[randomNum];
        }
        
        //function to ask user if they want to play again
        private void AskIfWantToPlayAgain()
        {
            if (noGames == 10)
            {
                MessageBox.Show("Thanks for playing. You lost " + losses +
                                " games and won " + wins + " games",
                                "Game over");
            }
            else
            {
                DialogResult dialogResult;
                dialogResult = MessageBox.Show("Do you want to play again?", "Play Again",
                                                MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("You lost " + losses + " games and won " + wins + " games",
                                    "Thanks for playing");
                }
                else if (dialogResult == DialogResult.Yes)
                {
                    MessageBox.Show("Click OK and then New Game button", "Play again!");
                }
            }
        }

        //event function handler for user typing a letter in currentGuessTextBox        
        private void currentGuessTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            //add guess to guesses
            string currentGuess = currentGuessTextBox.Text.ToLower();
            lettersGuessed[totalGuessCounter] = Convert.ToChar(currentGuess);
            string guesses = new string(lettersGuessed);
            guessTextBox.Text = guesses;

            //reset currentGuessTextBox
            currentGuessTextBox.Text = "";

            //search word for guess letter
            char[] dashedWordLetters = dashedWord.ToCharArray();
            char[] wordLetters = word.ToCharArray();
            int ctr;
            for (ctr = 0; ctr < word.Length; ctr++)
            {
                if (wordLetters[ctr] == lettersGuessed[totalGuessCounter])
                    dashedWordLetters[ctr] = lettersGuessed[totalGuessCounter];
            }

            //reset dashedWord (unguessed letters (-) and guessed letters (actual letter))
            string newDashedWord;
            newDashedWord = new string(dashedWordLetters);
            totalGuessCounter++;

            //data fields and user interface updates if letter is not in word
            if (newDashedWord == dashedWord)
            {
                determineImage++;
                if (determineImage == 8)
                {
                    losses++;
                    lostTextBox.Text = losses + "";
                    MessageBox.Show("You Lost!");
                    AskIfWantToPlayAgain();
                }
                else
                {
                    switch (determineImage)
                    {
                        case 2:
                            pictureBoxHangman.Image = ReesMilton.Properties.Resources.hangman2;
                            break;
                        case 3:
                            pictureBoxHangman.Image = ReesMilton.Properties.Resources.hangman3;
                            break;
                        case 4:
                            pictureBoxHangman.Image = ReesMilton.Properties.Resources.hangman4;
                            break;
                        case 5:
                            pictureBoxHangman.Image = ReesMilton.Properties.Resources.hangman5;
                            break;
                        case 6:
                            pictureBoxHangman.Image = ReesMilton.Properties.Resources.hangman6;
                            break;
                        default:
                            pictureBoxHangman.Image = ReesMilton.Properties.Resources.hangman7;
                            break;
                    }
                }
            }
            else
            {
                //data fields and user interface updates if letter is in word
                targetTextBox.Text = newDashedWord;
                dashedWord = newDashedWord;
                int gameOver = dashedWord.CompareTo(word);
                if (gameOver == 0)
                {
                    wins++;
                    wonTextBox.Text = wins + "";
                    MessageBox.Show("You Won!");
                    AskIfWantToPlayAgain();
                }
            }
        }

        //event handler function for user pressing new game button
        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            //reset data fields for new game
            determineImage = 1;
            totalGuessCounter = 0;
            GenerateRandomWord();
                     
            //check if random word used before and if yes generate another 
            for (int ctr = 0; ctr < SIZE - 1; ctr++)
            {
                if (usedWordArray[ctr] == word)
                {
                    GenerateRandomWord();
                    ctr = -1;
                }
            }
            //add word to usedWordArray
            usedWordArray[noGames] = word;
            noGames++;

            //reset guessed letters array to empty
            for (int ctr = 0; ctr < 26; ctr++)
                lettersGuessed[ctr] = ' ';
            
            //determine number of letters in word and populate target with equal number of dashes 
            char[] wordLetters = word.ToCharArray();
            for (int ctr = 0; ctr < word.Length; ctr++)
            {
                wordLetters[ctr] = '-';
            }
            dashedWord = new string(wordLetters);

            //reset user interface for new game
            guessTextBox.Text = " ";
            targetTextBox.Text = dashedWord;
            pictureBoxHangman.Image = ReesMilton.Properties.Resources.hangman1;         
        }
    }
}
