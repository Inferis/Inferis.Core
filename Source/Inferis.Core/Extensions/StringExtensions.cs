using System;
using System.Text;

namespace Inferis.Core.Extensions
{
    public static class StringExtensions
    {
        public static string StripNonValidXmlCharacters(this string source)
        {
            var result = new StringBuilder(); // Used to hold the output.

            if (string.IsNullOrEmpty(source)) return source; // vacancy test.
            foreach (var current in source) {
                if ((current == 0x9) ||
                    (current == 0xA) ||
                    (current == 0xD) ||
                    ((current >= 0x20) && (current <= 0xD7FF)) ||
                    ((current >= 0xE000) && (current <= 0xFFFD)) ||
                    ((current >= 0x10000) && (current <= 0x10FFFF)))
                    result.Append(current);
            }
            return result.ToString();
        }

        public static string MD5(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;

            var x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(source);
            bs = x.ComputeHash(bs);
            var s = new StringBuilder();
            foreach (var b in bs) {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }

        public static string ToUnderscoreCase(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            var result = new StringBuilder((int)(source.Length * 1.5));
            foreach (var c in source) {
                if (char.IsUpper(c)) {
                    if (result.Length > 0)
                        result.Append('_');
                    result.Append(char.ToLower(c));
                }
                else
                    result.Append(c);
            }
            return result.ToString();
        }

        public static string ToUpperFirst(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            return string.Concat(
                source.Substring(0, 1).ToUpper(),
                source.Substring(1));
        }

        public static string ToLowerFirst(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            return string.Concat(
                source.Substring(0, 1).ToLower(),
                source.Substring(1));
        }

        public static string StripAllTags(this string html)
        {
            var array = new char[html.Length];
            var arrayIndex = 0;
            var inside = false;

            foreach (var let in html) {
                if (let == '<') {
                    inside = true;
                    continue;
                }
                if (let == '>') {
                    inside = false;
                    continue;
                }
                if (inside) continue;
                array[arrayIndex] = let;
                arrayIndex++;
            }
            return new string(array, 0, arrayIndex);
        }

        public static string CamelCaseToWords(this string value)
        {
            var pos = 0;
            var result = new StringBuilder();
            while (pos < value.Length) {
                var nextpos = pos + 1;
                while (nextpos < value.Length &&
                    (!char.IsUpper(value[nextpos]) || char.IsUpper(value[nextpos - 1])))
                    nextpos++;

                if (char.IsUpper(value[nextpos - 1]) && char.IsUpper(value[nextpos]))
                    nextpos--;

                if (nextpos > pos && nextpos <= value.Length) {
                    result.Append(value.Substring(pos, nextpos - pos));
                    if (!char.IsWhiteSpace(value[nextpos - 1]) && nextpos < value.Length)
                        result.Append(" ");
                }
                pos = nextpos;
            }

            return result.ToString();
        }

        /// <summary>
        /// Performs urlencoding on a string.
        /// </summary>
        /// <param name="source">The original string.</param>
        /// <returns>The urlencoded string.</returns>
        public static string UrlEncode(this string source)
        {
            return source == null ? null : UrlEncode(source, Encoding.UTF8);
        }

        /// <summary>
        /// Performs urlencoding on a string.
        /// </summary>
        /// <param name="source">The original string.</param>
        /// <returns>The urlencoded string.</returns>
        public static string UrlEncode(string source, Encoding encoding)
        {
            if (source == null)
                return null;

            var result = UrlEncode(encoding.GetBytes(source));
            return Encoding.UTF8.GetString(result, 0, result.Length);
        }

        /// <summary>
        /// Performs urlencoding on a series of bytes (representing a string).
        /// </summary>
        /// <param name="bytes">The original string as bytes.</param>
        /// <returns>The urlencoded string as bytes.</returns>
        private static byte[] UrlEncode(byte[] bytes)
        {
            Func<char, bool> isSafe = ch => {
                if ((((ch >= 'a') && (ch <= 'z')) || ((ch >= 'A') && (ch <= 'Z'))) ||
                    ((ch >= '0') && (ch <= '9'))) {
                    return true;
                }

                return !("'()*-_.!".Contains(ch.ToString()));
            };

            // Check if we actually need to encode the string: check for unsafe characters and spaces.
            var numSpaces = 0;
            var numUnsafe = 0;
            var count = bytes.Length;
            for (var i = 0; i < count; i++) {
                var ch = (char)bytes[i];
                if (ch == ' ')
                    numSpaces++;
                else if (!isSafe(ch))
                    numUnsafe++;
            }

            // Couldn't find any, so skip the encoding.
            if (numSpaces == 0 && numUnsafe == 0)
                return bytes;

            Func<int, byte> hexify = n => n <= 9 ? (byte)(char)(n + 0x30) : (byte)(char)((n - 10) + 0x61);
            var result = new byte[count + (numUnsafe * 2)];
            var resultIndex = 0;
            for (var j = 0; j < count; j++) {
                var ch = bytes[j];
                if (isSafe((char)ch))
                    result[resultIndex++] = bytes[j];
                else if ((char)ch == ' ')
                    result[resultIndex++] = 0x2b;
                else {
                    result[resultIndex++] = 0x25;
                    result[resultIndex++] = hexify((ch >> 4) & 15);
                    result[resultIndex++] = hexify(ch & 15);
                }
            }
            return result;
        }

        /// <summary>
        /// Performs urlencoding on a string.
        /// </summary>
        /// <param name="source">The original string.</param>
        /// <returns>The urlencoded string.</returns>
        public static string UrlDecode(this string source)
        {
            return source == null ? null : UrlDecode(source, Encoding.UTF8);
        }

        /// <summary>
        /// Performs urlencoding on a string.
        /// </summary>
        /// <param name="source">The original string.</param>
        /// <returns>The urlencoded string.</returns>
        public static string UrlDecode(string source, Encoding encoding)
        {
            if (source == null)
                return null;

            Func<char, int> unhexify = h => {
                if ((h >= '0') && (h <= '9')) return (h - '0');
                if ((h >= 'a') && (h <= 'f')) return ((h - 'a') + 10);
                if ((h >= 'A') && (h <= 'F')) return ((h - 'A') + 10);
                return -1;
            };

            var length = source.Length;
            UrlDecoder decoder = new UrlDecoder(length, encoding);
            for (var i = 0; i < length; i++) {
                var ch = source[i];
                if (ch == '+') {
                    ch = ' ';
                }
                else if ((ch == '%') && (i < (length - 2))) {
                    if ((source[i + 1] == 'u') && (i < (length - 5))) {
                        var unicode1 = unhexify(source[i + 2]);
                        var unicode2 = unhexify(source[i + 3]);
                        var unicode3 = unhexify(source[i + 4]);
                        var unicode4 = unhexify(source[i + 5]);
                        if (((unicode1 < 0) || (unicode2 < 0)) || ((unicode3 < 0) || (unicode4 < 0))) {
                            if ((ch & 0xff80) == 0) {
                                decoder.AddByte((byte)ch);
                            }
                            else {
                                decoder.AddChar(ch);
                            }
                        }
                        ch = (char)((((unicode1 << 12) | (unicode2 << 8)) | (unicode3 << 4)) | unicode4);
                        i += 5;
                        decoder.AddChar(ch);
                        continue;
                    }

                    var plain1 = unhexify(source[i + 1]);
                    var plain2 = unhexify(source[i + 2]);
                    if ((plain1 >= 0) && (plain2 >= 0)) {
                        byte b = (byte)((plain1 << 4) | plain2);
                        i += 2;
                        decoder.AddByte(b);
                        continue;
                    }
                }

                if ((ch & 0xff80) == 0) {
                    decoder.AddByte((byte)ch);
                }
                else {
                    decoder.AddChar(ch);
                }
            }

            return decoder.GetString();
        }

        private class UrlDecoder {
            // Fields
            private readonly int bufferSize;
            private byte[] byteBuffer;
            private readonly char[] charBuffer;
            private readonly Encoding encoding;
            private int numBytes;
            private int numChars;

            internal UrlDecoder(int bufferSize, Encoding encoding)
            {
                this.bufferSize = bufferSize;
                this.encoding = encoding;
                charBuffer = new char[bufferSize];
            }

            internal void AddByte(byte b)
            {
                if (byteBuffer == null) {
                    byteBuffer = new byte[bufferSize];
                }
                byteBuffer[numBytes++] = b;
            }

            internal void AddChar(char ch)
            {
                if (numBytes > 0) FlushBytes();
                charBuffer[numChars++] = ch;
            }

            private void FlushBytes()
            {
                if (numBytes <= 0) return;
                numChars += encoding.GetChars(byteBuffer, 0, numBytes, charBuffer, numChars);
                numBytes = 0;
            }

            internal string GetString()
            {
                if (numBytes > 0) FlushBytes();
                return numChars > 0 ? new string(charBuffer, 0, numChars) : string.Empty;
            }
        }
    }
}