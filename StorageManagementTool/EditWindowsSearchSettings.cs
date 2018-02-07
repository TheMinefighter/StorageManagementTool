using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace StorageManagementTool
{
   public partial class EditWindowsSearchSettings : Form
   {
      public readonly RegPath SearchDatatDirectoryRegPath = new RegPath(
         @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search", "DataDirectory");

      //@"HKEY_LOCAL_MACHINE\SOFTWARE\TBP", "Test");
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
         if (Directory.Exists(NewPath_tb.Text))
         {
            if (Wrapper.SetRegistryValue(SearchDatatDirectoryRegPath,
               new DirectoryInfo(NewPath_tb.Text).CreateSubdirectory("Search").CreateSubdirectory("Data").FullName,
               RegistryValueKind.String,
               true))
            {
               if (MessageBox.Show(
                      "Um die Änderungen zu anzuwenden muss der Computer neugestartet werden. Jetzt neustarten?",
                      "Neustarten?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
               {
                  Wrapper.ExecuteExecuteable(
                     Environment.ExpandEnvironmentVariables(Path.Combine(Wrapper.System32Path,
                        @"shutdown.exe")), " /R /T 1", false,
                     true);
               }

               Close();
            }
         }
         else
         {
            MessageBox.Show("Der angegebene Ordner existiert nicht", "Fehler", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
         }
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
         if (Wrapper.GetRegistryValue(SearchDatatDirectoryRegPath, out object text,false))
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