# ShiftJISExtension

[![NuGet version](https://badge.fury.io/nu/ShiftJISExtension.svg)](https://badge.fury.io/nu/ShiftJISExtension)

.NET で `Shift_JIS` コードの文字列を扱うための拡張です。
エンコーディングには `CP932` を指定しています。

## 使い方

`Stream` の `ToUTF8Async` 拡張メソッドを呼び出すと `Sjift_JIS` から `UTF-8` に変換されます。もともとUTF-8だった場合はそのままUTF-8です。

例はイメージです。

```cs
using System.IO;
using ShiftJISExtension;

using (var utf8stream = await File.Open("sjis.txt").ToUTF8Async())
using (var reader = new StreamReader(utf8stream))
{
    // ...
}
```
