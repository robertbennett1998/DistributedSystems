using System;
using System.Collections.Generic;
using System.Text;

namespace DistSysACW.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ConvertHexStringToBytes(this string hexString)
        {
            List<byte> bytes = new List<byte>();
            foreach (var hexValue in hexString.Split('-'))
            {
                bytes.Add(Convert.ToByte(hexValue, 16));
            }

            return bytes.ToArray();
        }
    }
}
