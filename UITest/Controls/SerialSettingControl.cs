using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace UITest.Controls
{
    public partial class SerialSettingControl : UserControl
    {
        private SerialPort _port;
        private bool _isOpen = false;

        public SerialSettingControl()
        {
            InitializeComponent();
            if (!DesignMode)
                InitComboBoxes();
        }

        private void InitComboBoxes()
        {
            // COM ports
            foreach (string p in SerialPort.GetPortNames())
                cmbCOM.Items.Add(p);
            if (cmbCOM.Items.Count > 0) cmbCOM.SelectedIndex = 0;

            // BaudRate
            foreach (int b in new[] { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 })
                cmbBaudRate.Items.Add(b);
            cmbBaudRate.SelectedItem = 38400;

            // Databit
            foreach (int d in new[] { 5, 6, 7, 8 })
                cmbDatabit.Items.Add(d);
            cmbDatabit.SelectedItem = 8;

            // Parity
            foreach (string v in Enum.GetNames(typeof(Parity)))
                cmbParity.Items.Add(v);
            cmbParity.SelectedItem = "None";

            // Stopbit
            foreach (string v in Enum.GetNames(typeof(StopBits)))
                cmbStopbit.Items.Add(v);
            cmbStopbit.SelectedItem = "One";
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (!_isOpen)
            {
                OpenPort();
            }
            else
            {
                ClosePort();
            }
        }

        private void OpenPort()
        {
            try
            {
                if (cmbCOM.SelectedItem == null)
                {
                    MessageBox.Show("請選擇 COM Port", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _port = new SerialPort
                {
                    PortName = cmbCOM.SelectedItem.ToString(),
                    BaudRate = (int)cmbBaudRate.SelectedItem,
                    DataBits = (int)cmbDatabit.SelectedItem,
                    Parity   = (Parity)Enum.Parse(typeof(Parity), cmbParity.SelectedItem.ToString()),
                    StopBits = (StopBits)Enum.Parse(typeof(StopBits), cmbStopbit.SelectedItem.ToString())
                };
                _port.Open();
                _isOpen = true;
                btnOpen.Text = "關閉";
                SetSerialControls(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"開啟失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClosePort()
        {
            try
            {
                if (_port != null && _port.IsOpen)
                {
                    _port.Close();
                    _port.Dispose();
                    _port = null;
                }
                _isOpen = false;
                btnOpen.Text = "開啟";
                SetSerialControls(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"關閉失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetSerialControls(bool enabled)
        {
            cmbCOM.Enabled      = enabled;
            cmbBaudRate.Enabled = enabled;
            cmbDatabit.Enabled  = enabled;
            cmbParity.Enabled   = enabled;
            cmbStopbit.Enabled  = enabled;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int delay   = (int)nudDelay.Value;
            int maxRows = (int)nudMaxRows.Value;
            MessageBox.Show(
                $"已儲存\n延遲：{delay} ms\n最多列數：{maxRows}",
                "儲存成功",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// 透過目前開啟的串列埠傳送 byte[]。
        /// 回傳 true=成功，false=未開啟或失敗。
        /// </summary>
        public bool SendData(byte[] data)
        {
            if (_port == null || !_port.IsOpen) return false;
            try { _port.Write(data, 0, data.Length); return true; }
            catch { return false; }
        }

        /// <summary>取得目前延遲設定 (ms)</summary>
        public int PacketDelay => (int)nudDelay.Value;

        /// <summary>取得目前最多顯示列數</summary>
        public int MaxDisplayRows => (int)nudMaxRows.Value;

        // ── 大廳層 All ID ────────────────────────────────────
        private bool _updatingLobby = false;

        private void chkAllLobby_CheckedChanged(object sender, EventArgs e)
        {
            if (_updatingLobby) return;
            _updatingLobby = true;
            bool v = chkAllLobby.Checked;
            foreach (var chk in new[] { chkLobby_ID1, chkLobby_ID2, chkLobby_ID3, chkLobby_ID4,
                                        chkLobby_ID5, chkLobby_ID6, chkLobby_ID7 })
                chk.Checked = v;
            _updatingLobby = false;
        }

        private void LobbyItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updatingLobby) return;
            _updatingLobby = true;
            bool allChecked = chkLobby_ID1.Checked && chkLobby_ID2.Checked && chkLobby_ID3.Checked
                           && chkLobby_ID4.Checked && chkLobby_ID5.Checked && chkLobby_ID6.Checked
                           && chkLobby_ID7.Checked;
            chkAllLobby.Checked = allChecked;
            _updatingLobby = false;
        }

        // ── 月臺層 All ID ────────────────────────────────────
        private bool _updatingPlatform = false;

        private void chkAllPlatform_CheckedChanged(object sender, EventArgs e)
        {
            if (_updatingPlatform) return;
            _updatingPlatform = true;
            bool v = chkAllPlatform.Checked;
            foreach (var chk in new[] { chkPlatform_ID11, chkPlatform_ID12, chkPlatform_ID13, chkPlatform_ID14,
                                        chkPlatform_ID15, chkPlatform_ID16, chkPlatform_ID17, chkPlatform_ID18 })
                chk.Checked = v;
            _updatingPlatform = false;
        }

        private void PlatformItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_updatingPlatform) return;
            _updatingPlatform = true;
            bool allChecked = chkPlatform_ID11.Checked && chkPlatform_ID12.Checked
                           && chkPlatform_ID13.Checked && chkPlatform_ID14.Checked
                           && chkPlatform_ID15.Checked && chkPlatform_ID16.Checked
                           && chkPlatform_ID17.Checked && chkPlatform_ID18.Checked;
            chkAllPlatform.Checked = allChecked;
            _updatingPlatform = false;
        }
    }
}
