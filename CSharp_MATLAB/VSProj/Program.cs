using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

// MathWorks assemblies that ship with Builder for .NET
// and should be registered in Global Assembly Cache
using MathWorks.MATLAB.NET.Utility;
using MathWorks.MATLAB.NET.Arrays;

// Assembly built by Builder for .NET containing 
// math_on_numbers.m
using dotnet;

// To include the C Shared Library created with
// MATLAB Compiler we need to include InteropServices
// to integrate the unmanaged code
using System.Runtime.InteropServices;


// Copyright 2006, 2007, 2010 The MathWorks, Inc.

namespace example_ML_integration {
	class Program {
		static void Main(string[] args) {
			////////////////////
			// Input Parameters
			////////////////////

			// Using feature array as input for simplicity....
			// Will Later send over the 30x(t) matrix for processing
			System.Array ar = new double[] { 68.8, 120, 110, 118, 150, 179, 180, 150, 178, 152, 163, 107, 149, 97.8, 90.1, 94.3, 90.9, 89.9, 53.4 };


			// create an array ai for the imaginary part of "a"
			System.Array ai = new double[] { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 };


			/////////////////////
			// Output Parameters
			/////////////////////

			// initialize variables for return value from ML
			System.Array cr = new double[3];
			System.Array ci = new double[3];


			////////////////////////
			// Call MATLAB function
			////////////////////////
			// call appropriate function/method based on Mode
			UseEngine(ar, ai, ref cr, ref ci);


			/////////////////////
			// Output to console
			/////////////////////
			DisplayArgs(true, ar); // true = input
			DisplayArgs(false, cr); // false = not-input or output
			DisplayEnd();


		}
		static private void UseEngine(Array ar, Array ai, ref Array cr, ref Array ci) {
			/*
			 * This function calls the math_by_numbers routine inside
			 * MATLAB using the MATLAB Engine's com interface
			 */

			// Instantiate MATLAB Engine Interface through com
			MLApp.MLAppClass matlab = new MLApp.MLAppClass();

			// Using Engine Interface, put the matrix "a" into 
			// the base workspace.
			// "a" is a complex variable with a real part of ar,
			// and an imaginary part of ai
			matlab.PutFullMatrix("a", "base", ar, ai);

			// Using Engine Interface, execute the ML command
			// contained in quotes.
			String MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			String ProjectLocation = "Visual Studio 2010\\Projects\\Kinect-Tracking-Project\\MatlabPrototypes\\FeatureDetection";
			String matFileCD_command = String.Format("cd '{0}';", System.IO.Path.Combine(MyDocs, ProjectLocation));

			Console.WriteLine(matFileCD_command);

			matlab.Execute(matFileCD_command);
			// matlab.Execute("open testBayes.m");
			// matlab.Execute("dbstop in math_on_numbers.m");
			matlab.Execute("c = transpose(testBayes(a));");
			
			//matlab.Execute("com.mathworks.mlservices.MLEditorServices.closeAll");
			//matlab.Execute("dbquit all");

			// Using Engine Interface, get the matrix "c" from
			// the base workspace.
			// "c" is a complex variable with a real part of cr,
			// and an imaginary part of ci
			Console.WriteLine("Staring to do shit");
			try {
				matlab.GetFullMatrix("c", "base", ref cr, ref ci);
			} catch (Exception) {
				Console.WriteLine("someErr");
			}
		}


		static private void DisplayArgs(Boolean In, [In]Array OneReal) {
			// --cheics: I swear there's a better way to do this. So sorry...
			String outVals = "";
			for (int i = 0; i < OneReal.Length; i++) {
				outVals += OneReal.GetValue(i).ToString();
				if (i != OneReal.Length - 1) {
					outVals += ",";
				}

			}

			if (In) {
				// this is the first output, so
				// prepare the console
				Console.Clear();
				Console.WriteLine("Input was: [{0}]\r\n", outVals);
			} else {
				Console.WriteLine("Output class using squatz classifier (should be [1,2,0]): [{0}]\r\n", outVals);
			}
		}

		static private void DisplayEnd() {
			ConsoleKeyInfo keypressed;

			Console.WriteLine();
			Console.WriteLine("Press any key to exit.");

			keypressed = Console.ReadKey(true);

		} // end function DisplayEnd

	} // end class Program

} // end namespace example_ML_integration