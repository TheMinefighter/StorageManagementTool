using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;
using static StorageManagementCore.WPFGUI.GlobalizationRessources.MoveObjectsStrings;

namespace StorageManagementCore.WPFGUI {
	public partial class MainWindow {
		private void MoveFileOrFolderTi_OnLoaded(object sender, RoutedEventArgs e) {
			LocalizeMoveObjects();
			SuggestionsLb.Items.Clear();
			SuggestionsLb.ItemsSource = OperatingMethods.GetRecommendedPaths();
			MoveObjectsRootPathTb.Text = Session.Singleton.Configuration.DefaultHDDPath ?? "";
		}

		private void LocalizeMoveObjects() {
			MoveFileOrFolderTi.Header = MoveFileOrFolderTiText;
			SelectFilesToMoveBtn.Content = FileToMove_btn_Text;
			SelectFoldersToMoveBtn.Content = FolderToMove_btn_Text;
			ObjectsToMoveLbl.Text = ObjectsToMoveLblText;
			IsMoveObjectsRootPathAbsoluteCb.Content = IsPathAbsoluteCbText;
			SelectMoveObjectsRootPathBtn.Content = SetRootPath_btn_Text;
			MoveObjectsRootPathLbl.Text = RootPathLblText;
			SetMoveObjectsRootPathConfigBtn.Content = SetHDDPathAsDefault_btn_Text;
			SuggestionsLbl.Text = Suggestions_gb_Text;
			MoveObjectsBtn.Content = MoveObjectsBtnText;
		}

		private void SelectFoldersToMoveBtn_Click(object sender, RoutedEventArgs e) {
			PathsToMoveTb.Text = string.Join(";", FileAndFolder.SelectDirectories().Select(x => x.FullName));
			SuggestionsLb.UnselectAll();
		}

		private void SelectFilesToMoveBtn_OnClick(object sender, RoutedEventArgs e) {
			PathsToMoveTb.Text = string.Join(";", FileAndFolder.SelectFiles().Select(x => x.FullName));
			SuggestionsLb.UnselectAll();
		}

		private void PathsToMoveTb_TextChanged(object sender, TextChangedEventArgs e) {
			PathsToMoveTb.Background = PathsToMoveTb.Text.Split(';').All(x => Directory.Exists(x) || File.Exists(x))
				? Brushes.White
				: Brushes.DarkOrange;
		}

		private void RootPathTb_TextChanged(object sender, TextChangedEventArgs e) {
			bool exists = Directory.Exists(MoveObjectsRootPathTb.Text);
			SetMoveObjectsRootPathConfigBtn.IsEnabled = exists;
			MoveObjectsRootPathTb.Background = exists ? Brushes.White : Brushes.DarkOrange;
		}

		private void SelectRootPathBtn_Click(object sender, RoutedEventArgs e) {
			MoveObjectsRootPathTb.Text = FileAndFolder.SelectDirectory().FullName;
		}

		private void SetRootPathConfigBtn_Click(object sender, RoutedEventArgs e) {
			if (IsMoveObjectsRootPathAbsoluteCb.IsChecked == false) {
				SetRootPathChecked();
			}
		}

		private void SetRootPathChecked() {
			if (Directory.Exists(MoveObjectsRootPathTb.Text)) {
				Session.Singleton.Configuration.DefaultHDDPath = MoveObjectsRootPathTb.Text;
				Session.Singleton.SaveCfg();
			}
		}

		private void MoveObjectsBtn_Click(object sender, RoutedEventArgs e) {
			if (!Directory.Exists(MoveObjectsRootPathTb.Text)) {
				//TODO Throw
			}

			if (IsMoveObjectsRootPathAbsoluteCb.IsChecked == true) {
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
						OperatingMethods.MoveFile(fileToMove,
							new FileInfo(Path.Combine(MoveObjectsRootPathTb.Text, fileToMove.Name)));
						break;
					case FileAndFolder.FileOrFolder.Folder:
						OperatingMethods.MoveFolder(new DirectoryInfo(PathsToMoveTb.Text),
							new DirectoryInfo(MoveObjectsRootPathTb.Text));
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
							OperatingMethods.MoveFile(fileToMove,new DirectoryInfo(MoveObjectsRootPathTb.Text));
							break;
						case FileAndFolder.FileOrFolder.Folder:
							OperatingMethods.MoveFolder(new DirectoryInfo(s), new DirectoryInfo(MoveObjectsRootPathTb.Text),
								true);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}
		}

		private void SuggestionsLb_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (SuggestionsLb.SelectedItems.Count != 0) {
				PathsToMoveTb.Text = string.Join(";", SuggestionsLb.SelectedItems.Cast<string>());
			}
		}
	}
}