using System;
using System.Globalization;
using Eto.Forms;
using OpenTabletDriver.UX.Controls.Generic.Text.Providers;

namespace OpenTabletDriver.UX.Controls.Generic.Text
{
    public class HexByteArrayBox : MaskedTextBox<byte[]>
    {
        public HexByteArrayBox()
        {
            Provider = new HexByteArrayTextProvider();
        }

        private class HexByteArrayTextProvider : HexArrayTextProvider<byte[]>
        {
            public override byte[] Value
            {
                set => Text = ToHexString(value);
                get => ToByteArray(Text);
            }

            protected override bool Validate(string str)
            {
                if (string.IsNullOrWhiteSpace(str) || str == "0" || str == "0x")
                    return true;
                return base.Validate(str) && TryGetHexValue(str, out _);
            }

            private bool TryGetHexValue(string str, out byte value) => byte.TryParse(str.Replace("0x", string.Empty), NumberStyles.HexNumber, null, out value);

            private string ToHexString(byte[] value)
            {
                if (value is byte[] array)
                    return "0x" + BitConverter.ToString(array).Replace("-", " 0x") ?? string.Empty;
                else
                    return string.Empty;
            }
            
            private byte[] ToByteArray(string hex)
            {
                var raw = hex.Split(' ');
                byte[] buffer = new byte[raw.Length];
                for (int i = 0; i < raw.Length; i++)
                {
                    if (TryGetHexValue(raw[i], out var val))
                        buffer[i] = val;
                    else
                        return null;
                }
                return buffer;
            }
        }
    }
}