using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using csmatio.types;
using csmatio.io;

namespace CSMatIOTest
{
	class Program {
		static void Main(string[] args) {
			List<MLArray> mlList = new List<MLArray>();
			mlList.Add(CreateTestStruct());

			try {
				MatFileWriter mfw = new MatFileWriter("C:\\Users\\cheics\\Documents\\cc.mat", mlList, true);
			} catch (Exception err) {
				Console.WriteLine("shit...");
			}
		}

		private static MLArray CreateTestStruct() {
			MLStructure outStruct = new MLStructure("kinectData", new int[] { 1, 1 });
			double[] myRealNums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
			// new MLInt64("IA", myRealNums, myImagNums, length);
			// IA: matlab Name (blank for struct)
			// realNumbers => Double array of the real parts
			// imagNumbers => Double array of the img parts
			// length => dimension of the matrix
			outStruct["skelData", 0] = new MLDouble("", myRealNums, 2);
			outStruct["dateHeader", 0] = new MLChar("", "January 29th... etc...");

			MLStructure groundStruct = new MLStructure("", new int[] { 1, 1 });
			groundStruct["height", 0] = new MLDouble("", new double[] { 0.68 }, 1); // metres?
			groundStruct["gpVector", 0] = new MLDouble("", new double[] {1.4, 1, 1, 1}, 4); //metres?
			groundStruct["kinectTilt", 0] = new MLInt16("", new Int16[]{ -15 }, 1); //degrees
			outStruct["groundPlaneData", 0] = groundStruct;

			return outStruct;
		}
	}
}
