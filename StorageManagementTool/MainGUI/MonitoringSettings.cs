using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using System.Xml.Linq;
using static StorageManagementTool.MainGUI.GlobalizationRessources.MonitoringSettingsStrings;
namespace StorageManagementTool.MainGUI
{
   public partial class MonitoringSettings : Form
   {
      private static readonly XNamespace TaskNamespace =
         XNamespace.Get("http://schemas.microsoft.com/windows/2004/02/mit/task");

      private readonly Dictionary<JSONConfig.MonitoringSetting.MonitoringAction, RadioButton> _forFilesDictionary =
         new Dictionary<JSONConfig.MonitoringSetting.MonitoringAction, RadioButton>();


      private readonly Dictionary<JSONConfig.MonitoringSetting.MonitoringAction, RadioButton> _forFoldersDictionary =
         new Dictionary<JSONConfig.MonitoringSetting.MonitoringAction, RadioButton>();

      private JSONConfig.MonitoringSetting _editedSettings = new JSONConfig.MonitoringSetting();
      private List<Control> _whenEnabled = new List<Control>();
      private List<Control> _whenSelected = new List<Control>();

      public MonitoringSettings()
      {
         InitializeComponent();
      }

      private void EnableNotifications_cb_CheckedChanged(object sender, EventArgs e)
      {
         EnableControls();
         _editedSettings.SSDMonitoringEnabled = EnableNotifications_cb.Checked;
      }
      /// <summary>
      /// Loads UI strings from culture sepcific ressource file
      /// </summary>
      private void LoadUIStrings()
      {
         this.Text = WindowTitle;
         EnableNotifications_cb.Text = EnableNotifications_cb_Text;
         InitalizeSSDMonitoring_btn.Text = InitalizeSSDMonitoring_btn_Text;
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
         EnableNotifications_cb.Checked = _editedSettings.SSDMonitoringEnabled;
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
            new FolderBrowserDialog { Description = "Wählen sie den zu überwachenden Ordner aus" };
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
            new FolderBrowserDialog { Description = "Wählen sie den zu überwachenden Ordner aus" };
         browser.ShowDialog();
         _editedSettings.MonitoredFolders[AllFolders_lb.SelectedIndex].TargetPath = browser.SelectedPath;
         AllFolders_lb.Items[AllFolders_lb.SelectedIndex] = browser.SelectedPath;
         browser.Dispose();
      }

      private void SaveSettings_btn_Click(object sender, EventArgs e)
      {
         Session.Singleton.CurrentConfiguration.MonitoringSettings = _editedSettings;
         Session.Singleton.SaveCfg();
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
         string task = new XDocument(new XDeclaration("1.0", "UTF-16", null),
            new XElement(TaskNamespace + "Task", new XAttribute("version", "1.4"),
               new XElement(TaskNamespace + "RegistrationInfo",
                  new XElement(TaskNamespace + "Date", DateTime.Now.ToWin32Format()),
                  new XElement(TaskNamespace + "Author", WindowsIdentity.GetCurrent().Name),
                  new XElement(TaskNamespace + "Description",
                     "Monitors a list of configured paths"),
                  new XElement(TaskNamespace + "URI", "\\SSDMonitoring")),
               new XElement(TaskNamespace + "Triggers",
                  new XElement(TaskNamespace + "LogonTrigger", new XElement(TaskNamespace + "Enabled", "true"))),
               new XElement(TaskNamespace + "Principals",
                  new XElement(TaskNamespace + "Principal", new XAttribute("ID", "Author"),
                     new XElement(TaskNamespace + "GroupId", "S-1-5-32-545"),
                     new XElement(TaskNamespace + "RunLevel", "HighestAvailable"))),
               new XElement(TaskNamespace + "Settings",
                  new XElement(TaskNamespace + "MultipleInstancesPolicy", "Parallel"),
                  new XElement(TaskNamespace + "DisallowStartIfOnBatteries", "false"),
                  new XElement(TaskNamespace + "StopIfGoingOnBatteries", "false"),
                  new XElement(TaskNamespace + "AllowHardTerminate", "false"),
                  new XElement(TaskNamespace + "StartWhenAvailable", "false"),
                  new XElement(TaskNamespace + "RunOnlyIfNetworkAvailable", "false"),
                  new XElement(TaskNamespace + "IdleSettings", new XElement(TaskNamespace + "StopOnIdleEnd", "true"),
                     new XElement(TaskNamespace + "RestartOnIdle", "false")),
                  new XElement(TaskNamespace + "AllowStartOnDemand", "true"),
                  new XElement(TaskNamespace + "Enabled", "true"), new XElement(TaskNamespace + "Hidden", "false"),
                  new XElement(TaskNamespace + "RunOnlyIfIdle", "false"),
                  new XElement(TaskNamespace + "DisallowStartOnRemoteAppSession", "false"),
                  new XElement(TaskNamespace + "UseUnifiedSchedulingEngine", "true"),
                  new XElement(TaskNamespace + "WakeToRun", "false"),
                  new XElement(TaskNamespace + "ExecutionTimeLimit", "PT0S"),
                  new XElement(TaskNamespace + "Priority", "7")),
               new XElement(TaskNamespace + "Actions", new XAttribute("Context", "Author"),
                  new XElement(TaskNamespace + "Exec",
                     new XElement(TaskNamespace + "Command", Process.GetCurrentProcess().MainModule.FileName),
                     new XElement(TaskNamespace + "Arguments", "/background"))))).ToString();

         File.WriteAllText(Path.Combine(Path.GetTempPath(), "StorageManagementTool.Task.xml"), task);
         Wrapper.ExecuteCommand(
            $"SCHTASKS /Create /XML \"{Path.Combine(Path.GetTempPath(), "StorageManagementTool.Task.xml")}\" /TN SSDMonitoring /RP * /RU {Environment.UserName}",
            true, false, true, false);
      }
   }
}