using Display;
using Display.DisplayMode;
using System.Collections.Generic;
using static Display.DisplaySettingsEnums;

namespace UITest.Services
{
    // ── 上傳參數 ──────────────────────────────────────────────────────────
    public class DisplayMessageParams
    {
        public string            Text         { get; set; } = "";
        public string            HexColor     { get; set; } = "FFFF00";
        public FontSize          FontSz       { get; set; } = FontSize.Font24x24;
        public Display.FontStyle FontSty      { get; set; } = Display.FontStyle.Ming;
        public byte              ScrollMode   { get; set; } = 0x61;
        public byte              ScrollSpeed  { get; set; } = 5;
        public byte              PauseTime    { get; set; } = 10;
        public byte              MessageLevel { get; set; } = 4;
        /// <summary>WindowDisplayMode 的 byte 值（0x71, 0x72 ...）</summary>
        public byte              MessageType  { get; set; } = 0x71;
        /// <summary>0x01=上行 / 0x02=下行</summary>
        public byte              SequenceNo   { get; set; } = 0x01;
        public List<byte>        TargetIDs    { get; set; } = new List<byte>();
        public byte              FunctionCode { get; set; } = 0x34;
    }

    // ── Service（不依賴任何 UI 元件）────────────────────────────────────
    public class DisplayMessageService
    {
        private readonly PacketBuilderService _builder = new PacketBuilderService();

        /// <summary>
        /// 依 DisplayMessageParams 組成完整封包，回傳序列化 byte[]。
        /// 若任何步驟失敗，回傳 BuildResult.IsValid = false。
        /// </summary>
        public BuildResult<byte[]> Build(DisplayMessageParams p)
        {
            // ── Step 1: TextStringBody ────────────────────────────────────
            var r1 = _builder.BuildTextStringBody(p.HexColor, p.Text);
            if (!r1.IsValid) return BuildResult<byte[]>.Fail($"[StringBody] {r1.ErrorMessage}");

            // ── Step 2: StringMessage ─────────────────────────────────────
            var r2 = _builder.BuildStringMessage(r1.Value);
            if (!r2.IsValid) return BuildResult<byte[]>.Fail($"[StringMessage] {r2.ErrorMessage}");

            // ── Step 3: FullWindow ────────────────────────────────────────
            var scroll = new ScrollInfo
            {
                ScrollMode  = p.ScrollMode,
                ScrollSpeed = p.ScrollSpeed,
                PauseTime   = p.PauseTime
            };
            var r3 = _builder.BuildFullWindow(r2.Value, p.MessageLevel, scroll);
            if (!r3.IsValid) return BuildResult<byte[]>.Fail($"[FullWindow] {r3.ErrorMessage}");

            // 覆蓋 MessageType 為板型選擇的對應值（BuildFullWindow 預設 0x71）
            r3.Value.MessageType = p.MessageType;

            // ── Step 4: Sequence ──────────────────────────────────────────
            var font = new FontSetting { Size = p.FontSz, Style = p.FontSty };
            var r4 = _builder.BuildSequence(r3.Value, font);
            if (!r4.IsValid) return BuildResult<byte[]>.Fail($"[Sequence] {r4.ErrorMessage}");

            r4.Value.SequenceNo = p.SequenceNo;

            // ── Step 5: Packet ────────────────────────────────────────────
            var r5 = _builder.BuildPacket(r4.Value, p.TargetIDs, p.FunctionCode);
            if (!r5.IsValid) return BuildResult<byte[]>.Fail($"[Packet] {r5.ErrorMessage}");

            var bytes = r5.Value.ToBytes();
            return BuildResult<byte[]>.Success(bytes, PacketBuilderService.ToHex(bytes));
        }
    }
}
