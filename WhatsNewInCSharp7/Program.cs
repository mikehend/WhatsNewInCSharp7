﻿using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WhatsNewInCSharp7
{
	partial class Program
	{
		const int FirstValue = 00_11_00_11;
		const int SecondValue = 0b00_11_00_11;
		const int ThirdValue = 0x3_3;

#pragma warning disable IDE0022 // Use expression body for methods
		static void Main(string[] args)
		{
			//Program.ShowBinaryDigitsAndDigitSeparators();
			//Program.ShowLocalFunctions();
			//Program.ShowOutVar();
			//Program.ShowRefReturnsAndLocals();
			//Program.ShowPatternMatching();
			//Program.ShowTuples();
			//Program.ShowTuplesWithGenerics();
			Program.ShowValueTask();
			//Program.ShowExpressionBodiedMembersAndThrowExpressions();
		}
#pragma warning restore IDE0022

		private static void ShowBinaryDigitsAndDigitSeparators()
		{
			Console.Out.WriteLine(Program.FirstValue);
			Console.Out.WriteLine(Program.SecondValue);
			Console.Out.WriteLine(Program.ThirdValue);
		}

		private static void ShowLocalFunctions()
		{
			uint Collatz(uint value) =>
				value % 2 == 1 ? (3 * value + 1) / 2 : value / 2;

			uint[] CollatzSequence(uint start)
			{
				var sequence = new List<uint> { start };

				var next = start;

				while (next > 1)
				{
					next = Collatz(next);
					sequence.Add(next);
				}

				return sequence.ToArray();
			}

			Console.Out.WriteLine(string.Join(", ", CollatzSequence(5)));
			Console.Out.WriteLine(string.Join(", ", CollatzSequence(11)));
		}

		private static void ShowOutVar()
		{
			Program.MakingParsingEasy("fjdklaf");
			Program.MakingParsingEasy("3");
		}

		private static void MakingParsingEasy(string value)
		{
			// First way
			try
			{
				var firstWay = int.Parse(value);
				Console.Out.WriteLine($"First way: {firstWay}");
			}
			catch (ArgumentNullException) { }
			catch (OverflowException) { }
			catch (FormatException) { }

			// Second way
#pragma warning disable IDE0018 // Inline variable declaration
			var secondWay = 0;
#pragma warning restore IDE0018 // Inline variable declaration

			if (int.TryParse(value, out secondWay))
			{
				Console.Out.WriteLine($"Second way: {secondWay}");
			}

			// Thrid way
			if (int.TryParse(value, out var thirdWay))
			{
				Console.Out.WriteLine($"Third way: {thirdWay}");
			}
		}

		private static void ShowRefReturnsAndLocals()
		{
			var slowList = new SlowList<GiantStruct>(100);
			var slowStruct1 = slowList[50];
			var slowStruct2 = slowList[50];
			slowStruct1.Value1 = 22;
			Console.Out.WriteLine(
				$"{nameof(slowStruct2)}.{nameof(slowStruct2.Value1)} is {slowStruct2.Value1}");

			var fastList = new FastList<GiantStruct>(100);

			// Note, you can type this, but it won't be "ref"
			//var fastStruct1 = fastList[50];
			//var fastStruct2 = fastList[50];

			ref var fastStruct1 = ref fastList[50];
			ref var fastStruct2 = ref fastList[50];
			fastStruct1.Value1 = 22;
			Console.Out.WriteLine(
				$"{nameof(fastStruct2)}.{nameof(fastStruct2.Value1)} is {fastStruct2.Value1}");

			// For benchmarks...
			BenchmarkRunner.Run<ListIndexPerformance>();
			/*
							  Method |        Mean |      Error |     StdDev |  Scaled |  ScaledSD | Allocated |
			-------------------- | -----------:| ----------:| ----------:| -------:| ---------:| ---------:|
			 GetNameFromFastList |   0.4910 ns |  0.0549 ns |  0.0855 ns |    1.00 |      0.00 |       0 B |
			 GetNameFromSlowList |  18.8409 ns |  0.4125 ns |  0.8045 ns |   39.47 |      6.76 |       0 B |
			*/
		}

		private static void ShowPatternMatching()
		{
			void CheckThings(Thing thing)
			{
				switch (thing)
				{
					case SomeThing someThing22 when someThing22.Value == 22:
						Console.Out.WriteLine($"{nameof(SomeThing)} with {nameof(Thing.Value)} == 22");
						break;
					case SomeThing someThingLessThan10 when someThingLessThan10.Value < 10:
						Console.Out.WriteLine($"{nameof(SomeThing)} with {nameof(Thing.Value)} < 10");
						break;
					case AnotherThing anotherThing:
						Console.Out.WriteLine($"{nameof(AnotherThing)}");
						break;
					default:
						Console.Out.WriteLine("Unknown case");
						break;
					case null:
						throw new ArgumentNullException(nameof(thing));
				}
			}

			CheckThings(new SomeThing { Value = 22 });
			CheckThings(new SomeThing { Value = 2 });
			CheckThings(new AnotherThing { Value = 2 });
			CheckThings(new SneakyThing());
			CheckThings(new SomeThing { Value = 222 });
			CheckThings(null);
		}

		private static void ShowTuples()
		{
			var tupleResult = Calculator.Calculate(
				Program.FirstValue, Program.ThirdValue);
			Console.Out.WriteLine($"{nameof(Calculator.Calculate)} value: {tupleResult.Item1}");

			var namedTupleResult = Calculator.CalculateWithNamedValues(
				Program.FirstValue, Program.ThirdValue);
			Console.Out.WriteLine($"{nameof(Calculator.CalculateWithNamedValues)} value: {namedTupleResult.value}");
			(var namedValue, var _) = Calculator.CalculateWithNamedValues(
				Program.FirstValue, Program.ThirdValue);

			Console.Out.WriteLine($"{nameof(Calculator.CalculateWithNamedValues)} value from name: {namedValue}");

			var result = Calculator.CalculateWithDeconstructedType(
				Program.FirstValue, Program.ThirdValue);
			Console.Out.WriteLine($"{nameof(Calculator.CalculateWithDeconstructedType)} value: {result.Result}");
			(var x, var y) = result;
			Console.Out.WriteLine($"{nameof(Calculator.CalculateWithDeconstructedType)} value from x: {result.Result}");
		}

		private static void ShowTuplesWithGenerics()
		{
			var handler = new Handler();
			handler.Handle();
			handler.Handle(("a", 1, Guid.NewGuid()));
			handler.Handle(("a", "b"));
		}

		private static void ShowValueTask()
		{
			ValueTask<int> ReturnValueAsync() => new ValueTask<int>(22);

			Console.Out.WriteLine(ReturnValueAsync().GetAwaiter().GetResult());
		}

		private static void ShowExpressionBodiedMembersAndThrowExpressions()
		{
			void PersonPropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				Console.Out.WriteLine($"{e.PropertyName} changed.");
			}

			var person = new ExpressedPerson(22, "John");
			person.PropertyChanged += PersonPropertyChanged;
			person.Id = 44;
			person.Name = "Jeff";

			Console.Out.WriteLine($"{person.Id}, {person.Name}");
			Console.Out.WriteLine();

			try
			{
				new ExpressedPerson(22, null);
			}
			catch(ArgumentNullException e)
			{
				Console.Out.WriteLine($"{nameof(ArgumentNullException)} - {e.ParamName}");
			}
		}
	}
}
