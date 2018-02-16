using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static StorageManagementTool.MainGUI.GlobalizationRessources.MonitoringSettingsStrings;
namespace StorageManagementTool.MainGUI
{
   public partial class MonitoringSettings : Form
   {
      private readonly Dictionary<JSONConfig.MonitoringSetting.MonitoringAction, RadioButton> _forFilesDictionary =
         new Dictionary<JSONConfig.MonitoringSetting.MonitoringAction, RadioButton>();


      private readonly Dictionary<JSONConfig.MonitoringSetting.MonitoringAction, RadioButton> _forFoldersDictionary =
         new Dictionary<JSONConfig.MonitoringSetting.MonitoringAction, RadioButton>();

      private JSONConfig.MonitoringSetting _editedSettings = new JSONConfig.MonitoringSetting();
      private List<Control> _whenEnabled = new List<Control>();
      private List<Control> _whenSelected = new List<Control>();
      private bool IsMonitored;
      public MonitoringSettings()
      {
         InitializeComponent();
      }

      private void EnableNotifications_cb_CheckedChanged(object sender, EventArgs e)
      {
         EnableControls();
      }
      /// <summary>
      /// Loads UI strings from culture sepcific ressource file
      /// </summary>
      private void LoadUIStrings()
      {
         this.Text = WindowTitle;
         EnableNotifications_cb.Text = EnableNotifications_cb_Text;
         //InitalizeSSDMonitoring_btn.Text = InitalizeSSDMonitoring_btn_Text;
         OpenSelectedfolder_btn.Text = OpenSelectedfolder_btn_Text;
         ChangeFolder_btn.Text = ChangeFolder_btn_Text;
         AddFolder_btn.Text = AddFolder_btn_Text;
         RemoveSelectedFolder_btn.Text = RemoveSelectedFolder_btn_Text;
         ActionForFiles_gb.Text = ActionForFiles_gb_Text;
         ActionForFolders_gb.Text = ActionForFolders_gb_Text;
         Abort_btn.Text = Abort_btn_Text;
         SaveSettings_btn.Text = SaveSettings_btn_Text;
         AskActionForFolders_rb.Text = AskForAction_Text;
         IgnoreForFolders_rb.Text = Ignore_Text;
         AutomaticMoveForFolders_rb.Text = AutomaticMove_Text;
         AskActionForFiles_rb.Text = AskForAction_Text;
         IgnoreForFiles_rb.Text = Ignore_Text;
         AutomaticMoveForFiles_rb.Text = AutomaticMove_Text;
      }
      private void NotificationSettings_Load(object sender, EventArgs e)
      {


         LoadUIStrings();
         _forFoldersDictionary.Add(JSONConfig.MonitoringSetting.MonitoringAction.Ask, AskActionForFolders_rb);
         _forFoldersDictionary.Add(JSONConfig.MonitoringSetting.MonitoringAction.Ignore, IgnoreForFolders_rb);
         _forFoldersDictionary.Add(JSONConfig.MonitoringSetting.MonitoringAction.Move, AutomaticMoveForFolders_rb);
         _forFilesDictionary.Add(JSONConfig.MonitoringSetting.MonitoringAction.Ask, AskActionForFiles_rb);
         _forFilesDictionary.Add(JSONConfig.MonitoringSetting.MonitoringAction.Ignore, IgnoreForFiles_rb);
         _forFilesDictionary.Add(JSONConfig.MonitoringSetting.MonitoringAction.Move, AutomaticMoveForFiles_rb);
         _editedSettings = Session.Singleton.CurrentConfiguration.MonitoringSettings ?? new JSONConfig.MonitoringSetting();
         _whenEnabled = new List<Control>
         {
            AllFolders_lb,
            AddFolder_btn,
            OpenSelectedfolder_btn,
            RemoveSelectedFolder_btn,
            ActionForFiles_gb,
            ActionForFolders_gb,
            ChangeFolder_btn
         };
         _whenSelected = new List<Control>
         {
            OpenSelectedfolder_btn,
            RemoveSelectedFolder_btn,
            ActionForFiles_gb,
            ActionForFolders_gb,
            ChangeFolder_btn
         };
         EnableControls();
         AllFolders_lb.Items.Clear();
         AllFolders_lb.Items.AddRange(
            _editedSettings.MonitoredFolders.Select(x => x.TargetPath).Cast<object>().ToArray());
         IsMonitored = OperatingMethods.SSDMonitoring.SSDMonitoringEnabled();
         EnableNotifications_cb.Checked = IsMonitored;
      }

