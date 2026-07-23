# CDU / SDU / PDN / PUP 共用邏輯重構設計

## 0. 結論先講

四個專案的重複程式碼分三個層次，風險與工作量遞增：

| 層次 | 內容 | 目前重複程度 | 建議處理順序 |
|---|---|---|---|
| L1 PA 訊息解析 | `ProcessDataBytes`/`HandleCase01`/`HandleCase15`/`HexStringToBytes`/`CalculateLRC` | 4 份幾乎逐字相同 | 第 1 波，風險最低 |
| L2 Helper 共用方法 | `TaskXXXHelper.cs` 裡 `CreatePacket`/`SerializeAndSendPacket`/`HandleError`/`ProcessMessageColor`/`CreateDisplaySequence` 等 | 4 份結構相同，但藏了一個實際的資料不一致（見第 2 節） | 第 2 波，需先確認 bug |
| L3 訊息分派主流程 | `ProMsgFromDMD`（`ProcEvent`/`StartTask`/序列埠事件） | 邏輯形狀相同、寫法風格不同（PUP 已用 Strategy Pattern，其他三個是 switch-case 內嵌） | 第 3 波，工作量最大 |

建議新增一個共用類別庫專案 **`TaskDU_Common`**，四個 Task 專案都參照它，而不是塞進 `ASILib`（原因見第 3 節）。

---

## 1. 現況重複量化

```
TaskCDU/ProcTaskCDU.cs        420 行
TaskSDU/ProcTaskSDU.cs        383 行
TaskPDN/ProTaskPDN.cs         387 行
TaskPUP/ProcTaskPUP.cs        296 行
TaskPUP/PAMessage.cs          204 行   ← PUP 已經把 PA 訊息處理抽成獨立類別
TaskCDU/TaskCDUHelper.cs      861 行
TaskSDU/TaskSduHelper.cs      626 行
TaskPDN/TaskPDNHelper.cs      737 行
TaskPUP/TaskPUPHelper.cs      845 行
------------------------------------
合計                         4,759 行
```

逐方法比對後，`CreatePacket`、`SerializeAndSendPacket`、`HandleError`、`ProcessMessageColor`、`HandleCase01`、`HexStringToBytes`、`CalculateLRC`、`FireAlarmMessages` 這幾塊在四個檔案裡是**只有空白/註解差異**的複製貼上，粗估 Helper 檔案裡有 55–65% 的行數可以合併成共用基底類別。

---

## 2. 比對過程中發現的兩個真實風險（不只是重複，是潛在 bug）

**(a) front/back 位元組順序不一致**

`CreatePacket` 裡組封包時：

- CDU、PDN：`new List<byte> { front, back }`
- SDU、PUP：`new List<byte> { back, front }`

四份程式碼算 `front`/`back` 的方式完全相同（都是呼叫 `GetPanelIDByDuAndOrientation(DU_ID, false/true)`），但組進封包的順序前後相反。這可能是：
1. 四種硬體真的有不同的接線/面板順序（那就是刻意的，需要在重構時保留成參數化差異），或
2. 單純複製貼上時漏改，其中一組是 bug。

**這點需要你或熟悉硬體的人確認**，重構前必須先釐清，否則合併成共用基底類別時會把 bug 一起複製，或誤把正確行為改壞。

**(b) HandleCase15 錯誤訊息語言不一致**

CDU 是中文（`"表示數據包長度錯誤"` 等），SDU、PDN、PUP 是英文（`"Indicates packet data length error"`）。純粹是 log 文字，不影響功能，但合併時要決定統一成中文還是英文。

---

## 3. 為什麼不放進 `ASILib`，而要開新專案

`ASILib` 是最底層共用函式庫，被 `DCU_DB`、`Display`、`DMD_Frame` 等所有專案往上依賴。而這次要共用的邏輯（`CreatePacket`、`SendMessageToUrgnt`、DB 查詢面板 ID 等）本身依賴 `DCU_DB`、`Display`、`DMD_Frame`、`PA_Frame`——如果塞進 `ASILib`，會變成 `ASILib → DCU_DB → ASILib` 的循環依賴，編譯不過。

所以新增一個中間層專案 **`TaskDU_Common`**，依賴關係跟現在的四個 Task 專案一樣（`ASILib`、`DCU_DB`、`DCU_Frame`、`Display`、`DMD_Frame`、`PA_Frame`），只是被四個 Task 專案共同參照：

```
ASILib
  └─ DCU_DB / DCU_Frame / Display / DMD_Frame / PA_Frame
       └─ TaskDU_Common  ← 新增
            ├─ TaskCDU
            ├─ TaskSDU
            ├─ TaskPDN
            └─ TaskPUP
```

---

## 4. `TaskDU_Common` 內容規劃

