using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UITest.Services;

namespace UITest.Controls
{
    public partial class CalendarClockControl : UserControl
    {
        public Func<byte[], bool>    SendAction    { get; set; }
        /// <summary>讀取 MCU 時鐘：傳入空封包，回傳接收到的 byte[]（null = 失敗）</summary>
        public Func<byte[], byte[]>  ReadAction    { get; set; }

        private readonly Timer _clockTimer = new Timer { Interval = 1000 };

        public CalendarClockControl()
        {
            InitializeComponent();
            if (DesignMode) return;

            _clockTimer.Tick += (s, e) => RefreshCurrentTime();
            _clockTimer.Start();
            RefreshCurrentTime();
        }

        // ── 即時更新現在時間 ──────────────────────────────────────────────

        private void RefreshCurrentTime()
        {
            var now = DateTime.Now;
            string dow = new[] { "星期日","星期一","星期二","星期三","星期四","星期五","星期六" }[(int)now.DayOfWeek];
            lblCurrentTime.Text = $"現在時間：{now:yyyy/M/d tt hh:mm:ss}, {dow}";
        }

        // ── 傳送（設定 MCU 時鐘）─────────────────────────────────────────

        private void btnSend_Click(object sender, EventArgs e)
        {
            var mon = ConnectionMonitor.Instance;

            if (SendAction == null)
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "Calendar", "傳送失敗：未設定 SendAction");
                MessageBox.Show("尚未連接串列埠。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] packet = BuildSetPacket();
            mon.Log(ConnectionMonitor.LogLevel.Send, "Calendar",
                $"設定MCU時鐘 → {DateTime.Now:yyyy/MM/dd HH:mm:ss}  HEX: {PacketBuilderService.ToHex(packet)}");

            if (SendAction(packet))
                mon.Log(ConnectionMonitor.LogLevel.Recv, "Calendar", "MCU時鐘設定成功");
            else
            {
                mon.Log(ConnectionMonitor.LogLevel.Error, "Calendar", "傳送失敗");
                MessageBox.Show("傳送失敗：串列埠未開啟。", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── 讀取 MCU 時鐘 ─────────────────────────────────────────────────

        private void btnRead_Click(object sender, EventArgs e)
        {
            var mon = ConnectionMonitor.Instance;

            if (ReadAction == null)
            {
                // 無 ReadAction 時顯示目前系統時間作為示意
                lblMCUTime.Text = $"MCU System Clock（{DateTime.Now:yyyy/M/d HH:mm:ss}）";
                mon.Log(ConnectionMonitor.LogLevel.Warn, "Calendar",
                    "ReadAction 未設定，顯示系統時間");
                return;
            }

            byte[] query  = BuildGetPacket();
            byte[] result = ReadAction(query);

            if (result != null && result.Length >= 6)
            {
                try
                {
                    // 假設回傳格式：YY MM DD HH mm ss（BCD 或直接 byte）
                    int y = 2000 + result[0], mo = result[1], d  = result[2];
                    int h = result[3],        mi = result[4], s  = result[5];
                    lblMCUTime.Text = $"MCU：{y}/{mo:D2}/{d:D2} {h:D2}:{mi:D2}:{s:D2}";
                    mon.Log(ConnectionMonitor.LogLevel.Recv, "Calendar",
                        $"讀取MCU時鐘：{lblMCUTime.Text}  HEX:{PacketBuilderService.ToHex(result)}");
                }
                catch
                {
                    lblMCUTime.Text = $"MCU System Clock（raw：{PacketBuilderService.ToHex(result)}）";
                }
            }
            else
            {
                lblMCUTime.Text = "MCU System Clock（無回應）";
                mon.Log(ConnectionMonitor.LogLevel.Error, "Calendar", "讀取MCU時鐘失敗");
            }
        }

        // ── 封包組建 ──────────────────────────────────────────────────────

        /// <summary>設定時鐘：功能碼 0x33，data = YY MM DD HH mm ss DoW</summary>
        private static byte[] BuildSetPacket()
        {
            var now = DateTime.Now;
            byte[] data = new byte[]
            {
                (byte)(now.Year - 2000),
                (byte)now.Month,
                (byte)now.Day,
                (byte)now.Hour,
                (byte)now.Minute,
                (byte)now.Second,
                (byte)now.DayOfWeek
            };
            return WrapPacket(0x33, data);
        }

        /// <summary>讀取時鐘：功能碼 0x33，data = 讀取命令</summary>
        private static byte[] BuildGetPacket()
            => WrapPacket(0x33, new byte[] { 0x00 });

        private static byte[] WrapPacket(byte funcCode, byte[] data)
        {
            byte[] len = System.BitConverter.GetBytes((ushort)data.Length);
            byte   cs  = 0;
            foreach (var b in data) cs = (byte)((cs + b) & 0xFF);

            var r = new List<byte>();
            r.AddRange(new byte[] { 0x55, 0xAA });
            r.Add(0x00);
            r.Add(funcCode);
            r.AddRange(len);
            r.AddRange(data);
            r.Add(cs);
            return r.ToArray();
        }
    }
}