      private void EnableControls()
      {
         bool itemSelected = AllFolders_lb.SelectedIndex != -1;

         //To SCHTASK /TN /DiSABLE
         bool monitoringEnabled = EnableNotifications_cb.Checked;
         foreach (Control control in _whenEnabled)
         {
            if (monitoringEnabled)
            {
               if (itemSelected)
               {
                  control.Enabled = true;
               }
               else
               {
                  control.Enabled = !_whenSelected.Contains(control);
               }
            }
            else
            {
               control.Enabled = false;
            }
         }
      }

      private void OpenSelectedfolder_btn_Click(object sender, EventArgs e)
      {
         Wrapper.ExecuteExecuteable(
            Wrapper.ExplorerPath,
            (string)AllFolders_lb.SelectedItem, false, false, false);
      }

      private void RemoveSelectedFolder_btn_Click(object sender, EventArgs e)
      {
         _editedSettings.MonitoredFolders.RemoveAt(AllFolders_lb.SelectedIndex);
         AllFolders_lb.Items.RemoveAt(AllFolders_lb.SelectedIndex);
      }

      private void AddFolder_btn_Click(object sender, EventArgs e)
      {
         FolderBrowserDialog browser =
            new FolderBrowserDialog { Description = AddFolder_fbdDescription };
         browser.ShowDialog();
         _editedSettings.MonitoredFolders.Add(new JSONConfig.MonitoringSetting.MonitoredFolder(browser.SelectedPath));
         AllFolders_lb.Items.Add(browser.SelectedPath);
         AllFolders_lb.SelectedIndex = AllFolders_lb.Items.Count - 1;
         browser.Dispose();
      }

      private void AllFolders_lb_SelectedIndexChanged(object sender, EventArgs e)
      {
         EnableControls();
         if (AllFolders_lb.SelectedIndex != -1)
         {
            _forFoldersDictionary[_editedSettings.MonitoredFolders[AllFolders_lb.SelectedIndex].ForFolders].Checked =
               true;
            _forFilesDictionary[_editedSettings.MonitoredFolders[AllFolders_lb.SelectedIndex].ForFiles].Checked =
               true;
         }
         else
         {
            ActionForFolders_gb.Controls.OfType<RadioButton>().ToList().ForEach(x => x.Checked = false);
            ActionForFiles_gb.Controls.OfType<RadioButton>().ToList().ForEach(x => x.Checked = false);
         }
      }

      private void ChangeFolder_btn_Click(object sender, EventArgs e)
      {
         FolderBrowserDialog browser =
            new FolderBrowserDialog { Description = ChangeFolder_fbdDescription };
         browser.ShowDialog();
         _editedSettings.MonitoredFolders[AllFolders_lb.SelectedIndex].TargetPath = browser.SelectedPath;
         AllFolders_lb.Items[AllFolders_lb.SelectedIndex] = browser.SelectedPath;
         browser.Dispose();
      }

      private void SaveSettings_btn_Click(object sender, EventArgs e)
      {
         if (!_editedSettings.Equals(Session.Singleton.CurrentConfiguration.MonitoringSettings))
         {
                     Session.Singleton.CurrentConfiguration.MonitoringSettings = _editedSettings;
         Session.Singleton.SaveCfg();

         }
         if (EnableNotifications_cb.Checked!=IsMonitored)
         {
            if (!OperatingMethods.SSDMonitoring.SetSSDMonitoring(EnableNotifications_cb.Checked))
            {
               return;
               //error
            }
            }
         Close();
      }

      private void Abort_btn_Click(object sender, EventArgs e)
      {
         Close();
      }

      private void ForFoldersChanged(object sender, EventArgs e)
      {
         if (((RadioButton)sender).Checked)
         {
            _editedSettings.MonitoredFolders[AllFolders_lb.SelectedIndex].ForFolders =
               _forFoldersDictionary.FirstOrDefault(x => x.Value == (RadioButton)sender).Key;
         }
      }

      private void ForFilesChanged(object sender, EventArgs e)
      {
         if (((RadioButton)sender).Checked)
         {
            _editedSettings.MonitoredFolders[AllFolders_lb.SelectedIndex].ForFiles =
               _forFilesDictionary.FirstOrDefault(x => x.Value == (RadioButton)sender).Key;
         }
      }

      private void InitalizeSSDMonitoring_btn_Click(object sender, EventArgs e)
      {
         OperatingMethods.SSDMonitoring.InitalizeSSDMonitoring();
      }
   }
}