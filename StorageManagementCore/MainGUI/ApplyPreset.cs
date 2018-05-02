using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static StorageManagementCore.MainGUI.GlobalizationRessources.ApplyPresetStrings;

namespace StorageManagementCore.MainGUI {
	public partial class ApplyPreset : Form {
		public ApplyPreset() {
			InitializeComponent();
		}

		private void ApplyPreset_Load(object sender, EventArgs e) {
			if (!Session.Singleton.IsAdmin) {
				if (MessageBox.Show(Load_AdministratorRequired, Error,
					    MessageBoxButtons.YesNo,
					    MessageBoxIcon.Error,
					    MessageBoxDefaultButton.Button1) == DialogResult.Yes) {
					Wrapper.RestartAsAdministrator();
				}
				else {
					Close();
				}
			}

			SelectHDD_lbl.Text = SelectHDD_lbl_Text;
			SelectSSD_lbl.Text = SelectSSD_lbl_Text;
			ApplyPreset_btn.Text = ApplyPreset_btn_Text;
			SelectScenario_lbl.Text = SelectScenario_lbl_Text;
			Session.Singleton.FillWithDriveInfo(SelectHDD_lb);
			Session.Singleton.FillWithDriveInfo(SelectSSD_lb);
			Text = WindowTitle;
			SelectScenario_lb.Items.AddRange(ScenarioPreset.AvailablePresets.Select(x => x.ViewedName).ToArray());
		}

		private void ApplyPreset_btn_Click(object sender, EventArgs e) {
			if (SelectScenario_lb.SelectedIndex == -1) {
				MessageBox.Show(NoScenarioSelected, Error, MessageBoxButtons.OK,
					MessageBoxIcon.Error,
					MessageBoxDefaultButton.Button1);
				return;
			}

			ScenarioPreset toApply = ScenarioPreset.AvailablePresets[SelectScenario_lb.SelectedIndex];
			if (toApply.HDDRequired && SelectHDD_lb.SelectedIndex == -1) {
				MessageBox.Show(NoHDDSelectedButRequired, Error, MessageBoxButtons.OK,
					MessageBoxIcon.Error,
					MessageBoxDefaultButton.Button1);
				return;
			}

			if (toApply.SSDRequired && SelectSSD_lb.SelectedIndex == -1) {
				MessageBox.Show(NoSSDSelectedButRequired, Error, MessageBoxButtons.OK,
					MessageBoxIcon.Error,
					MessageBoxDefaultButton.Button1);
				return;
			}

			IEnumerable<DriveInfo> driveInfos = Wrapper.getDrives();
			DriveInfo HDDToUse = driveInfos.First(x =>
				OperatingMethods.GetDriveInfoDescription(x) == SelectHDD_lb.SelectedItem.ToString());
			DriveInfo SSDToUse = driveInfos.First(x =>
				OperatingMethods.GetDriveInfoDescription(x) == SelectSSD_lb.SelectedItem.ToString());
			toApply.ToRun(
				SSDToUse,
				HDDToUse);
		}
	}
}