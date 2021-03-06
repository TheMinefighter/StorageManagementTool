﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace UpdateInstaller {
	public static class Update {
		public const string UpdateFileName = "Name";
		public const string UpdateFileTarget = "Target";
		public const string UpdateFileMD5 = "MD5";
		public const string UpdateDataDirectory = "UpdateData";

		public static void Main(string[] args) {
			Console.Title = "Update installer of the StorageManagementTool";
			Console.WriteLine("This is the update installer of the StorageManagementTool");
			Console.WriteLine();
				Console.WriteLine("Wait a second or two...");//Only waiting that parent quits
				Thread.Sleep(2000);
			Console.WriteLine("Reading Update Data...");
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
			catch (Exception e1) {
				Console.WriteLine("Could not read UpdateData\\UpdateMeta.xml. Details below. Press a key to quit.");
				Console.WriteLine(e1);
				Console.Read();
				Environment.Exit(-1);
			}

			XElement root1 = meta.Root;
			if (root1 is null) {
				Console.WriteLine("The root tag of UpdateData\\UpdateMeta.xml does not exist.  Press a key to quit.");
				Console.Read();
				Environment.Exit(-1);
			}

			XAttribute newVersion = root1.Attribute("NewVersion");
			if (newVersion is null) {
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
			Console.WriteLine();
			Console.WriteLine("Verifying update package integrity...");
			XElement content = (XElement) root1.Nodes().FirstOrDefault(x => x is XElement e && e.Name == "UpdateMeta.Content");
			if (content is null) {
				Console.WriteLine("The update package does not define it's content. Press a key to quit.");
				Console.Read();
				Environment.Exit(-1);
			}

			XElement[] updateFiles = content.Nodes().OfType<XElement>().Where(x => x.Name == "UpdateFile")
				.ToArray();
			if (updateFiles.Length==0)
			{
				Console.WriteLine("The update package is empty. Press a key to quit.");
				Console.Read();
				Environment.Exit(-1);
            }

			bool violations = false;
			for (int i = 0; i < updateFiles.Length; i++) {
				XAttribute name = updateFiles[i].Attribute(UpdateFileName);
				if (name is null) {
					Console.WriteLine($"The UpdateFile {i} has no name defined. Press a key to quit.");
					Console.Read();
					Environment.Exit(-1);
				}

				if (!File.Exists(Path.Combine(Environment.CurrentDirectory, UpdateDataDirectory, name.Value))) {
					Console.WriteLine($"The UpdateFile {i} ({name.Value}) does not exist. Press a key to quit.");
					Console.Read();
					Environment.Exit(-1);
				}

				XAttribute target1 = updateFiles[i].Attribute(UpdateFileTarget);
				if (target1 is null) {
					Console.WriteLine($"The UpdateFile {i} has no target defined. Press a key to quit.");
					Console.Read();
					Environment.Exit(-1);
				}

				XAttribute expectedHash = updateFiles[i].Attribute(UpdateFileMD5);
				if (expectedHash is null) {
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
					}
				}
			}

			if (violations) {
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
					string target = updateFile.Attribute(UpdateFileTarget).Value;
					new FileInfo(Path.Combine(Environment.CurrentDirectory, target)).Directory.Create();
					File.Copy(
						Path.Combine(Environment.CurrentDirectory, UpdateDataDirectory, updateFile.Attribute(UpdateFileName).Value),
						Path.Combine(Environment.CurrentDirectory, target));
				}
			}
			catch (Exception e) {
				Console.WriteLine("An error occurred inserting the updated files. Details below. Press a key to quit");
				Console.WriteLine(e);
				Console.Read();
				Environment.Exit(-1);
			}

			Console.WriteLine("Update installation complete, cleaning up...");
			try {
				Directory.Delete(Path.Combine(Environment.CurrentDirectory, UpdateDataDirectory), true);
			}
			catch (Exception e) {
				Console.WriteLine("An error occurred removing the update package. Details below. Press a key to quit");
				Console.WriteLine(e);
				Console.Read();
				Environment.Exit(-1);
			}

			Console.WriteLine("Cleanup done, starting StorageManagementTool");
			StringBuilder argsBuilder = new StringBuilder(255);
			foreach (string s in args) {
				argsBuilder.Append(" \"");
				argsBuilder.Append(s);
				argsBuilder.Append('\"');
			}

			try {
				new Process {
					StartInfo = new ProcessStartInfo(Path.Combine(Environment.CurrentDirectory, "StorageManagementCLI.bat"),
						argsBuilder.ToString())
				}.Start();
			}
			catch (Exception e) {
				Console.WriteLine("An error occurred while restarting the StorageManagementTool. Details below. Press a key to quit");
				Console.WriteLine(e);
				Console.Read();
				Environment.Exit(-1);
			}

			Environment.Exit(0);
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
	}
}