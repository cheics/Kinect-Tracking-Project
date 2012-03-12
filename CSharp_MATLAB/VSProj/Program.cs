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
			// Using feature array as input for simplicity....
			// Will Later send over the 30x(t) matrix for processing
			Boolean writeStuff=true;
			double[] ar = new double[] { 68.8, 120, 110, 118, 150, 179, 180, 150, 178, 152, 163, 107, 149, 97.8, 90.1, 94.3, 90.9, 89.9, 53.4 };
			double[,] ar_2d = new double[,] { { 1, 2 }, { 1, 2 } };

			// initialize variables for return value from ML
			System.Array cr = new double[3];
            System.Array ci = new double[3];

			// call MATLAB function
            UseEngine(ar_2d, ref cr, ref ci);

			// Output stuff..
			if (writeStuff==true) {
				//Console.Clear();
				Console.WriteLine("Input was:");
				Console.WriteLine(String.Join(",", ar.Select(p => p.ToString()).ToArray()));
				Console.WriteLine("Output was:");
				double [] cr_d = new double[3];
				cr_d = (double [])cr;
				Console.WriteLine(String.Join(",", cr_d.Select(p => p.ToString()).ToArray()));
				DisplayEnd();
			}


		}
		static private void UseEngine(double[,] ar, ref Array cr, ref Array ci) {
			// Instantiate MATLAB Engine Interface through com

			Console.WriteLine("Matlab Startup...\r\n");
			MLApp.MLAppClass matlab = new MLApp.MLAppClass();

			// Make imaginary matricies
            double[] a_d = new double[] { 68.8, 120, 110, 118, 150, 179, 180, 150, 178, 152, 163, 107, 149, 97.8, 90.1, 94.3, 90.9, 89.9, 53.4 };
			matlab.PutFullMatrix("a", "base", a_d, new double[19]);

			String MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			String ProjectLocation = "Visual Studio 2010\\Projects\\Kinect-Tracking-Project\\MatlabPrototypes\\FeatureDetection";
			String matFileCD_command = String.Format("cd '{0}';", System.IO.Path.Combine(MyDocs, ProjectLocation));

			Console.WriteLine(matFileCD_command);

			matlab.Execute(matFileCD_command);
			// matlab.Execute("open testBayes.m");
			// matlab.Execute("dbstop in math_on_numbers.m");
			Console.WriteLine("Matlab Processing...\r\n");
            matlab.Execute("c = transpose(testBayes(a));");
			
			//matlab.Execute("com.mathworks.mlservices.MLEditorServices.closeAll");
			//matlab.Execute("dbquit all");
			
			
			try {
				matlab.GetFullMatrix("c", "base", ref cr, ref ci);
			} catch (Exception) {
				Console.WriteLine("someErr");
			}
		}

		static private void DisplayEnd() {
			ConsoleKeyInfo keypressed;

			Console.WriteLine();
			Console.WriteLine("Press any key to exit.");

			keypressed = Console.ReadKey(true);

		}



	}

}