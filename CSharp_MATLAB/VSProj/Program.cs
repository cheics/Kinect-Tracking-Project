using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;

// MathWorks assemblies that ship with Builder for .NET
// and should be registered in Global Assembly Cache
//using MathWorks.MATLAB.NET.Utility;
//using MathWorks.MATLAB.NET.Arrays;

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

            //DataTable table = new DataTable();
            //table.Columns.Add("Name", typeof(double));
            //table.Columns.Add("Price", typeof(double));
            //table.Columns.Add("Date", typeof(double));

            //table.Rows.Add(1.5, 6.3, 8.9);
            //table.Rows.Add(88.4, 17.1, 0.8);
            //table.Rows.Add(0.04, 63.1, 8.75);
            //table.Rows.Add(0.04, 63.1, 99);

            //DataTable table1 = table.Clone();
            //for (int i = 0; i < table.Rows.Count; i++)
            //{
            //    for (int j = 0; j < table.Columns.Count; j++)
            //    {
            //        string test = "test";
            //    }
            //    table1.ImportRow(table.Rows[1]);
            //}

            //double[] ground = new double[4] {7.5, 8.3, 1.4, 7};

            //string exercise = "arm raise";

            //double[] output = scores(table, ground, exercise);

            //Console.ReadLine();

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

        static private double[] scores(DataTable kinectTable, double[] groundPlane, string exercise)
        {
            double[,] kinectData = new double[kinectTable.Rows.Count, kinectTable.Columns.Count];
            double[,] kinectZeros = new double[kinectTable.Rows.Count, kinectTable.Columns.Count];
            double[] groundPlaneZeros = new double[4];

            System.Array cr = new double[3];
            System.Array ci = new double[3];

            for (int r = 0; r < kinectTable.Rows.Count; r++)
            {
                for (int c = 0; c < kinectTable.Columns.Count; c++)
                {
                    kinectData[r, c] = (double)kinectTable.Rows[r][c];
                }
            }

            MLApp.MLAppClass matlab = new MLApp.MLAppClass();
            matlab.PutFullMatrix("CS_kinectData", "base", kinectData, kinectZeros);
            matlab.PutFullMatrix("CS_groundPlane", "base", groundPlane, groundPlaneZeros);



            String MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            String ProjectLocation = "Visual Studio 2010\\Projects\\Kinect-Tracking-Project\\MatlabPrototypes\\FeatureDetection";
            String matFileCD_command = String.Format("cd '{0}';", System.IO.Path.Combine(MyDocs, ProjectLocation));

            matlab.Execute(matFileCD_command);
            if (exercise == "squats")
            {
                matlab.Execute("c = cs_matlab_classifier(CS_kinectData, CS_groundPlane, 'squats');");
            }
            else if (exercise == "arm raise")
            {
                matlab.Execute("c = transpose(testBayes(CS_kinectData, CS_groundPlane, 'arm raise'));");
            }
            else if (exercise == "leg raise")
            {
                matlab.Execute("c = transpose(testBayes(CS_kinectData, CS_groundPlane, 'leg raise'));");
            }
            else if (exercise == "leg extension")
            {
                matlab.Execute("c = transpose(testBayes(CS_kinectData, CS_groundPlane, 'leg extension'));");
            }
            

            matlab.GetFullMatrix("c", "base", cr, ci);

            double[] cr_d = new double[3];
            cr_d = (double[])cr;
            return cr_d;
        }



	}

}