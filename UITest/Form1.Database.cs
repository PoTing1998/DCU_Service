using Npgsql;
using System;
using System.Windows.Forms;

namespace UITest
{
    /// <summary>
    /// 資料庫設定相關功能
    /// </summary>
    public partial class Form1
    {
        #region Database Settings

        /// <summary>
        /// 從DMD.DB.Manager載入資料庫設定到UI
        /// </summary>
        private void LoadDatabaseSettings()
        {
            txtDbServer.Text = ASI.Wanda.DMD.DB.Manager.ConnectIP;
            txtDbPort.Text = ASI.Wanda.DMD.DB.Manager.ConnPort;
            txtDbDatabase.Text = ASI.Wanda.DMD.DB.Manager.DataBaseName;
            txtDbUserId.Text = ASI.Wanda.DMD.DB.Manager.UserID;
            txtDbPassword.Text = ASI.Wanda.DMD.DB.Manager.Passward;
            UpdateConnectionStringDisplay();
        }

        /// <summary>
        /// 更新連線字串顯示
        /// </summary>
        private void UpdateConnectionStringDisplay()
        {
            txtDbConnectionString.Text = ASI.Wanda.DMD.DB.Manager.ConnectionString;
        }

        /// <summary>
        /// 更新連線字串按鈕點擊事件
        /// </summary>
        private void btnUpdateConnectionString_Click(object sender, EventArgs e)
        {
            try
            {
                // 驗證輸入
                if (string.IsNullOrWhiteSpace(txtDbServer.Text) ||
                    string.IsNullOrWhiteSpace(txtDbPort.Text) ||
                    string.IsNullOrWhiteSpace(txtDbDatabase.Text) ||
                    string.IsNullOrWhiteSpace(txtDbUserId.Text))
                {
                    txtDbStatus.Text = "✗ 錯誤: 伺服器、連接埠、資料庫名稱和使用者ID不能為空";
                    return;
                }

                // 更新DMD.DB.Manager的連線設定
                bool result = ASI.Wanda.DMD.DB.Manager.Initializer(
                    txtDbServer.Text,
                    txtDbPort.Text,
                    txtDbDatabase.Text,
                    txtDbUserId.Text,
                    txtDbPassword.Text,
                    "admin"
                );

                if (result)
                {
                    UpdateConnectionStringDisplay();
                    txtDbStatus.Text = "✓ 連線字串已成功更新";
                }
                else
                {
                    txtDbStatus.Text = "⚠ 連線字串更新失敗";
                }
            }
            catch (Exception ex)
            {
                txtDbStatus.Text = $"✗ 更新連線字串失敗:\r\n{ex.Message}";
            }
        }

        /// <summary>
        /// 測試資料庫連線按鈕點擊事件
        /// </summary>
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                txtDbStatus.Text = "正在測試連線...\r\n";
                Application.DoEvents();

                string connectionString = ASI.Wanda.DMD.DB.Manager.ConnectionString;
                txtDbStatus.Text += $"連線字串: {connectionString}\r\n\r\n";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    txtDbStatus.Text += "✓ 資料庫連線測試成功!\r\n";
                    txtDbStatus.Text += $"伺服器版本: {connection.ServerVersion}\r\n";
                    txtDbStatus.Text += $"資料庫: {connection.Database}\r\n";

                    // 測試查詢  
                    using (var command = new NpgsqlCommand("SELECT COUNT(*) FROM dbo.dmd_target", connection))
                    {
                        var count = command.ExecuteScalar();
                        txtDbStatus.Text += $"\r\ndmd_target 表中有 {count} 筆設備資料";
                    }
                }
            }
            catch (Exception ex)
            {
                txtDbStatus.Text += $"\r\n✗ 資料庫連線測試失敗:\r\n{ex.Message}\r\n";
                txtDbStatus.Text += $"\r\n例外類型: {ex.GetType().Name}";
                if (ex.InnerException != null)
                {
                    txtDbStatus.Text += $"\r\n內部例外: {ex.InnerException.Message}";
                }
            }
        }

        #endregion
    }
}
