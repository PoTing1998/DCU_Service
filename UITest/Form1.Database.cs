using Npgsql;
using System;
using System.Data;
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

        #region Table Browser

        /// <summary>
        /// 載入資料庫表列表
        /// </summary>
        private void btnLoadTables_Click(object sender, EventArgs e)
        {
            try
            {
                cmbTables.Items.Clear();
                lblRowCount.Text = "資料筆數: 0";

                string connectionString = ASI.Wanda.DMD.DB.Manager.ConnectionString;

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // 查詢所有表（來自 public schema 和 dbo schema）
                    string query = @"
                        SELECT table_schema, table_name
                        FROM information_schema.tables
                        WHERE table_type = 'BASE TABLE'
                          AND table_schema IN ('public', 'dbo')
                        ORDER BY table_schema, table_name";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string schema = reader.GetString(0);
                                string tableName = reader.GetString(1);
                                // 格式: schema.table_name
                                cmbTables.Items.Add($"{schema}.{tableName}");
                            }
                        }
                    }
                }

                if (cmbTables.Items.Count > 0)
                {
                    cmbTables.SelectedIndex = 0;
                    MessageBox.Show($"成功載入 {cmbTables.Items.Count} 個資料表", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("沒有找到任何資料表", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入表列表失敗:\r\n{ex.Message}", "錯誤",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 查詢表內容
        /// </summary>
        private void btnQueryTable_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTables.SelectedItem == null)
                {
                    MessageBox.Show("請先選擇一個資料表", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string tableName = cmbTables.SelectedItem.ToString();
                string whereClause = txtTableFilter.Text.Trim();

                // 構建查詢語句
                string query = $"SELECT * FROM {tableName}";
                if (!string.IsNullOrWhiteSpace(whereClause))
                {
                    query += $" WHERE {whereClause}";
                }

                // 限制最多返回 1000 筆資料，避免過大
                query += " LIMIT 1000";

                string connectionString = ASI.Wanda.DMD.DB.Manager.ConnectionString;

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // 綁定到 DataGridView
                            dgvTableData.DataSource = dataTable;

                            // 更新資料筆數
                            lblRowCount.Text = $"資料筆數: {dataTable.Rows.Count}";
                            lblRowCount.ForeColor = dataTable.Rows.Count == 1000
                                ? System.Drawing.Color.Red
                                : System.Drawing.Color.Black;

                            if (dataTable.Rows.Count == 1000)
                            {
                                MessageBox.Show(
                                    "查詢結果已達到 1000 筆上限，可能還有更多資料未顯示。\r\n請使用篩選條件來縮小查詢範圍。",
                                    "提示",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查詢表內容失敗:\r\n{ex.Message}\r\n\r\n請檢查篩選條件是否正確。",
                    "錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 清除表資料顯示
        /// </summary>
        private void btnClearTableData_Click(object sender, EventArgs e)
        {
            dgvTableData.DataSource = null;
            lblRowCount.Text = "資料筆數: 0";
            lblRowCount.ForeColor = System.Drawing.Color.Black;
        }

        #endregion
    }
}
