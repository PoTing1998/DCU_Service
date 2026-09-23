using System;
using ASI.Wanda.DMD.Service.Handlers;

namespace ASI.Wanda.DMD.Service
{
    /// <summary>
    /// DMD 訊息分派的核心邏輯（原 ProcTaskDMD.DMD_API_ReceivedEvent 內容）。
    /// TaskDMD 服務與 UITest「DMD 接收」畫面共用同一份邏輯，差別只在傳入的 <see cref="DMDHelper"/>（真實 / 模擬）。
    /// </summary>
    public class DMDMessageProcessor
    {
        private readonly DMDMessageHandlerFactory _handlerFactory = new DMDMessageHandlerFactory();
        private readonly TrainSignalHandler _trainSignalHandler = new TrainSignalHandler();
        private readonly object _lock = new object();

        /// <summary>
        /// 處理一筆從 DMD Server 收到的訊息
        /// </summary>
        public DMDProcessResult Process(ASI.Wanda.DMD.Message.Message DMDServerMessage, DMDHelper helper)
        {
            if (DMDServerMessage == null) throw new ArgumentNullException(nameof(DMDServerMessage));
            if (helper == null) throw new ArgumentNullException(nameof(helper));

            var result = new DMDProcessResult
            {
                IsSimulated = helper.IsSimulated,
                MessageType = DMDServerMessage.MessageType,
                MessageID   = DMDServerMessage.MessageID
            };

            lock (_lock)
            {
                var previousTrace = helper.Trace;
                helper.Trace = text => { result.Actions.Add(text); previousTrace?.Invoke(text); };
                try
                {
                    result.MessageLength  = DMDServerMessage.MessageLength;
                    result.HexContent     = DMDServerMessage.CompleteContent == null ? ""
                        : ASI.Lib.Text.Parsing.String.BytesToHexString(DMDServerMessage.CompleteContent, "");
                    result.JsonContent    = DMDServerMessage.JsonContent;
                    result.JsonObjectName = string.IsNullOrEmpty(result.JsonContent) ? ""
                        : ASI.Lib.Text.Parsing.Json.GetValue(result.JsonContent, "JsonObjectName");

                    switch (DMDServerMessage.MessageType)
                    {
                        case ASI.Wanda.DMD.Message.Message.eMessageType.Ack:
                            result.HandlerName = "Ack";
                            helper.HandleAckMessage(DMDServerMessage);
                            break;

                        case ASI.Wanda.DMD.Message.Message.eMessageType.Command:
                            HandleCommand(DMDServerMessage, helper, result);
                            break;

                        case ASI.Wanda.DMD.Message.Message.eMessageType.Response:
                            result.HandlerName = "(不應收到 Response)";
                            result.Success = false;
                            result.ErrorMessage = $"從DMD Server來的訊息不應有Response，MessageType:{DMDServerMessage.MessageType}";
                            ASI.Lib.Log.ErrorLog.Log("TaskDMD", result.ErrorMessage);
                            break;

                        case ASI.Wanda.DMD.Message.Message.eMessageType.trainMessage:
                            LogReceived(DMDServerMessage, result);
                            result.HandlerName = nameof(TrainSignalHandler);
                            _trainSignalHandler.Handle(DMDServerMessage, helper);
                            break;

                        default:
                            result.Success = false;
                            result.ErrorMessage = $"無此種訊息類別:[{DMDServerMessage.MessageType}]";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.ErrorMessage = ex.Message;
                    ASI.Lib.Log.ErrorLog.Log("TaskDMD", ex);
                }
                finally
                {
                    helper.Trace = previousTrace;
                }
            }
            return result;
        }

        private void HandleCommand(ASI.Wanda.DMD.Message.Message msg, DMDHelper helper, DMDProcessResult result)
        {
            LogReceived(msg, result);

            var handler = _handlerFactory.GetHandler(result.JsonObjectName);
            if (handler != null)
            {
                result.HandlerName = handler.GetType().Name;
                handler.Handle(msg, helper);
            }
            else if (result.JsonObjectName == Constants.ParameterSetting)
            {
                result.HandlerName = "ParameterSetting(Ack)";
                helper.HandleAckMessage(msg);
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = $"未找到處理器: {result.JsonObjectName}";
                ASI.Lib.Log.DebugLog.Log("FromDMD_server", result.ErrorMessage);
            }
        }

        private static void LogReceived(ASI.Wanda.DMD.Message.Message msg, DMDProcessResult r)
        {
            string sLog = $"從DMD Server收到:{r.HexContent}；訊息類別碼:{msg.MessageType}；識別碼:{r.MessageID}；長度:{r.MessageLength}；內容:{r.JsonContent}；JsonObjectName:{r.JsonObjectName}";
            ASI.Lib.Log.DebugLog.Log("FromDMD_server", $"{sLog}\r\n");
        }
    }
}
