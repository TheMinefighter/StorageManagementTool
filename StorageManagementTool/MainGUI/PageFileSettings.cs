using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using static  StorageManagementTool.MainGUI.GlobalizationRessources.PagefileSettingsStrings;

namespace StorageManagementTool.MainGUI
{
   public partial class PagefileSettings : Form
   {
      
      public PagefileSettings()
      {
         InitializeComponent();
      }

      private void DisableHibernate_btn_Click(object sender, EventArgs e)
      {
         OperatingMethods.SetHibernate(false);
      }

      private void EnableHibernate_btn_Click(object sender, EventArgs e)
      {
         OperatingMethods.SetHibernate(true);
      }

      private OperatingMethods.SwapfileMethods.SwapfileState currentState;
      private void PageFileOptionDialog_Load(object sender, EventArgs e)
      {
         LoadUIStrings();
         currentState = OperatingMethods.SwapfileMethods.GetSwapfileState();
         Swapfileinfo_tb.Text = currentState.GetStateDescription();
        // Session.Singleton.RefreshSwapfileStadium();
         //const string hint =
         //   "\r\nUm eine schon verschobene Swapfile auf ein weiteres Laufwerk zu verschieben muss erst zum Stadium 2 zurückgekehrt werden um sie dann auf eine andere Partition auszulagern";
         //switch (currentState)
         //{
         //   case OperatingMethods.SwapfileMethods.SwapfileState.Standard:
         //      Swapfileinfo_tb.Text = "Die Registry muss zum verschieben zuerst geändert werden (Stadium 1/3)" + hint;
         //      break;
         //   case OperatingMethods.SwapfileMethods.SwapfileState.Disabled:
         //      Swapfileinfo_tb.Text = "Es muss eine Verknüpfung zur Swapfile erstellt werden (Stadium 2/3)" + hint;
         //      break;
         //   case OperatingMethods.SwapfileMethods.SwapfileState.Moved:
         //      Swapfileinfo_tb.Text = "Die Swapfile wurde verschoben (Stadium 3/3)" + hint;
         //      break;
         //   default:
         //      throw new ArgumentOutOfRangeException();
         //}

         Pagefilepartition_lb_SelectedIndexChanged(null, null);
         foreach (DriveInfo driveInfo in FileSystem.Drives)
         {
            try
            {
               if (driveInfo.AvailableFreeSpace >= 16 * 1048576)
               {
                  Swapfilepartition_lb.Items.Add(OperatingMethods.DriveInfo2String(driveInfo));
               }
            }
            catch (IOException)
            {
            }
         }

         Session.Singleton.FillWithDriveInfo(Swapfilepartition_lb);
         Session.Singleton.FillWithDriveInfo(Pagefilepartition_lb);
      }

      private void LoadUIStrings()
      {
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
         this.Text = WindowTitle;
      }

      private void SwapfileStepBackward_btn_Click(object sender, EventArgs e)
      {
         if (Session.Singleton.IsAdmin)
         {
            if (OperatingMethods.SwapfileMethods.ChangeSwapfileStadium(false, currentState))
            {
               ProgramStatusStrip.Text =
                  "Der nächste Schritt des Wiederherstellens des Speicherortes der Swapfile war erfolgreich.";
               if (
                  MessageBox.Show(
                     "Der Computer muss neugestartet werden um die Änderungen zu übernehmen und um weitere Schritte auszuführen. Jetzt neustarten?",
                     "Neustarten?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
               {
                  Wrapper.RestartComputer();
               }
            }
            else
            {
               ProgramStatusStrip.Text =
                  "Der nächste Schritt des Wiederherstellens des Speicherortes der Swapfile war nicht erfolgreich.";
            }
         }
         else
         {
            MessageBox.Show("Für diese Operation muss das Programm mit Administratoren-Privilegien gestartet werden.",
               "Administratoren-Privilegien", MessageBoxButtons.OK, MessageBoxIcon.Error,
               MessageBoxDefaultButton.Button1);
         }
      }

      private void SwapfileStepForward_btn_Click(object sender, EventArgs e)
      {
         if (Session.Singleton.IsAdmin)
         {
            if (OperatingMethods.SwapfileMethods.ChangeSwapfileStadium(true, currentState))
            {
               ProgramStatusStrip.Text =
                  "Der Versuch den nächsten Schritt beim verschieben des Speicherortes der Swapfile war erfolgreich";
               if (
                  MessageBox.Show(
                     "Der Computer muss neugestartet werden um die Änderungen zu übernehmen und um weitere Schritte auszuführen. Jetzt neustarten?",
                     "Neustarten?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
               {
                  Wrapper.RestartComputer();
               }
            }
            else
            {
               ProgramStatusStrip.Text =
                  "Bei dem Versuch den nächsten Schritt beim verschieben des Speicherortes der Swapfile durchzuführen ist ein Fehler aufgetaucht";
            }
         }
         else
         {
            MessageBox.Show("Für diese Operation muss das Programm mit Administratoren-Privilegien gestartet werden.",
               "Administratoren-Privilegien", MessageBoxButtons.OK, MessageBoxIcon.Error,
               MessageBoxDefaultButton.Button1);
         }
      }

      private void RefreshAvailablePartitions_btn_Click(object sender, EventArgs e)
      {
         Session.Singleton.FillWithDriveInfo(Swapfilepartition_lb);
         Session.Singleton.FillWithDriveInfo(Pagefilepartition_lb);
      }


      private void ExtendedPagefileOptions_btn_Click(object sender, EventArgs e)
      {
         Wrapper.ExecuteExecuteable(Path.Combine(Wrapper.System32Path, "SystemPropertiesPerformance.exe"), "",
            true);
         Thread.Sleep(500);
         MessageBox.Show(
            "Öffne den Tab \"Erweitert\" und klicke dort auf \"Ändern...\" um die Weiteren Optionen für Arbeitsspeicherauslagerungas Dateien zu öffnen ",
            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
      }

      private void ApplyPagefileChanges_btn_Click(object sender, EventArgs e)
      {
         int minSize = decimal.ToInt32(MinimumPagefileSize_nud.Value);
         int maxSize = decimal.ToInt32(MaximumPagefilesize_nud.Value);
         int selectedPartitionIndex = Pagefilepartition_lb.SelectedIndex;
         string currentSelection = (string) Pagefilepartition_lb.Items[selectedPartitionIndex];
         OperatingMethods.ChangePagefileSettings(currentSelection, maxSize, minSize);
      }

      private void Pagefilepartition_lb_SelectedIndexChanged(object sender, EventArgs e)
      {
         ApplyPagefileChanges_btn.Enabled = Pagefilepartition_lb.SelectedIndex != -1;
      }
   }
}