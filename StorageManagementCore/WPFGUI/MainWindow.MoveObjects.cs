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
			PathsToMoveTb.Text = string.Join(";", FileAndFolder.SelectDirectories("").Select(x => x.FullName));
			SuggestionsLb.UnselectAll();
		}

		private void SelectFilesToMoveBtn_OnClick(object sender, RoutedEventArgs e) {
			PathsToMoveTb.Text = string.Join(";", FileAndFolder.SelectFiles("").Select(x => x.FullName));
			SuggestionsLb.UnselectAll();
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
			BasePathTb.Text = FileAndFolder.SelectDirectory("").FullName;
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

				switch (FileAndFolder.IsFileOrFolder(PathsToMoveTb.Text)) {
					case FileAndFolder.FileOrFolder.Neither:
						//TODO Throw error
						return;
						break;
					case FileAndFolder.FileOrFolder.File:
						FileInfo fileToMove = new FileInfo(PathsToMoveTb.Text);
						OperatingMethods.MoveFile(fileToMove, new FileInfo(Path.Combine(BasePathTb.Text, fileToMove.Name)));
						break;
					case FileAndFolder.FileOrFolder.Folder:
						OperatingMethods.MoveFolder(new DirectoryInfo(PathsToMoveTb.Text), new DirectoryInfo(BasePathTb.Text));
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else {
				foreach (string s in PathsToMoveTb.Text.Split(';')) {
					switch (FileAndFolder.IsFileOrFolder(s)) {
						case FileAndFolder.FileOrFolder.Neither:
							//TODO Throw error
							return;

							break;
						case FileAndFolder.FileOrFolder.File:
							FileInfo fileToMove = new FileInfo(s);
							OperatingMethods.MoveFile(fileToMove, new FileInfo(s), true);
							break;
						case FileAndFolder.FileOrFolder.Folder:
							OperatingMethods.MoveFolder(new DirectoryInfo(s), new DirectoryInfo(BasePathTb.Text), true);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}
		}

		private void MoveFileOrFolderTi_OnLoaded(object sender, RoutedEventArgs e) {
			SuggestionsLb.Items.Clear();
			SuggestionsLb.ItemsSource = OperatingMethods.GetRecommendedPaths();
		}

//		private IEnumerator iEnumerator;
		private void SuggestionsLb_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (SuggestionsLb.SelectedItems.Count != 0) {
//			iEnumerator = SuggestionsLb.SelectedItems.GetEnumerator();
//				iEnumerator.MoveNext();
//			MessageBox.Show(iEnumerator.ToString());
//				object test = iEnumerator.Current;
//			MessageBox.Show(test.ToString());
				PathsToMoveTb.Text = string.Join(";", SuggestionsLb.SelectedItems.Cast<string>());
			}
		}
	}
}