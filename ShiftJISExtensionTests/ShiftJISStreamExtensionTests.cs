using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShiftJISExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftJISExtension.Tests
{
    [TestClass()]
    public class ShiftJISStreamExtensionTests
    {
        [TestMethod()]
        public void ToUTF8AsyncTest()
        {
            var sjis = File.ReadAllBytes(@"Data/shift_jis.txt");
            var utf8 = File.ReadAllBytes(@"Data/utf8.txt");
            var utf8bom = File.ReadAllBytes(@"Data/utf8-bom.txt");

            var cases = new[]
            {
                new { name = "sjis", input = sjis, expected = utf8, },
                new { name = "utf8", input = utf8, expected = utf8,},
                new { name = "utf8bom", input = utf8bom, expected = utf8bom,},
            };

            foreach (var c in cases)
            {
                using (var inputStream = new MemoryStream(c.input))
                using (var outputStream = inputStream.ToUTF8Async().Result)
                using (var ms = new MemoryStream())
                {
                    outputStream.CopyTo(ms);
                    var actual = (ms.ToArray());
                    var utf8Text = Encoding.UTF8.GetString(c.expected);
                    var actualText = Encoding.UTF8.GetString(actual);

                    Assert.AreEqual(utf8Text, actualText, $"{c.name} text equals");
                    Assert.IsTrue(c.expected.SequenceEqual(actual), $"{c.name} sequence equals");
                }
            }
        }
    }
}