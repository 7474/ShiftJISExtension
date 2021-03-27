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
            using (var sjisStream = new MemoryStream(sjis))
            using (var utf8Stream = sjisStream.ToUTF8Async().Result)
            using (var ms = new MemoryStream())
            {
                utf8Stream.CopyTo(ms);
                var sjis2utf = (ms.ToArray());
                var utf8Text = Encoding.UTF8.GetString(utf8);
                var sjis2utf8Text = Encoding.UTF8.GetString(sjis2utf);

                Assert.AreEqual(utf8Text, sjis2utf8Text);
                Assert.IsTrue(utf8.SequenceEqual(sjis2utf));
            }
        }
    }
}