using System;

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
}