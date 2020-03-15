using System;
using System.Collections.Generic;
using System.Linq;

namespace OthelloView
{
	class View
	{
		public void writeBock(String toWrite)
		{
			Console.Write(toWrite);
		}

		public NumPair getPair(String prompt)
		{
			bool good = true;
			String errorMsg = null;

			String rawInput = null;
			String[] numStr = null;

			int[] num = new int[2];


			do
			{
				good = true;

				Console.WriteLine(prompt);

				rawInput = Console.ReadLine();

				if (rawInput.Contains(","))
				{
					numStr = rawInput.Split(",");
				}
				else if (rawInput.Contains(" "))
				{
					numStr = rawInput.Split(" ");
				}
				else
				{
					//string cannot be split
					good = false;
					errorMsg = "Please separate your coordinates by a comma (,) and/or space ( ).";
				}

				if (!good)
				{
					Console.WriteLine(errorMsg);
					continue;
				}

				if (numStr.Length != 2)
				{
					good = false;
					errorMsg = "Could not determine how many digits you entered. "
						+ "Please separate your coordinates by a comma (,) and/or space ( )."
					;
				}

				if (!good)
				{
					Console.WriteLine(errorMsg);
					continue;
				}

				try
				{
					num[0] = Convert.ToInt32(numStr[0]);
				}
				catch (System.OverflowException)
				{
					good = false;
					errorMsg = "Somehow, you have managed to enter in a number so large that "
						+ "the computer cannot process it. Please enter a smaller value for "
						+ "the first number.";
				}
				catch (System.FormatException)
				{
					good = false;
					errorMsg = "You have not enterd the first number in an understandable format. "
						+ "Please enter a number.";
				}

				if (!good)
				{
					Console.WriteLine(errorMsg);
					continue;
				}

				try
				{
					num[1] = Convert.ToInt32(numStr[1]);
				}
				catch (System.OverflowException)
				{
					good = false;
					errorMsg = "Somehow, you have managed to enter in a number so large that "
						+ "the computer cannot process it. Please enter a smaller value for "
						+ "the second number.";
				}
				catch (System.FormatException)
				{
					good = false;
					errorMsg = "You have not enterd the second number in an understandable format. "
						+ "Please enter a number.";
				}

				if (!good)
				{
					Console.WriteLine(errorMsg);
					continue;
				}

			} while (!good);

			return new NumPair(num[0], num[1]);
		}

		public int getInt(String prompt)
		{
			Console.WriteLine(prompt);

			String rawInput = null;

			int result = 0;

			bool good;
			String errMsg = null;
			do
			{
				good = true;

				rawInput = Console.ReadLine();

				try
				{
					result = Convert.ToInt32(rawInput);
				}
				catch (OverflowException)
				{
					good = false;
					errMsg = "Somehow, you have managed to enter in a number so large that "
						+ "the computer cannot process it.";
				}
				catch (FormatException)
				{
					good = false;
					errMsg = "You have not entered a valid number.";
				}

				if (!good)
				{
					Console.WriteLine(errMsg);
					continue;
				}
			} while (!good);

			return result;
		}

		public void multipleChoice(MultipleChoice options, String prompt)
		{
			Console.Write(prompt + ": ");

			String[] opts = options.getOptions();
			int i = 0;
			foreach (String option in opts)
			{
				Console.Write("({0}) {1} ", i++, option);
			}
			Console.Write("\n");

			String rawResult = null;
			int resultIndex = -1;

			bool good;
			String errorMsg = null;
			do
			{
				good = true;

				rawResult = Console.ReadLine();

				try
				{
					resultIndex = Convert.ToInt32(rawResult);
				}
				catch (System.OverflowException)
				{
					good = false;
					errorMsg = "I guarantee you that there are not that many options. Please try again.";
				}
				catch (System.FormatException)
				{
					good = false;
					errorMsg = "You have not entered a number. Please try again.";
				}

				if (!good)
				{
					Console.WriteLine(errorMsg);
					continue;
				}

				try
				{
					options.select(opts[resultIndex]);
				}
				catch (IndexOutOfRangeException)
				{
					good = false;
					errorMsg = "That is not an option. Please try again.";
				}

				if (!good)
				{
					Console.WriteLine(errorMsg);
					continue;
				}

				if (!options.selected())
				{
					good = false;
					errorMsg = "Selection failed. Please try again.";
				}

				if (!good)
				{
					Console.WriteLine(errorMsg);
					continue;
				}
			} while (!good);
		}

		public void sayHello()
		{
			Console.WriteLine("Hello, World!");
		}
	}

	class NumPair
	{
		int[] pair;

		public NumPair(int num1, int num2)
		{
			pair = new int[2];
			pair[0] = num1;
			pair[1] = num2;
		}

		public int this[int index]
		{
			get { return pair[index]; }
		}
	}

	class MultipleChoice
	{
		private Dictionary<String, int> options;

		private bool selectionMade = false;
		private int selection;

		public MultipleChoice()
		{
			options = new Dictionary<string, int>();
		}

		public void addOption(String option, int index)
		{
			options.Add(option, index);
		}

		public String[] getOptions()
		{
			//this is cool syntax
			return options.Select(key => key.Key).ToArray();
		}

		public bool select(String selection)
		{
			bool result;
			try
			{
				this.selection = options[selection];
				result = true;
				selectionMade = true;
			}
			catch (KeyNotFoundException)
			{
				result = false;
			}
			return result;
		}

		public bool selected()
		{
			return selectionMade;
		}

		public int getSelection()
		{
			return selection;
		}
	}
}