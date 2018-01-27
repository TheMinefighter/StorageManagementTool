﻿using System;
using System.Windows.Forms;
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

            Session.Singleton.FillWithDriveInfo(SelectHDD_lb);
            Session.Singleton.FillWithDriveInfo(SelectSSD_lb);
        }
    }
}