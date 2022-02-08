using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVGB07_viktlund104_Laboration2_Lotto
{
	public partial class MainForm : Form
	{
		// Settings
		private int minBound = 1; // low bound, inclusive
		private int maxBound = 35; // high bound, inclusive
		private int minSimulations = 1; //inclusive
		private int maxSimulations = 999999; //inclusive
		private int n = 0; // how many times to simulate
		private int lottoNumbers = 7; // how many lotto numbers to pull
		private List<int> choices;
		private Random rand = new Random();
		private int twoCorrect;
		private int threeCorrect;
		private int fourCorrect;
		private int fiveCorrect;
		private int sixCorrect;
		private int sevenCorrect;

		// Initialize Form
		public MainForm()
		{
			InitializeComponent();
		}

		// When we click the button, start the lotto
		private void startLottoButton_Click(object sender, EventArgs e)
		{
			// Initialize counts
			fiveCorrect = 0;
			sixCorrect = 0;
			sevenCorrect = 0;

			// Check that the user has given us good inputs
			if (!allInBounds())
			{
				// out of bounds, abort
				return;
			}

			if (noDuplicates(choices))
			{
				// duplicates found, abort
				return;
			}

			if (!numberOfTriesInBounds())
			{
				// number of tries out of bounds, abort
				return;
			}

			// We got OK inputs. Run the test
			generateLotto(n);

			// Display results
			twoCorrectTextBox.Text = twoCorrect.ToString();
			threeCorrectTextBox.Text = threeCorrect.ToString();
			fourCorrectTextBox.Text = fourCorrect.ToString();
			fiveCorrectTextBox.Text = fiveCorrect.ToString();
			sixCorrectTextBox.Text = sixCorrect.ToString();
			sevenCorrectTextBox.Text = sevenCorrect.ToString();
		}

		// Creates the list of choices and tries to check the user input.
		private bool allInBounds()
		{
			choices = new List<int>(7); //initialize list here so we start with a fresh list every click

			if (!inBounds(firstNumberTextBox.Text))
			{
				// This means we didn't pass the first test, abort
				return false;
			}

			if (!inBounds(secondNumberTextBox.Text))
			{
				return false;
			}

			if (!inBounds(thirdNumberTextBox.Text))
			{
				return false;
			}

			if (!inBounds(fourthNumberTextBox.Text))
			{
				return false;
			}

			if (!inBounds(fifthNumberTextBox.Text))
			{
				return false;
			}

			if (!inBounds(sixthNumberTextBox.Text))
			{
				return false;
			}

			if (!inBounds(seventhNumberTextBox.Text))
			{
				return false;
			}

			// We passed the test, return true
			return true;
		}

		// Parses the user inputs to int numbers. Returns true if successful
		private bool inBounds(string text)
		{
			int number = 0;

			try
			{
				number = int.Parse(text);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if (number < minBound)
			{
				MessageBox.Show($"Please enter number only larger than {minBound - 1}", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return false;
			}

			if (number > maxBound)
			{
				MessageBox.Show($"Please enter number only smaller than {maxBound + 1}", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return false;
			}

			choices.Add(number); // add number to the list

			return true;
		}

		// Checks for duplicates in the list, true if no duplicates. false if duplicates found
		private bool noDuplicates(List<int> list)
		{
			// Check for duplicates. Distinct will find number of unique values, needs to NOT be the same as total list
			if (list.Count != list.Distinct().Count())
			{
				MessageBox.Show("Please choose only unique values, no duplicates", "Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return true;
			}

			return false;
		}


		// Parses number of tries to user input. True if success, false if user provided us with some wrong inputs
		private bool numberOfTriesInBounds()
		{
			try
			{
				n = int.Parse(numberLottosTextBox.Text);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if (n < minSimulations)
			{
				MessageBox.Show($"Please enter number of tries larger than {minSimulations - 1}", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if (n > maxSimulations)
			{
				MessageBox.Show($"Please enter number of tries smaller than {maxSimulations + 1}", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			return true;
		}


		// Generates a new lotto draw. It then checks how many the user had correct, before it calls itself again
		private int generateLotto(int n)
		{
			if (n == 0)
			{
				return 0;
			}

			List<int> possibleNumbers = new List<int>(maxBound - minBound);
			List<int> winningNumbers = new List<int>(lottoNumbers);

			// fill list with possible drawings
			for (int i = minBound; i <= maxBound; i++)
			{
				possibleNumbers.Add(i);
			}

			// pull out a number using the indexes of the list
			for (int i = 0; i < lottoNumbers; i++)
			{
				int draw = rand.Next(possibleNumbers.Count); // get index of number to pull out

				winningNumbers.Add(possibleNumbers[draw]); // add winning number to winning list
				possibleNumbers.RemoveAt(draw); // remove already drawn number from being drawn again
			}
			
			// check user score
			int score = checkUserScore(winningNumbers);
			// int score = checkUserScoreOptimized(winningNumbers);
			
			// Assign score to our counter class variables
			if (score == 2)
			{
				twoCorrect++;
			}
			
			if (score == 3)
			{
				threeCorrect++;
			}

			if (score == 4)
			{
				fourCorrect++;
			}

			if (score == 5)
			{
				fiveCorrect++;
			}

			if (score == 6)
			{
				sixCorrect++;
			}

			if (score == 7)
			{
				sevenCorrect++;
			}

			return generateLotto(n - 1);
		}


		// Checks how many correct numbers the user had in his choices, returns the amount of correct numbers.
		private int checkUserScore(List<int> winningNumbers_)
		{
			int score = 0;

			foreach (var e in winningNumbers_)
			{
				if (choices.Contains(e))
				{
					score++;
				}
			}

			return score;
		}
		
	}
}