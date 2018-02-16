using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using StorageManagementTool.GlobalizationRessources;
using StorageManagementTool.MainGUI.GlobalizationRessources;

namespace StorageManagementTool.MainGUI
{
   public partial class ApplyPreset : Form
   {
      public ApplyPreset()
      {
         InitializeComponent();
      }

      private void ApplyPreset_Load(object sender, EventArgs e)
      {
         if (!Session.Singleton.IsAdmin)
         {
            if (MessageBox.Show(ApplyPresetStrings.Load_AdministratorRequired, ApplyPresetStrings.Error, MessageBoxButtons.YesNo, MessageBoxIcon.Error,
                   MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
               Wrapper.RestartAsAdministrator();
            }
            else
            {
               Close();
            }
         }

         SelectHDD_lbl.Text = ApplyPresetStrings.SelectHDD_lbl_Text;
         SelectSSD_lbl.Text = ApplyPresetStrings.SelectSSD_lbl_Text;
         ApplyPreset_btn.Text = ApplyPresetStrings.ApplyPreset_btn_Text;
         SelectScenario_lbl.Text = ApplyPresetStrings.SelectScenario_lbl_Text;
         Session.Singleton.FillWithDriveInfo(SelectHDD_lb);
         Session.Singleton.FillWithDriveInfo(SelectSSD_lb);
         Text = ApplyPresetStrings.WindowTitle;
         SelectScenario_lb.Items.AddRange(ScenarioPreset.AvailablePresets.Select(x => x.ViewedName).ToArray());
      }

      private void ApplyPreset_btn_Click(object sender, EventArgs e)
      {
         if (SelectScenario_lb.SelectedIndex == -1)
         {
            MessageBox.Show(ApplyPresetStrings.NoScenarioSelected, ApplyPresetStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error,
               MessageBoxDefaultButton.Button1);
            return;
         }

         ScenarioPreset toApply = ScenarioPreset.AvailablePresets[SelectScenario_lb.SelectedIndex];
         if (toApply.HDDRequired && SelectHDD_lb.SelectedIndex == -1)
         {
            MessageBox.Show(ApplyPresetStrings.NoHDDSelectedButRequired, ApplyPresetStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error,
               MessageBoxDefaultButton.Button1);
            return;
         }

         if (toApply.SSDRequired && SelectSSD_lb.SelectedIndex == -1)
         {
            MessageBox.Show(ApplyPresetStrings.NoSSDSelectedButRequired, ApplyPresetStrings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error,
               MessageBoxDefaultButton.Button1);
            return;
         }

         IEnumerable < DriveInfo > driveInfos= FileSystem.Drives;
         DriveInfo HDDToUse = driveInfos.First(x =>
            OperatingMethods.DriveInfo2String(x) == SelectHDD_lb.SelectedItem.ToString());
         DriveInfo SSDToUse = driveInfos.First(x =>
            OperatingMethods.DriveInfo2String(x) == SelectSSD_lb.SelectedItem.ToString());
         toApply.ToRun(
            SSDToUse,
            HDDToUse);
      }
   }
}