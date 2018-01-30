using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace StorageManagementTool
{
    public partial class PageFileOptionDialog : Form
    {
        public PageFileOptionDialog()
        {
            InitializeComponent();
        }

        private void DisableHibernate_btn_Click(object sender, EventArgs e)
        {
            Wrapper.ExecuteExecuteable(Path.Combine(Wrapper.System32Path,"powercfg.exe"), "/h off", true, true,
                true);
        }

        private void Enablehibernate_btn_Click(object sender, EventArgs e)
        {
            Wrapper.ExecuteExecuteable(Path.Combine(Wrapper.System32Path,"powercfg.exe"), "/h on", true, true,
                true);
        }

        private void PageFileOptionDialog_Load(object sender, EventArgs e)
        {
            //Swapfileinfo_tb.Lines= new string[2];
            const string hint =
                "\r\nUm eine schon verschobene Swapfile auf ein weiteres Laufwerk zu verschieben muss erst zum Stadium 2 zurückgekehrt werden um sie dann auf eine andere Partition auszulagern";
            switch (Session.Singleton.Swapstadium)
            {
                case 1:
                    Swapfileinfo_tb.Text = "Die Registry muss zum verschieben zuerst geändert werden (Stadium 1/4)" + hint;
                    break;
                case 2:
                    Swapfileinfo_tb.Text = "Es muss eine Verknüpfung zur Swapfile erstellt werden (Stadium 2/4)" + hint;
                    break;
                case 3:
                    Swapfileinfo_tb.Text = "Die Swapfile muss wiederhergestellt werden (Stadium 3/4)" + hint;
                    break;
                case 4:
                    Swapfileinfo_tb.Text = "Die Swapfile wurde verschoben (Stadium 4/4)" + hint;
                    break;
            }

            Pagefilepartition_lb_SelectedIndexChanged(null, null);
            foreach (DriveInfo driveInfo in Session.Singleton.CurrentDrives)
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

        private void SwapfileStepBack_btn_Click(object sender, EventArgs e)
        {
            if (Session.Singleton.Isadmin)
            {
                if (OperatingMethods.ChangeSwapfileStadium(Session.Singleton.Swapstadium, false))
                {
                    ProgramStatusStrip.Text =
                        "Der nächste Schritt des Wiederherstellens des Speicherortes der Swapfile war erfolgreich.";
                    if (
                        MessageBox.Show(
                            "Der Computer muss neugestartet werden um die Änderungen zu übernehmen und um weitere Schritte auszuführen. Jetzt neustarten?",
                            "Neustarten?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Wrapper.ExecuteExecuteable(Path.Combine(Wrapper.System32Path,"shutdown.exe"), " /r /t 1",
                            false, false, false);
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
                    "Administratoren-Privilegien", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void SwapfileStepForward_btn_Click(object sender, EventArgs e)
        {
            if (Session.Singleton.Isadmin)
            {
                if (OperatingMethods.ChangeSwapfileStadium(Session.Singleton.Swapstadium, true))
                {
                    ProgramStatusStrip.Text =
                        "Der Versuch den nächsten Schritt beim verschieben des Speicherortes der Swapfile war erfolgreich";
                    if (
                        MessageBox.Show(
                            "Der Computer muss neugestartet werden um die Änderungen zu übernehmen und um weitere Schritte auszuführen. Jetzt neustarten?",
                            "Neustarten?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Wrapper.ExecuteExecuteable(Path.Combine(Wrapper.System32Path,"shutdown.exe"), " /r /t 1",
                            false, false, false);
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
                    "Administratoren-Privilegien", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void RefreshAvailablePartitions_btn_Click(object sender, EventArgs e)
        {
            Session.Singleton.RefreshDriveInformation();
            Session.Singleton.FillWithDriveInfo(Swapfilepartition_lb);
            Session.Singleton.FillWithDriveInfo(Pagefilepartition_lb);
        }



        private void ExtendedPagefileOptions_btn_Click(object sender, EventArgs e)
        {
            Wrapper.ExecuteExecuteable(Path.Combine(Wrapper.System32Path,"SystemPropertiesPerformance.exe"), "",
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