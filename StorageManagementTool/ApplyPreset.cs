using System;
using System.Linq;
using System.Windows.Forms;
using StorageManagementTool.GlobalizationRessources;
using static StorageManagementTool.GlobalizationRessources.ApplyPresetStrings;

namespace StorageManagementTool
{
    public partial class ApplyPreset : Form
    {
        public ApplyPreset()
        {
            InitializeComponent();
        }

        private void ApplyPreset_Load(object sender, EventArgs e)
        {
            if (!Session.Singleton.Isadmin)
            {
                if (MessageBox.Show(Load_AdministratorRequired, Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    Wrapper.RestartAsAdministrator();
                }
                else
                {
                    Close();
                }
            }
            SelectHDD_lbl.Text = SelectHDD_lbl_Text;
            SelectSSD_lbl.Text = SelectSSD_lbl_Text;
            ApplyPreset_btn.Text = ApplyPreset_btn_Text;
            SelectScenario_lbl.Text = SelectScenario_lbl_Text;
            Session.Singleton.FillWithDriveInfo(SelectHDD_lb);
            Session.Singleton.FillWithDriveInfo(SelectSSD_lb);
            foreach (ScenarioPreset availablePreset in ScenarioPreset.AvailablePresets)
            {
                //SelectScenario_lb.Items.Add(availablePreset.Name);
            }

          //  SelectScenario_lb.Items.AddRange(ScenarioPreset.AvailablePresets.Select(x => x.Name).ToArray());

        }

        private void ApplyPreset_btn_Click(object sender, EventArgs e)
        {
            if (SelectScenario_lb.SelectedIndex == -1)
            {
                MessageBox.Show(NoScenarioSelected, Error, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                return;
            }
            ScenarioPreset toApply = ScenarioPreset.AvailablePresets[SelectScenario_lb.SelectedIndex];
            if (toApply.HDDRequired && SelectHDD_lb.SelectedIndex == -1)
            {
                MessageBox.Show(NoHDDSelectedButRequired, Error, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                return;
            }
            if (toApply.SSDRequired && SelectSSD_lb.SelectedIndex == -1)
            {
                MessageBox.Show(NoSSDSelectedButRequired, Error, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                return;
            }

            toApply.toRun(
                Session.Singleton.CurrentDrives.First(x =>
                    OperatingMethods.DriveInfo2String(x) == SelectSSD_lb.SelectedItem.ToString()),
                Session.Singleton.CurrentDrives.First(x =>
                    OperatingMethods.DriveInfo2String(x) == SelectHDD_lb.SelectedItem.ToString()));
        }
    }
}