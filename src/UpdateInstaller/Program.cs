using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace UpdateInstaller {
	internal static class Program {
		private const string UpdateFileName = "Name";
		private const string UpdateFileTarget = "Target";
		private const string UpdateFileMD5 = "MD5";
		private const string UpdateDataDirectory = "UpdateData";

		public static void Main(string[] args) {
			Console.Title = "Update installer of the StorageManagementTool";
			Console.WriteLine("This is the update installer of the StorageManagementTool");
			Console.WriteLine();
			Console.WriteLine("Reading Update Data...");
			XElement root = GetRoot();
			NewVersion(root);
			Console.WriteLine();
			Console.WriteLine("Verifying update package integrity...");
			XElement content = (XElement) root.Nodes().FirstOrDefault(x => x is XElement e && e.Name == "UpdateMeta.Content");
			if (content == null) {
				Console.WriteLine("The update package does not define it's content. Press a key to quit.");
				Console.Read();
				Environment.Exit(-1);
			}

			XElement[] updateFiles = content.Nodes().Where(x => x is XElement).Cast<XElement>().Where(x => x.Name == "UpdateFile")
				.ToArray();
			bool violation = VerifyPackage(updateFiles);
			if (violation) {
				Console.WriteLine("No further integrity violations detected.");
			}
			else {
				Console.WriteLine("No integrity violations detected.");
			}

			Console.WriteLine("Removing previous version...");
			DirectoryInfo cd = new DirectoryInfo(Environment.CurrentDirectory);
			try {
				foreach (DirectoryInfo toDelete in cd.EnumerateDirectories().Where(x => x.Name != UpdateDataDirectory)) {
					toDelete.Delete(true);
				}

				foreach (FileInfo fileInfo in cd.EnumerateFiles().Where(x => x.Name != "UpdateInstaller.exe")) {
					fileInfo.Delete();
				}
			}
			catch (Exception e) {
				Console.WriteLine("An error occurred while removing the old installation. Details below. Press a key to quit");
				Console.WriteLine(e);
				Console.Read();
				Environment.Exit(-1);
			}

			Console.WriteLine("Installing Update...");
			try {
				foreach (XElement updateFile in updateFiles) {
					File.Copy(
						Path.Combine(Environment.CurrentDirectory, UpdateDataDirectory, updateFile.Attribute(UpdateFileName).Value),
						Path.Combine(Environment.CurrentDirectory, updateFile.Attribute(UpdateFileTarget).Value));
				}
			}
			catch (Exception e) {
				Console.WriteLine("An error occurred while removing the old installation. Details below. Press a key to quit");
				Console.WriteLine(e);
				Console.Read();
				Environment.Exit(-1);
			}
			Console.WriteLine("Update installation complete, starting StorageManagementTool");
			StringBuilder argsBuilder= new StringBuilder(255);
			foreach (string s in args) {
				argsBuilder.Append(" \"");
				argsBuilder.Append(s);
				argsBuilder.Append('\"');
			}
			new Process {StartInfo = new ProcessStartInfo(Path.Combine(Environment.CurrentDirectory,"StorageMangementCLI.bat"),argsBuilder.ToString())}.Start();
			Environment.Exit(0);
		}

		private static bool VerifyPackage(XElement[] updateFiles) {
			bool violations = false;
			for (int i = 0; i < updateFiles.Length; i++) {
				XAttribute name = updateFiles[i].Attribute(UpdateFileName);
				if (name == null) {
					Console.WriteLine($"The UpdateFile {i} has no name defined. Press a key to quit.");
					Console.Read();
					Environment.Exit(-1);
				}

				if (!File.Exists(Path.Combine(Environment.CurrentDirectory, UpdateDataDirectory, name.Value))) {
					Console.WriteLine($"The UpdateFile {i} ({name.Value}) does not exist. Press a key to quit.");
					Console.Read();
					Environment.Exit(-1);
				}

				XAttribute target = updateFiles[i].Attribute(UpdateFileTarget);
				if (target == null) {
					Console.WriteLine($"The UpdateFile {i} has no target defined. Press a key to quit.");
					Console.Read();
					Environment.Exit(-1);
				}

				XAttribute expectedHash = updateFiles[i].Attribute(UpdateFileMD5);
				if (expectedHash == null) {
					Console.WriteLine($"The UpdateFile {i} has no hash defined. Press P to proceed or any other key to quit.");
					if (Console.ReadKey().Key != ConsoleKey.P) {
						Environment.Exit(-1);
					}
					else {
						Console.WriteLine();
						violations = true;
						continue;
					}
				}

				if (!string.Equals(ComputeHash(Path.Combine(Environment.CurrentDirectory, UpdateDataDirectory, name.Value)),
					expectedHash.Value,
					StringComparison.OrdinalIgnoreCase)) {
					Console.WriteLine($"The UpdateFile {i} has an invalid hash. Press P to proceed or any other key to quit.");
					if (Console.ReadKey().Key != ConsoleKey.P) {
						Environment.Exit(-1);
					}
					else {
						Console.WriteLine();
						violations = true;
						continue;
					}
				}
			}

			return violations;
		}

		private static string ByteArrayToString(byte[] ba) => BitConverter.ToString(ba).Replace("-", "");

		private static string ComputeHash(string path) {
			try {
				using (MD5 md5 = MD5.Create()) {
					using (FileStream stream = File.OpenRead(path)) {
						return ByteArrayToString(md5.ComputeHash(stream));
					}
				}
			}
			catch (Exception e) {
				Console.WriteLine($"An error occurred while calculating the hash of {path}. Details below. Press a key to quit");
				Console.WriteLine(e);
				Console.Read();
				Environment.Exit(-1);
				return null;
			}
		}

		private static XElement GetRoot() {
			if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UpdateDataDirectory))) {
				Console.WriteLine("The UpdateData directory does not exist. Press a key to quit.");
				Console.Read();
				Environment.Exit(-1);
			}

			if (!File.Exists(Path.Combine(Environment.CurrentDirectory, UpdateDataDirectory, "UpdateMeta.xml"))) {
				Console.WriteLine("The UpdateData\\UpdateMeta.xml file does not exist. Press a key to quit.");
				Console.Read();
				Environment.Exit(-1);
			}

			XDocument meta = null;
			try {
				meta = XDocument.Load(Path.Combine(Environment.CurrentDirectory, UpdateDataDirectory, "UpdateMeta.xml"));
			}
			catch (Exception e) {
				Console.WriteLine("Could not read UpdateData\\UpdateMeta.xml. Details below. Press a key to quit.");
				Console.WriteLine(e);
				Console.Read();
				Environment.Exit(-1);
			}

			XElement root = meta.Root;
			if (root == null) {
				Console.WriteLine("The root tag of UpdateData\\UpdateMeta.xml does not exist.  Press a key to quit.");
				Console.Read();
				Environment.Exit(-1);
			}

			return root;
		}

		private static void NewVersion(XElement root) {
			XAttribute newVersion = root.Attribute("NewVersion");
			if (newVersion == null) {
				Console.WriteLine(
					"The NewVersion attribute of the root tag of UpdateData\\UpdateMeta.xml does not exist. Press a key to quit.");
				Console.Read();
				Environment.Exit(-1);
			}

			string newVersionValue = newVersion.Value;
			if (newVersionValue == "") {
				Console.WriteLine(
					"The NewVersion attribute of the root tag of UpdateData\\UpdateMeta.xml is not defined properly. Press a key to quit.");
				Console.Read();
				Environment.Exit(-1);
			}

			Console.WriteLine($"The version to be installed is {newVersionValue}.");
		}
	}
}