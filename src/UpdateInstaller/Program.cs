using System;
using System.IO;
using System.Xml.Linq;

namespace UpdateInstaller {
	internal class Program {
		public static void Main(string[] args) {
			Console.Title = "Update installer of the StorageManagementTool";
			Console.WriteLine("This is the update installer of the StorageManagementTool");
			Console.WriteLine();
			Console.WriteLine("Reading Update Data...");
			if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory,"UpdateData"))) {
				Console.WriteLine("The UpdateData directory does not exist. Press a key to quit.");
				Console.Read();
				Environment.Exit(-1);
			}
			if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory,"UpdateData","UpdateMeta.xml"))) {
				Console.WriteLine("The UpdateData\\UpdateMeta.xml file does not exist. Press a key to quit.");
				Console.Read();
				Environment.Exit(-1);
			}

			XElement meta;
			try {
				meta = XElement.Load(Path.Combine(Environment.CurrentDirectory, "UpdateData", "UpdateMeta.xml"));
			}
			catch (Exception e) {
				Console.WriteLine("Could not read UpdateData\\UpdateMeta.xml. Details below. Press a key to quit.");
				Console.WriteLine(e);
				Console.Read();
				Environment.Exit(-1);
			}
			
		}
	}
}