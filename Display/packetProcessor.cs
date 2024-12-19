using Display.DisplayMode;
using Display.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Display.DisplaySettingsEnums;
using static Display.Packet;

namespace Display
{


    public class PacketProcessor
    {
        private readonly FunctionHandlerFactory _handlerFactory;

        public PacketProcessor()
        {
            _handlerFactory = new FunctionHandlerFactory();
        }

        public Packet CreatePacket(byte[] startCode, List<byte> ids, byte functionCode, List<Sequence> sequences)
        {
            var packet = new Packet
            {
                StartCode = startCode,
                IDs = ids,
                FunctionCode = functionCode,
                Sequences = sequences
            };
            // Generate CheckSum after all other fields are set
            packet.CheckSum = (byte)(packet.ToBytes().Sum(b => b) & 0xFF);

            return packet;
        }



        public CustomPacket CreatePacketOff(byte[] startCode, List<byte> ids, byte functionCode, byte[] sequences)
        {
            var customPacket = new CustomPacket
            {
                StartCode = startCode,
                IDs = ids,
                FunctionCode = functionCode,
                Sequences = sequences
            };

            // Generate CheckSum after all other fields are set
            customPacket.CheckSum = (byte)(customPacket.ToBytes().Sum(b => b) & 0xFF);

            return customPacket;
        }

        public byte[] SerializePacket(Packet packet)
        {
            return packet.ToBytes();
        }
        public byte[] SerializePacket(CustomPacket packet)
        {
            return packet.ToBytes();
        }

        public Packet DeserializePacket(byte[] data)
        {
            var startCode = data.Take(2).ToArray(); // Read StartCode (2 bytes)   
            var idLength = data[2];
            var ids = data.Skip(3).Take(idLength).ToList();
            var functionCode = data[3 + idLength];
            var dataLength = BitConverter.ToUInt16(data, 4 + idLength); // Data length
            var sequenceData = data.Skip(6 + idLength).Take(dataLength).ToArray();
            var checkSum = data.Last();

            var packet = new Packet
            {
                StartCode = startCode,
                IDs = ids,
                FunctionCode = functionCode,
                Sequences = new List<Sequence>(),
                CheckSum = checkSum
            };

            // Parse Sequences
            int index = 0;
            while (index < sequenceData.Length) 
            {
                var sequenceNo = sequenceData[index];
                var sequenceLength = BitConverter.ToUInt16(sequenceData, index + 1);
                var sequenceContent = sequenceData.Skip(index + 3).Take(sequenceLength - 1).ToArray();
                var sequenceEnd = sequenceData[index + 3 + sequenceContent.Length];

                var sequence = new Sequence
                {
                    SequenceNo = sequenceNo,
                    Messages = new List<IMessage>(), 
                    SequenceEnd = sequenceEnd
                };

                // Parse Messages
                int msgIndex = 0;
                while (msgIndex < sequenceContent.Length)
                {
                    var messageType = sequenceContent[msgIndex];
                    IMessage message; 

                    if (messageType == 0x7F)
                    {
                        // FontSetting
                        var fontSize = (FontSize)sequenceContent[msgIndex + 1];
                        var fontStyle = (FontStyle)sequenceContent[msgIndex + 2];
                        message = new FontSetting 
                        {
                            Size = fontSize,
                            Style = fontStyle
                        };

                        msgIndex += 3;
                    }
                    else if (Enum.IsDefined(typeof(WindowDisplayMode), messageType))
                    {
                        ushort messageLength = BitConverter.ToUInt16(sequenceContent, msgIndex + 1);
                        var messageLevel = sequenceContent[msgIndex + 3];
                       var  scrollInfo = new ScrollInfo
                        {
                            ScrollMode = sequenceContent[msgIndex + 4],
                            ScrollSpeed = sequenceContent[msgIndex + 5],
                            PauseTime = sequenceContent[msgIndex + 6]
                        };

                        var fontSetting = new FontSetting
                        {
                            Size = (FontSize)sequenceContent[msgIndex + 7],
                            Style = (FontStyle)sequenceContent[msgIndex + 8]
                        };

                        var messageContentBytes = sequenceContent.Skip(msgIndex + 9).Take(messageLength - 7).ToArray();
                        var messageEnd = sequenceContent[msgIndex + 9 + messageContentBytes.Length];

                        var stringMessages = ParseStringMessages(messageContentBytes);
                        message = new FullWindow
                        {
                            MessageType = messageType,
                            MessageLevel = messageLevel,
                            MessageScroll = scrollInfo,
                            MessageContent = stringMessages,
                            MessageEnd = messageEnd
                        };
                       var Font = fontSetting;
                        msgIndex += 3 + messageLength;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unknown MessageType {messageType}");
                    }

                    sequence.Messages.Add(message);
                }

                packet.Sequences.Add(sequence);
                index += 3 + sequenceLength;
            }

            return packet;
        }
        private List<StringMessage> ParseStringMessages(byte[] data)
        {
            var strings = new List<StringMessage>();
            int index = 0;

            while (index < data.Length)
            {
                var mode = data[index];
                StringBody body;

                switch (mode)
                {
                    case 0x2A: // TextMode (Static) 
                    case 0x2B: // TextMode (Blinking)
                        body = new TextStringBody
                        {
                            RedColor = data[index + 1],
                            GreenColor = data[index + 2],
                            BlueColor = data[index + 3],
                            StringText = Encoding.GetEncoding("BIG5").GetString(data, index + 4, data.Length - (index + 5))
                        };
                        break;
                    
                    case 0x2C: // PreRecordedText 
                        body = new PreRecordedTextBody
                        {
                            IndexNumber = BitConverter.ToUInt16(data, index + 1),
                            RedColor = data[index + 3],
                            GreenColor = data[index + 4],
                            BlueColor = data[index + 5]
                        };
                        break;

                    case 0x2D: // PreRecordedGraphic 
                        body = new PreRecordedGraphicBody
                        {
                            GraphicStartIndex = BitConverter.ToUInt16(data, index + 1),
                            GraphicNumber = data[index + 3],
                            RedColor = data[index + 4],
                            GreenColor = data[index + 5],
                            BlueColor = data[index + 6]
                        };
                        break;

                    default:
                        throw new InvalidOperationException("Unknown StringMode");
                }

                var stringMessage = new StringMessage 
                {
                    StringMode = mode,
                    StringBody = body,
                    StringEnd = data[index + data.Length - 1]
                };

                strings.Add(stringMessage); 
                index += 4 + body.ToBytes().Length; // Move to next StringMessage 
            }

            return strings;
        }
    }
}


