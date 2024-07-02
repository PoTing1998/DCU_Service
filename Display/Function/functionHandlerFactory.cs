using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.Function
{
    public class FunctionHandlerFactory
    {
        private readonly Dictionary<byte, IFunctionHandler> _handlers;

        public FunctionHandlerFactory()
        {
            _handlers = new Dictionary<byte, IFunctionHandler>
        {
            { 0x31, new CommunicationTestHandler() },
            { 0x32, new ParameterSettingHandler() },
            { 0x33, new PowerControlHandler() },
            { 0x34, new PassengerInfoHandler() },
            { 0x35, new PreRecordedDatabaseHandler() },
            { 0x36, new FontDatabaseUpdateHandler() },
            { 0x37, new DeviceCommunicationUpdateHandler() },
            { 0x38, new EmergencyMessagePlaybackHandler() }
        };
        }

        public IFunctionHandler GetHandler(byte functionCode)
        {
            if (_handlers.TryGetValue(functionCode, out var handler))
            {
                return handler;
            }
            throw new InvalidOperationException($"No handler found for FunctionCode {functionCode}");
        }
    }


}
