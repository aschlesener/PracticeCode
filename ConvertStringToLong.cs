/*
	This is an implementation of a conversion from a string to a long number. 
	Conversion is done by examining the string backwards, converting characters into digits, and multiplying each number by a factor of 10 to reach the final result. Tests are included for positive integers, negative integers, invalid formatting, and overflow.
	
	Amy Schlesener
	amy@amyschlesener.com
*/

using System;

public class ConvertStringToLong 
{
	public static void Main() 
	{
		// Run tests
		TestNormal();
		TestNegative();
		TestInvalidFormat();
		TestOverflow();
	}	

	// Converts given string to long number without using built-in conversion method.
	public static long StringToLong(string str) 
	{
		bool isNegative = false;
		long longNumber = 0;
		long factor = 1;
		
		if (str == null) 
			throw new FormatException("Format invalid");
		
		// Check for negative number
		if (str[0] == '-' && str.Length > 1) 
		{
			isNegative = true;
			str = str.Substring(1);
		}
		
		// Loop through string backwards
		for (int i = str.Length - 1; i >= 0; i--) 
		{
			//Check for valid digit
			long digit = str[i] - '0';
			if (digit > 9 || digit < 0) 
			{
				throw new FormatException("Format invalid");
			}
			
			// Check for overflow
			if (factor < 1000000000000000000) 
			{
				longNumber += digit * factor;
			}
			else 
			{	
				// Factor is high enough to warrant enabling overflow checking
				checked 
				{
					try 
					{
						longNumber += (digit * factor);
					}
					catch (OverflowException) 
					{
						throw new OverflowException("This is thrown for the overflow test.");
					}
				}
			}
			factor *= 10;
		}
		return isNegative ? (-1 * longNumber) : longNumber;
	}
	
	public static void TestNormal() 
	{
		long i = StringToLong("123");
		if (i == 123)
			Console.WriteLine("Passed TestNormal");
		else
			Console.WriteLine("Failed TestNormal");
		}
	
	public static void TestNegative() 
	{
		long i = StringToLong("-123");
		if (i == -123)
			Console.WriteLine("Passed TestNegative");
		else
			Console.WriteLine("Failed TestNegative");
	}
	
	public static void TestInvalidFormat() 
	{
		try 
		{
			long i = StringToLong("-ab!~");
			Console.WriteLine("Failed TestInvalidFormat");
		}
		catch (FormatException) 
		{
			Console.WriteLine("Passed TestInvalidFormat");
		}
	}
	
	public static void TestOverflow() 
	{
		try 
		{
			long i = StringToLong("9999999999999999999");
			Console.WriteLine("Failed TestOverflow");
		}
		catch (OverflowException) 
		{
			Console.WriteLine("Passed TestOverflow");
		}
	}	
}