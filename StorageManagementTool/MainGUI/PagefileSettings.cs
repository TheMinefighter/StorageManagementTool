using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using static StorageManagementTool.MainGUI.GlobalizationRessources.PagefileSettingsStrings;

namespace StorageManagementTool.MainGUI {
   public partial class PagefileSettings : Form {
      private OperatingMethods.SwapfileMethods.SwapfileState currentState;

      public PagefileSettings() {
         InitializeComponent();
      }

      private void DisableHibernate_btn_Click(object sender, EventArgs e) {
         OperatingMethods.SetHibernate(false);
      }

      private void EnableHibernate_btn_Click(object sender, EventArgs e) {
         OperatingMethods.SetHibernate(true);
      }

      private void PageFileOptionDialog_Load(object sender, EventArgs e) {
         LoadUIStrings();
         currentState = OperatingMethods.SwapfileMethods.GetSwapfileState();
         Swapfileinfo_tb.Text = currentState.GetStateDescription();

         Pagefilepartition_lb_SelectedIndexChanged(null, null);
         foreach (DriveInfo driveInfo in Wrapper.getDrives()) {
            try {
               if (driveInfo.AvailableFreeSpace >= 16 * 1048576) {
                  Swapfilepartition_lb.Items.Add(OperatingMethods.GetDriveInfoDescription(driveInfo));
               }
            }
            catch (IOException) { }
         }

         Session.Singleton.FillWithDriveInfo(Swapfilepartition_lb);
         Session.Singleton.FillWithDriveInfo(Pagefilepartition_lb);
      }

      private void LoadUIStrings() {
         ApplyPagefileChanges_btn.Text = ApplyPagefileChanges_btn_Text;
         DisableHibernate_btn.Text = DisableHibernate_btn_Text;
         EnableHibernate_btn.Text = Enablehibernate_btn_Text;
         ExtendedPagefileOptions_btn.Text = ExtendedPagefileOptions_btn_Text;
         HiberfilSettings_gb.Text = HiberfilSettings_gb_Text;
         MaximumPagefileSize_lbl.Text = MaximumPagefileSize_lbl_Text;
         MinimumPagefileSize_lbl.Text = MinimumPagefileSize_lbl_Text;
         PagefileDrive_lbl.Text = PagefileDrive_lbl_Text;
         PagefileSettings_gb.Text = PagefileSettings_gb_Text;
         RefreshAvailableParitions_btn.Text = RefreshAvailableParitions_btn_Text;
         SwapfileSettings_gb.Text = SwapfileSettings_gb_Text;
         SwapfileStepBackward_btn.Text = SwapfileStepBackward_btn_Text;
         SwapfileStepForward_btn.Text = SwapfileStepForward_btn_Text;
         Text = WindowTitle;
      }

      private void SwapfileStepBackward_btn_Click(object sender, EventArgs e) {
         if (OperatingMethods.SwapfileMethods.ChangeSwapfileStadium(false, currentState) &&
             MessageBox.Show(
                SwapfileSuccessful_Restart_Text,
                SwapfileSuccessful_Restart_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
            Wrapper.RestartComputer();
         }
      }

      private void SwapfileStepForward_btn_Click(object sender, EventArgs e) {
         OperatingMethods.GetDriveInfoFromDescription(out DriveInfo info, (string) Swapfilepartition_lb.SelectedItem);

         if (OperatingMethods.SwapfileMethods.ChangeSwapfileStadium(true, currentState, info) &&
             MessageBox.Show(
                SwapfileSuccessful_Restart_Text,
                SwapfileSuccessful_Restart_Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
            Wrapper.RestartComputer();
         }
      }

      private void RefreshAvailablePartitions_btn_Click(object sender, EventArgs e) {
         Session.Singleton.FillWithDriveInfo(Swapfilepartition_lb);
         Session.Singleton.FillWithDriveInfo(Pagefilepartition_lb);
      }


      private void ExtendedPagefileOptions_btn_Click(object sender, EventArgs e) {
         Wrapper.ExecuteExecuteable(
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "SystemPropertiesPerformance.exe"), "",
            true);
         Thread.Sleep(500);
         MessageBox.Show(
            ExtendedPagefileOptions_Text,
            ExtendedPagefileOptions_Title, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
      }

      private void ApplyPagefileChanges_btn_Click(object sender, EventArgs e) {
         int minSize = decimal.ToInt32(MinimumPagefileSize_nud.Value);
         int maxSize = decimal.ToInt32(MaximumPagefilesize_nud.Value);
         int selectedPartitionIndex = Pagefilepartition_lb.SelectedIndex;
         string currentSelection = (string) Pagefilepartition_lb.Items[selectedPartitionIndex];
         OperatingMethods.ChangePagefileSettings(currentSelection, maxSize, minSize);
      }

      private void Pagefilepartition_lb_SelectedIndexChanged(object sender, EventArgs e) {
         ApplyPagefileChanges_btn.Enabled = Pagefilepartition_lb.SelectedIndex != -1;
      }
   }
}