using System;
using System.Windows.Forms;

using UITest.Services;
using UITest.PacketVerifiers;

namespace UITest.Controls
{
    public partial class PacketVerifyControl : UserControl
    {
        public PacketVerifyControl()
        {
            InitializeComponent();
        }

        private void Version1BT_Click(object sender, EventArgs e) => ValidateAndShow(txtV1.Text, new FullWindowHandlerVerify());
        private void Version2BT_Click(object sender, EventArgs e) => ValidateAndShow(txtV2.Text, new LeftPlatformHandlerVerify());
        private void Version3BT_Click(object sender, EventArgs e) => ValidateAndShow(txtV3.Text, new LeftPlatformRightTimeHandlerVerify());
        private void Version4BT_Click(object sender, EventArgs e) => ValidateAndShow(txtV4.Text, new RightTimeHandlerVerify());
        private void Version5BT_Click(object sender, EventArgs e) => ValidateAndShow(txtV5.Text, new TrainDynamicVerify());
        private void Version6BT_Click(object sender, EventArgs e) => ValidateAndShow(txtV6.Text, new UrgentHandlerVerify());
        private void Version7BT_Click(object sender, EventArgs e) => ValidateAndShow(txtV7.Text, new IdentifierStatusImageTopLeft24x48HandlerVerify());
        private void Version8BT_Click(object sender, EventArgs e) => ValidateAndShow(txtV8.Text, new StandardTimeBottomLeftHandlerVerify());

        private void ClearBT_Click(object sender, EventArgs e) => txtResult.Text = string.Empty;

        private void ValidateAndShow(string hexInput, PacketVerifyBase verifier)
        {
            var data = PacketBuilderService.ConvertHexStringToByteArray(hexInput);
            bool ok = verifier.ValidatePacket(data, out string errorMessage);
            txtResult.Text = ok ? "正確封包" : "錯誤封包\n" + errorMessage;
        }
    }
}
