using ASI.Wanda.DMD.DB.Tables.DMD;
using System;
using System.Windows.Forms;

namespace UITest
{
    /// <summary>
    /// 消息发送测试功能 - 简化版本（修正版）
    /// 使用最少的输入字段，便于快速测试
    /// </summary>
    public partial class Form1
    {
        #region Simple Message Sender

        /// <summary>
        /// 简化版发送测试消息 - 使用固定的默认值
        /// </summary>
        private void btnSimpleSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                txtSimpleResult.Text = "===== 开始发送测试消息 =====\r\n";
                txtSimpleResult.Text += $"时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n\r\n";
                Application.DoEvents();

                // 1. 获取输入参数（简化版只需要3个输入）
                string messageText = txtSimpleMessageText.Text.Trim();
                string targetDevice = txtSimpleTargetDevice.Text.Trim();
                bool isInstantMessage = chkSimpleInstantMessage.Checked;

                // 验证
                if (string.IsNullOrEmpty(messageText))
                {
                    MessageBox.Show("请输入消息内容", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(targetDevice))
                {
                    MessageBox.Show("请输入目标设备", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. 生成 GUID
                Guid messageId = Guid.NewGuid();
                Guid playlistId = Guid.NewGuid();

                txtSimpleResult.Text += $"[1/4] 生成消息 ID\r\n";
                txtSimpleResult.Text += $"      消息 ID: {messageId}\r\n";
                txtSimpleResult.Text += $"      播放列表 ID: {playlistId}\r\n\r\n";
                Application.DoEvents();

                // 3. 解析目标设备
                string[] parts = targetDevice.Split('_');
                if (parts.Length != 3)
                {
                    txtSimpleResult.Text += "✗ 错误: 目标设备格式错误\r\n";
                    txtSimpleResult.Text += "  正确格式: StationID_AreaID_DeviceID\r\n";
                    txtSimpleResult.Text += "  例如: LG01_CCS_CDU-1\r\n";
                    return;
                }

                string stationId = parts[0];
                string areaId = parts[1];
                string deviceId = parts[2];

                txtSimpleResult.Text += $"[2/4] 解析目标设备\r\n";
                txtSimpleResult.Text += $"      车站: {stationId}\r\n";
                txtSimpleResult.Text += $"      区域: {areaId}\r\n";
                txtSimpleResult.Text += $"      设备: {deviceId}\r\n\r\n";
                Application.DoEvents();

                // 4. 插入消息到数据库（使用默认参数）
                txtSimpleResult.Text += $"[3/4] 插入消息到数据库\r\n";
                InsertSimpleMessageToDatabase(messageId, messageText, isInstantMessage);
                txtSimpleResult.Text += "      ✓ 消息插入成功\r\n\r\n";
                Application.DoEvents();

                // 5. 更新播放列表
                txtSimpleResult.Text += $"[4/4] 更新播放列表\r\n";
                InsertSimpleToPlaylist(playlistId, stationId, areaId, deviceId, messageId);
                txtSimpleResult.Text += "      ✓ 播放列表更新成功\r\n\r\n";
                Application.DoEvents();

                txtSimpleResult.Text += "===== 消息发送完成 =====\r\n";
                txtSimpleResult.Text += "数据已保存到数据库\r\n";
                txtSimpleResult.Text += "DCU Service 运行时会自动从数据库读取并发送到显示器\r\n";
                txtSimpleResult.Text += $"完成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n";

                MessageBox.Show("消息已保存到数据库！\r\n\r\n如果 DCU Service 正在运行，将自动处理并发送到显示器。", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                txtSimpleResult.Text += $"\r\n✗✗✗ 发送失败 ✗✗✗\r\n";
                txtSimpleResult.Text += $"错误信息: {ex.Message}\r\n";
                txtSimpleResult.Text += $"错误类型: {ex.GetType().Name}\r\n";

                if (ex.InnerException != null)
                {
                    txtSimpleResult.Text += $"内部异常: {ex.InnerException.Message}\r\n";
                }

                txtSimpleResult.Text += $"\r\n堆栈跟踪:\r\n{ex.StackTrace}\r\n";

                MessageBox.Show($"发送失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 插入简化消息到数据库（使用固定的默认值）
        /// </summary>
        private void InsertSimpleMessageToDatabase(Guid messageId, string messageText, bool isInstantMessage)
        {
            // 使用固定的默认参数
            string messageName = $"UITest测试_{DateTime.Now:HHmmss}";
            int messageType = 71;        // FullWindow
            int messagePriority = 1;     // 优先级 1（最高）
            int moveMode = 100;          // 0x64 = 静态显示
            int moveSpeed = 5;           // 滚动速度
            string interval = "0";       // 播放间隔

            // 字体设置
            string fontTypeCHN = "明體";  // 0x01
            int fontSizeCHN = 24;        // 24x24
            string fontColorCHN = "白色"; // 白色

            string fontTypeENG = "明體";
            int fontSizeENG = 24;
            string fontColorENG = "白色";

            txtSimpleResult.Text += $"      消息类型: {(isInstantMessage ? "即时消息" : "预录消息")}\r\n";
            txtSimpleResult.Text += $"      消息内容: {messageText}\r\n";
            txtSimpleResult.Text += $"      字体: {fontTypeCHN} {fontSizeCHN}pt\r\n";
            txtSimpleResult.Text += $"      颜色: {fontColorCHN}\r\n";
            txtSimpleResult.Text += $"      显示模式: {(moveMode == 100 ? "静态" : "滚动")}\r\n";

            if (isInstantMessage)
            {
                // 插入即时消息
                dmdInstantMessage.InsertInstantMessages(
                    messageId,
                    messageType,
                    messagePriority,
                    moveMode,
                    moveSpeed,
                    interval,
                    messageText,          // 中文内容
                    fontTypeCHN,
                    fontSizeCHN,
                    fontColorCHN,
                    messageText,          // 英文内容（与中文相同）
                    fontTypeENG,
                    fontSizeENG,
                    fontColorENG
                );
                txtSimpleResult.Text += "      数据库表: dmd_instant_message\r\n";
            }
            else
            {
                // 插入预录消息
                dmdPreRecordMessage.InsertPreRecordMessage(
                    messageId,
                    messageName,
                    messageType,
                    messagePriority,
                    moveMode,
                    moveSpeed,
                    interval,
                    messageText,          // 中文内容
                    fontTypeCHN,
                    fontSizeCHN,
                    fontColorCHN,
                    messageText,          // 英文内容（与中文相同）
                    fontTypeENG,
                    fontSizeENG,
                    fontColorENG
                );
                txtSimpleResult.Text += "      数据库表: dmd_pre_record_message\r\n";
            }
        }

        /// <summary>
        /// 插入到播放列表（简化版）
        /// </summary>
        private void InsertSimpleToPlaylist(Guid playlistId, string stationId, string areaId, string deviceId, Guid messageId)
        {
            txtSimpleResult.Text += $"      删除旧播放列表: {stationId}_{areaId}_{deviceId}\r\n";

            // 删除该设备的现有播放列表
            dmdPlayList.DeletePlayingItem(stationId, areaId, deviceId);

            // 插入新的播放项
            dmdPlayList.InsertPlayingItem(
                playlistId,
                stationId,
                areaId,
                deviceId,
                messageId,
                71, // message_type: 71 = FullWindow
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            );

            txtSimpleResult.Text += $"      插入新播放项: {messageId}\r\n";
        }

        /// <summary>
        /// 清除简化结果
        /// </summary>
        private void btnSimpleClearResult_Click(object sender, EventArgs e)
        {
            txtSimpleResult.Text = "";
        }

        /// <summary>
        /// 使用默认测试值填充字段
        /// </summary>
        private void btnSimpleUseDefaults_Click(object sender, EventArgs e)
        {
            txtSimpleMessageText.Text = "UITest 測試消息 - Test Message";
            txtSimpleTargetDevice.Text = "LG01_CCS_CDU-1";
            chkSimpleInstantMessage.Checked = false;

            txtSimpleResult.Text = "✓ 已填充默认测试值\r\n";
            txtSimpleResult.Text += "  消息内容: UITest 測試消息 - Test Message\r\n";
            txtSimpleResult.Text += "  目标设备: LG01_CCS_CDU-1\r\n";
            txtSimpleResult.Text += "  消息类型: 预录消息\r\n";
        }

        #endregion
    }
}
