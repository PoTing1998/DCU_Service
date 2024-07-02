using Display.DisplayMode;
using Display.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ASI.Wanda.DCU.DB;
namespace Display
{
   static class Program
    {
        static void Main()
        {
           
            var processor = new PacketProcessor();

            var textStringBody = new TextStringBody
            {
                RedColor = 0xFF,
                GreenColor = 0x00,
                BlueColor = 0x00,
                StringText = "Hello"
            };

            var stringMessage = new StringMessage
            {
                StringMode = 0x2A, // TextMode (Static)   
                StringBody = textStringBody
            };

            var fullWindowMessage = new FullWindow //Display version
            {
                MessageType = 0x71, // FullWindow message
                MessageLevel = 0x01, //  level 
                MessageScroll = new ScrollInfo { ScrollMode = 1, ScrollSpeed = 1, PauseTime = 1 },
                Font = new FontSetting { Size = FontSize.Font16x16, Style = FontStyle.Ming },
                MessageContent = new List<StringMessage> { stringMessage }  
            };

            var sequence1 = new Sequence 
            {
                SequenceNo = 1,
                Messages = new List<IMessage> { fullWindowMessage }  
            };

            var startCode = new byte[] { 0xAA, 0x55 };   
            var function = new PassengerInfoHandler(); // Use PassengerInfoHandler  
            var packet = processor.CreatePacket(startCode, new List<byte> { 0x01, 0x02, 0x03 }, function.FunctionCode, new List<Sequence> { sequence1 });

            var serializedData = processor.SerializePacket(packet);
            Console.WriteLine("Serialized Data: " + BitConverter.ToString(serializedData)); 

            var deserializedPacket = processor.DeserializePacket(serializedData);
            Console.WriteLine("Deserialized Packet: ");   
            Console.WriteLine("StartCode: " + BitConverter.ToString(deserializedPacket.StartCode));
            Console.WriteLine("FunctionCode: " + deserializedPacket.FunctionCode);
            Console.WriteLine("CheckSum: " + deserializedPacket.CheckSum);
             
            var handledData = processor.HandlePacket(deserializedPacket);
            Console.WriteLine("Handled Data: " + BitConverter.ToString(handledData));
      
        }
    }
     class dataBase 
    {
       void test()
        {
            ASI.Wanda.DCU.DB.Tables.DCU.userControlPanel.SelectAll();
        }
      
    }
}



