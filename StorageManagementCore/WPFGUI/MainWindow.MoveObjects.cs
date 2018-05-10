using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void SelectFoldersToMoveBtn_Click(object sender, RoutedEventArgs e) {
			PathsToMoveTb.Text = string.Join(";", Wrapper.FileAndFolder.SelectDirectories("").Select(x => x.FullName));
		}

		private void SelectFilesToMoveBtn_OnClick(object sender, RoutedEventArgs e) {
			PathsToMoveTb.Text = string.Join(";", Wrapper.FileAndFolder.SelectFiles("").Select(x => x.FullName));
		}

		private void PathsToMoveTb_TextChanged(object sender, TextChangedEventArgs e) {
			PathsToMoveTb.Background = PathsToMoveTb.Text.Split(';').All(x => Directory.Exists(x) || File.Exists(x))
				? Brushes.White
				: Brushes.DarkOrange;
		}

		private void BasePathTb_TextChanged(object sender, TextChangedEventArgs e) {
			BasePathTb.Background = Directory.Exists(BasePathTb.Text) ? Brushes.White : Brushes.DarkOrange;
		}

		private void SelectBasePathBtn_Click(object sender, RoutedEventArgs e) {
			BasePathTb.Text = Wrapper.FileAndFolder.SelectDirectory("").FullName;
		}

		private void SetBasePathConfigBtn_Click(object sender, RoutedEventArgs e) {
			if (IsBasePathAbsoluteCb.IsChecked == false) {
				SetBasePathChecked();
			}
			else {
				//TODO Connat Absolute
			}
		}

		private void SetBasePathChecked() {
			if (Directory.Exists(BasePathTb.Text)) {
				Session.Singleton.CurrentConfiguration.DefaultHDDPath = BasePathTb.Text;
			}
			else {
				//TODO Throw not available
			}
		}

		private void MoveObjectsBtn_Click(object sender, RoutedEventArgs e) {
			if (!Directory.Exists(BasePathTb.Text)) {
				//TODO Throw
			}

			if (IsBasePathAbsoluteCb.IsChecked == true) {
				if (PathsToMoveTb.Text.Contains(';')) {
					//TODO Not available
				}

				switch (Wrapper.FileAndFolder.IsFileOrFolder(PathsToMoveTb.Text)) {
					case Wrapper.FileAndFolder.FileOrFolder.Neither:
						//TODO Throw error
						return;
						break;
					case Wrapper.FileAndFolder.FileOrFolder.File:
						FileInfo fileToMove = new FileInfo(PathsToMoveTb.Text);
						OperatingMethods.MoveFile(fileToMove, new FileInfo(Path.Combine(BasePathTb.Text, fileToMove.Name)));
						break;
					case Wrapper.FileAndFolder.FileOrFolder.Folder:
						OperatingMethods.MoveFolder(new DirectoryInfo(PathsToMoveTb.Text), new DirectoryInfo(BasePathTb.Text));
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else {
				foreach (string s in PathsToMoveTb.Text.Split(';')) {
					switch (Wrapper.FileAndFolder.IsFileOrFolder(s)) {
						case Wrapper.FileAndFolder.FileOrFolder.Neither:
							//TODO Throw error
							return;
							break;
						case Wrapper.FileAndFolder.FileOrFolder.File:
							FileInfo fileToMove = new FileInfo(s);
							OperatingMethods.MoveFile(fileToMove, new FileInfo(s), true);
							break;
						case Wrapper.FileAndFolder.FileOrFolder.Folder:
							OperatingMethods.MoveFolder(new DirectoryInfo(s), new DirectoryInfo(BasePathTb.Text), true);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}
		}
	}
}