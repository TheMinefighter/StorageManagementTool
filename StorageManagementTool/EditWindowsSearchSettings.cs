using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace StorageManagementTool
{
   public partial class EditWindowsSearchSettings : Form
   {
      public EditWindowsSearchSettings()
      {
         InitializeComponent();
      }

      private void OpenCurrentPath_btn_Click(object sender, EventArgs e)
      {
         if (Directory.Exists(CurrentPath_tb.Text))
         {
            Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath,
               CurrentPath_tb.Text);
         }
      }

      private void OpenNewPath_btn_Click(object sender, EventArgs e)
      {
         if (Directory.Exists(NewPath_tb.Text))
         {
            Wrapper.ExecuteExecuteable(Wrapper.ExplorerPath,
               NewPath_tb.Text);
         }
      }

      private void SelectNewPath_btn_Click(object sender, EventArgs e)
      {
         SelectNewPath_fbd.ShowDialog();
         NewPath_tb.Text = SelectNewPath_fbd.SelectedPath;
      }

      private void ShowFurtherSettings_btn_Click(object sender, EventArgs e)
      {
         {
            Wrapper.ExecuteExecuteable(Path.Combine(Wrapper.System32Path, @"rundll32.exe"),
               " shell32.dll,Control_RunDLL srchadmin.dll");
         }
      }

      private void SaveSettings_btn_Click(object sender, EventArgs e)
      {
         OperatingMethods.SetSearchDataPath(new DirectoryInfo(NewPath_tb.Text));
      }

      private void Abort_btn_Click(object sender, EventArgs e)
      {
         Close();
      }

      private void CurrentPath_tb_TextChanged(object sender, EventArgs e)
      {
         EnableComponents();
      }

      private void NewPath_tb_TextChanged(object sender, EventArgs e)
      {
         EnableComponents();
      }

      private void EnableComponents()
      {
         OpenNewPath_btn.Enabled = Directory.Exists(NewPath_tb.Text);
         OpenCurrentPath_btn.Enabled = Directory.Exists(CurrentPath_tb.Text);
         SaveSettings_btn.Enabled = Directory.Exists(NewPath_tb.Text);
      }

      private void EditWindowsSearchSettings_Load(object sender, EventArgs e)
      {
         EnableComponents();
         RefreshCurrentPath();
      }

      private void RefreshCurrentPath()
      {
         string displayedSearchDataPath = "Error";
         if (Wrapper.GetRegistryValue(OperatingMethods.SearchDatatDirectoryRegPath, out object text, false))
         {

            try
            {
               DirectoryInfo readPath = new DirectoryInfo((string)text);
               displayedSearchDataPath = readPath.Parent.Parent.FullName;
            }
            catch (Exception)
            {
               MessageBox.Show("Der Speicherort für Suchindizierungen ist nicht korrekt definiert.", "Fehler",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
            }


         }
         CurrentPath_tb.Text = displayedSearchDataPath;
      }

      private void RefreshCurrentPath_btn_Click(object sender, EventArgs e)
      {
         RefreshCurrentPath();
      }
   }
}