```
TaskDU_Common/
├── Protocol/
│   └── PAMessageHandler.cs        // 取代 4 份 ProcessDataBytes/HandleCase01/15/HexStringToBytes/CalculateLRC
├── Constants/
│   ├── FireAlarmMessages.cs       // 取代 CDU/SDU/PDN 私有版本 + PUP 的 TaskPUPConstants 版本
│   └── DmdMessageConstants.cs     // SendPreRecordMsg / SendInstantMsg / ScheduleMsg 等，目前散落各 Helper
├── TaskDUHelperBase.cs            // 抽象基底，收納 CreatePacket/SerializeAndSendPacket/HandleError/
│                                   // ProcessMessageColor/CreateDisplaySequence/GetPlayingItemIds/
│                                   // CreateTextStringBody/CreateFullWindowMessage/PowerSettingOpen/Off/
│                                   // PowerSetting/SplitStringToDeviceInfo/ValidateInput/GetDeviceInfo
│                                   // front/back 順序等「裝置差異」開放成 abstract/virtual 或建構子參數
└── ProcTaskDUBase.cs               // 抽象基底，收納 ProcEvent 的 Label 判斷骨架、StartTask、
                                    // InitSerial/InitDatabase、SerialPort_DisconnectedEvent/ReceivedEvent、
                                    // OpenDisplay/CloseDisplay
```

各專案改法：

- `TaskCDUHelper : TaskDUHelperBase`，只保留 CDU 特有的方法（例如它獨有的 `SendInstantMessage`/`SendBatchMessage` 多目標邏輯）
- `TaskSduHelper : TaskDUHelperBase`
- `TaskPDNHelper : TaskDUHelperBase`
- `TaskPUPHelper : TaskDUHelperBase`
- `ProcTaskCDU/SDU/PDN/PUP : ProcTaskDUBase`，只覆寫 `ProMsgFromDMD` 中真正因裝置而異的分派邏輯

---

## 5. 分階段執行計畫

**第 1 波（低風險，建議先做）：PA 訊息處理層**
把 `ProcessDataBytes`、`ProcessByteAtIndex2`、`HandleCase01`、`HandleCase15`、`HexStringToBytes`、`CalculateLRC`、`FireAlarmMessages` 搬進 `TaskDU_Common/Protocol/PAMessageHandler.cs`，四個專案改成呼叫共用類別。這段目前四份幾乎完全相同（只有語言差異），順便統一成同一種語言。可先在 CDU 做示範，跑通、確認行為不變後，再套用到 SDU/PDN/PUP。

**第 2 波（中風險，需先解決前後順序疑問）：Helper 共用方法**
把 `TaskDUHelperBase` 抽出來，先處理沒有爭議的方法（`HandleError`、`ProcessMessageColor`、`SerializeAndSendPacket`），front/back 順序這種有疑慮的方法（`CreatePacket`）先留一個 `protected abstract bool IsReversedOrder` 之類的開關，把差異顯式化而不是默默統一，避免踩到硬體行為。

**第 3 波（風險較高，工作量最大）：主流程與訊息分派**
`ProcTaskDUBase` 收斂 `ProcEvent`/`StartTask`/序列埠事件這些骨架邏輯。`ProMsgFromDMD` 建議比照 PUP 現有的 `Strategies/IMessageStrategy` 模式，四個裝置共用同一套策略介面，各自只實作差異策略類別，而不是繼續用 switch-case 內嵌。這波要動到訊息分派主流程，建議放最後、且要有現場封包回放測試把關。

**驗證方式**：這是控制實體顯示/廣播設備的系統，光靠肉眼看程式碼不夠。建議每一波重構後，用側錄下來的既有 MSMQ 訊息或序列埠封包做「重構前 vs 重構後」的 byte-for-byte 輸出比對（可以寫個簡單的重放測試工具餵同樣輸入，比對 `_mSerial.Send()` 的輸出位元組），確認行為完全一致再上線。

---

## 6. 寫法風格統一建議

- Log 語言統一（目前中英文混雜），建議統一用中文，與系統其他模組一致
- `_mProcName`、`_mSerial` 等欄位命名/建構子簽章已經一致，維持
- XML 文件註解統一使用中文
- 例外處理統一模式：目前 `catch(Exception ex)` 大多只做 `ErrorLog.Log(...)`，建議至少統一記錄格式（含方法名、輸入內容）

---

## 7. 下一步

如果你要開始動手，建議從**第 1 波**開始：先建立 `TaskDU_Common` 專案 + `PAMessageHandler`，只改 CDU 一個專案驗證可行，我可以先給你這部分的實際程式碼 diff 讓你 review，確認沒問題後再套用到 SDU/PDN/PUP。
