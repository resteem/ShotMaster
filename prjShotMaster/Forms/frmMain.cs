﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using prjShotMaster.Actions;

namespace prjShotMaster
{
    public partial class frmMain : Form
    {
        public CActionManager actionManager = new CActionManager();

        public frmMain()
        {
            InitializeComponent();
            FillSettingsControls();
            // hook (старт перехвата клавы)
            CInterceptKeys.Hook();
            CInterceptKeys.KeyUp += InterceptKeys_KeyUp;

            // test
            pnlSettingsDefault.Enabled = false;
        }

        private void InterceptKeys_KeyUp(Keys Key)
        {
            // глобальный перехват клавы
            List<Keys> PressedKeys = CInterceptKeys.KeysDown;
            // Ctrl + L + K - в настройках хранить
            if (
                (
                    PressedKeys.Contains(Keys.Control)
                    || PressedKeys.Contains(Keys.LControlKey)
                    || PressedKeys.Contains(Keys.RControlKey)
                )
                && PressedKeys.Contains(Keys.L)
                && PressedKeys.Contains(Keys.K)
            )
            {
                ntfIcn.ShowBalloonTip(100, "prjShotMaster", "Test CInterceptKeys", ToolTipIcon.Info);
            }
        }

        private void FillSettingsControls()
        {
            // lbDeviceIndex
            foreach (System.Collections.DictionaryEntry actionEntry in actionManager.actionList)
            {
                CShotAction action = (actionEntry.Value as CShotAction);
                Control lbDeviceIndex = FindControlByName("lbDeviceIndex" + action.action_code);
                (lbDeviceIndex as ListBox).Items.Clear();
                foreach (string str in action.DeviceList)
                {
                    (lbDeviceIndex as ListBox).Items.Add(str);
                }
            }
            // cbFileFormat
            cbFileFormatDefault.Items.Clear();
            cbFileFormatDefault.Items.Add(CShotActionSettings.IT_EXT_JPG);
            cbFileFormatDefault.Items.Add(CShotActionSettings.IT_EXT_PNG);
            // show current settings values
            FillSettingsControlsState();
        }

        private void FillSettingsControlsState()
        {
            tbDestinationFolderDefault.Text = Properties.Settings.Default.DestinationFolder;
            tbTimerIntervalDefault.Text = Properties.Settings.Default.TimerInterval.ToString();
            cbFileFormatDefault.SelectedIndex = Properties.Settings.Default.FileFormat;
            tbJpegQualityDefault.Text = Properties.Settings.Default.JpegQuality.ToString();
            cbPlaySoundDefault.Checked = Properties.Settings.Default.PlaySound;
            tbSoundLocationDefault.Text = Properties.Settings.Default.SoundLocation;

            pnlJpegQualityDefault.Enabled = (cbFileFormatDefault.SelectedIndex == CShotActionSettings.IT_JPG);
            pnlSoundLocationDefault.Enabled = cbPlaySoundDefault.Checked;
        }

        private Control FindControlByName(string name)
        {
            foreach (Control control in this.Controls.Find(name, true))
            {
                if (control.Name == name)
                {
                    return control;
                }
            }
            return null;
        }
    }
}
