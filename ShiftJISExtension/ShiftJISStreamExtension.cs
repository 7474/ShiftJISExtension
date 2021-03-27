using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftJISExtension
{
    /// <summary>
    /// ShiftJIS コードの Stream を操作する拡張。
    /// </summary>
    public static class ShiftJISStreamExtension
    {
        /// <summary>
        /// source ストリームを Shift_JIS コードの文字列であるとみなして UTF8 コードの文字列ストリームを返す。
        /// source ストリームが UTF-8 コードだった場合 source と同じ内容のストリームを返す。
        /// いずれの場合も source ストリームは終端まで読み込まれた後破棄される。
        /// </summary>
        /// <param name="source">Shift_JIS コードの文字列ストリーム</param>
        /// <returns>source を UTF-8 コードに変換した文字列ストリーム</returns>
        public static async Task<Stream> ToUTF8Async(this Stream source)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var fromEnc = Encoding.GetEncoding(932);
            var outputMs = new MemoryStream();

            await source.ConvertEncodingAsync(outputMs, fromEnc, Encoding.UTF8);
            await source.DisposeAsync();

            outputMs.Position = 0;
            return outputMs;
        }

        public static async Task<bool> ConvertEncodingAsync(
                    this Stream source,
                    Stream destination,
                    Encoding fromEnc,
                    Encoding toEnc
                    )
        {
            byte[] converted;
            using (var inputMs = new MemoryStream())
            {
                await source.CopyToAsync(inputMs);
                var inputBytes = inputMs.ToArray();
                var inputStringFromEnc = fromEnc.GetString(inputBytes);
                var inputStringToEnc = toEnc.GetString(inputBytes);

                converted = toEnc.GetBytes(inputStringFromEnc);
                var isSjis = !inputBytes.SequenceEqual(toEnc.GetBytes(inputStringToEnc));
                if (isSjis)
                {
                    await destination.WriteAsync(converted, 0, converted.Length);
                }
                else
                {
                    inputMs.Position = 0;
                    await inputMs.CopyToAsync(destination);
                }
                return isSjis;
            }
        }
    }
}
