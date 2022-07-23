/*
 * Copyright (c) 2013 Calvin Rien
 *
 * Based on the JSON parser by Patrick van Bergen
 * http://techblog.procurios.nl/k/618/news/view/14605/14863/How-do-I-write-my-own-parser-for-JSON.html
 *
 * Simplified it so that it doesn't throw exceptions
 * and can be used in Unity iPhone with maximum code stripping.
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Forked from  https://github.com/Jackyjjc/MiniJSON.cs
// version: 6de00beb134bbab9d873033a48b32e4067ed0c25

namespace Utilities
{
    /// <summary>
    /// This class encodes and decodes JSON strings.
    /// Spec. details, see http://www.json.org/
    ///
    /// JSON uses Arrays and Objects. These correspond here to the datatypes IList and IDictionary.
    /// All numbers are parsed to doubles.
    /// </summary>
    public static class Json {
        /// <summary>
        /// Parses the string json into a value
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false</returns>
        public static object Deserialize(string json) {
            // save the string for debug information
            if (json == null) {
                return null;
            }

            return Parser.Parse(json);
        }

        private sealed class Parser : IDisposable {
            private const string k_wordBreak = "{}[],:\"";

            public static bool IsWordBreak(char c) {
                return Char.IsWhiteSpace(c) || k_wordBreak.IndexOf(c) != -1;
            }

            private enum Token {
                None,
                CurlyOpen,
                CurlyClose,
                SquaredOpen,
                SquaredClose,
                Colon,
                Comma,
                String,
                Number,
                True,
                False,
                Null
            };

            private StringReader m_json;

            private Parser(string jsonString) {
                m_json = new StringReader(jsonString);
            }

            public static object Parse(string jsonString) {
                using (var instance = new Parser(jsonString)) {
                    return instance.ParseValue();
                }
            }

            public void Dispose() {
                m_json.Dispose();
                m_json = null;
            }

            private Dictionary<string, object> ParseObject() {
                Dictionary<string, object> table = new Dictionary<string, object>();

                // ditch opening brace
                m_json.Read();

                while (true) {
                    switch (NextToken) {
                    case Token.None:
                        return null;
                    case Token.Comma:
                        continue;
                    case Token.CurlyClose:
                        return table;
                    default:
                        // name
                        string name = ParseString();
                        if (name == null) {
                            return null;
                        }

                        // :
                        if (NextToken != Token.Colon) {
                            return null;
                        }
                        // ditch the colon
                        m_json.Read();

                        // value
                        table[name] = ParseValue();
                        break;
                    }
                }
            }

            private List<object> ParseArray() {
                List<object> array = new List<object>();

                // ditch opening bracket
                m_json.Read();

                // [
                var parsing = true;
                while (parsing) {
                    Token nextToken = NextToken;

                    switch (nextToken) {
                    case Token.None:
                        return null;
                    case Token.Comma:
                        continue;
                    case Token.SquaredClose:
                        parsing = false;
                        break;
                    default:
                        object value = ParseByToken(nextToken);

                        array.Add(value);
                        break;
                    }
                }

                return array;
            }

            private object ParseValue() {
                Token nextToken = NextToken;
                return ParseByToken(nextToken);
            }

            private object ParseByToken(Token token) {
                switch (token) {
                case Token.String:
                    return ParseString();
                case Token.Number:
                    return ParseNumber();
                case Token.CurlyOpen:
                    return ParseObject();
                case Token.SquaredOpen:
                    return ParseArray();
                case Token.True:
                    return true;
                case Token.False:
                    return false;
                case Token.Null:
                    return null;
                default:
                    return null;
                }
            }

            private string ParseString() {
                StringBuilder s = new StringBuilder();
                char c;

                // ditch opening quote
                m_json.Read();

                bool parsing = true;
                while (parsing) {

                    if (m_json.Peek() == -1) {
                        break;
                    }

                    c = NextChar;
                    switch (c) {
                    case '"':
                        parsing = false;
                        break;
                    case '\\':
                        if (m_json.Peek() == -1) {
                            parsing = false;
                            break;
                        }

                        c = NextChar;
                        switch (c) {
                        case '"':
                        case '\\':
                        case '/':
                            s.Append(c);
                            break;
                        case 'b':
                            s.Append('\b');
                            break;
                        case 'f':
                            s.Append('\f');
                            break;
                        case 'n':
                            s.Append('\n');
                            break;
                        case 'r':
                            s.Append('\r');
                            break;
                        case 't':
                            s.Append('\t');
                            break;
                        case 'u':
                            var hex = new char[4];

                            for (int i=0; i< 4; i++) {
                                hex[i] = NextChar;
                            }

                            s.Append((char) Convert.ToInt32(new string(hex), 16));
                            break;
                        default:
                            s.Append(c);
                            break;
                        }
                        break;
                    default:
                        s.Append(c);
                        break;
                    }
                }

                return s.ToString();
            }

            private object ParseNumber() {
                string number = NextWord;

                if (number.IndexOf('.') == -1 && number.IndexOf('E') == -1 && number.IndexOf('e') == -1) {
                    long parsedInt;
                    Int64.TryParse(number, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out parsedInt);
                    return parsedInt;
                }

                double parsedDouble;
                Double.TryParse(number, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out parsedDouble);
                return parsedDouble;
            }

            private void EatWhitespace() {
                while (Char.IsWhiteSpace(PeekChar)) {
                    m_json.Read();

                    if (m_json.Peek() == -1) {
                        break;
                    }
                }
            }

            private char PeekChar {
                get {
                    return Convert.ToChar(m_json.Peek());
                }
            }

            private char NextChar {
                get {
                    return Convert.ToChar(m_json.Read());
                }
            }

            private string NextWord {
                get {
                    StringBuilder word = new StringBuilder();

                    while (!IsWordBreak(PeekChar)) {
                        word.Append(NextChar);

                        if (m_json.Peek() == -1) {
                            break;
                        }
                    }

                    return word.ToString();
                }
            }

            private Token NextToken {
                get {
                    EatWhitespace();

                    if (m_json.Peek() == -1) {
                        return Token.None;
                    }

                    switch (PeekChar) {
                    case '{':
                        return Token.CurlyOpen;
                    case '}':
                        m_json.Read();
                        return Token.CurlyClose;
                    case '[':
                        return Token.SquaredOpen;
                    case ']':
                        m_json.Read();
                        return Token.SquaredClose;
                    case ',':
                        m_json.Read();
                        return Token.Comma;
                    case '"':
                        return Token.String;
                    case ':':
                        return Token.Colon;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                        return Token.Number;
                    default:
                        switch (NextWord) {
                            case "false":
                                return Token.False;
                            case "true":
                                return Token.True;
                            case "null":
                                return Token.Null;
                            default:
                                return Token.None;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
        /// </summary>
        /// <param name="json">A Dictionary&lt;string, object&gt; / List&lt;object&gt;</param>
        /// <param name="humanReadable">Whether output as human readable format with spaces and
        /// indentations.</param>
        /// <param name="indentSpaces">Number of spaces for each level of indentation.</param>
        /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
        public static string Serialize(object obj,
                                       bool humanReadable = false,
                                       int indentSpaces = 2) {
            return Serializer.MakeSerialization(obj, humanReadable, indentSpaces);
        }

        private sealed class Serializer {
            private readonly StringBuilder m_builder;
            private readonly bool          m_humanReadable;
            private readonly int           m_indentSpaces;
            private          int           m_indentLevel;

            private Serializer(bool humanReadable, int indentSpaces) {
                m_builder = new StringBuilder();
                this.m_humanReadable = humanReadable;
                this.m_indentSpaces = indentSpaces;
                m_indentLevel = 0;
            }

            public static string MakeSerialization(object obj, bool humanReadable, int indentSpaces) {
                var instance = new Serializer(humanReadable, indentSpaces);

                instance.SerializeValue(obj);

                return instance.m_builder.ToString();
            }

            private void SerializeValue(object value) {
                IList asList;
                IDictionary asDict;
                string asStr;

                if (value == null) {
                    m_builder.Append("null");
                } else if ((asStr = value as string) != null) {
                    SerializeString(asStr);
                } else if (value is bool) {
                    m_builder.Append((bool) value ? "true" : "false");
                } else if ((asList = value as IList) != null) {
                    SerializeArray(asList);
                } else if ((asDict = value as IDictionary) != null) {
                    SerializeObject(asDict);
                } else if (value is char) {
                    SerializeString(new string((char) value, 1));
                } else {
                    SerializeOther(value);
                }
            }

            private void AppendNewLineFunc() {
                m_builder.AppendLine();
                m_builder.Append(' ', m_indentSpaces * m_indentLevel);
            }

            private void SerializeObject(IDictionary obj) {
                bool first = true;

                m_builder.Append('{');
                ++m_indentLevel;

                foreach (object e in obj.Keys) {
                    if (first) {
                        if (m_humanReadable) AppendNewLineFunc();
                    } else {
                        m_builder.Append(',');
                        if (m_humanReadable) AppendNewLineFunc();
                    }

                    SerializeString(e.ToString());
                    m_builder.Append(':');
                    if (m_humanReadable) m_builder.Append(' ');

                    SerializeValue(obj[e]);

                    first = false;
                }

                --m_indentLevel;
                if (m_humanReadable && obj.Count > 0) AppendNewLineFunc();

                m_builder.Append('}');
            }

            private void SerializeArray(IList anArray) {
                m_builder.Append('[');
                ++m_indentLevel;

                bool first = true;

                for (int i=0; i<anArray.Count; i++) {
                    object obj = anArray[i];
                    if (first) {
                        if (m_humanReadable) AppendNewLineFunc();
                    } else {
                        m_builder.Append(',');
                        if (m_humanReadable) AppendNewLineFunc();
                    }

                    SerializeValue(obj);

                    first = false;
                }

                --m_indentLevel;
                if (m_humanReadable && anArray.Count > 0) AppendNewLineFunc();

                m_builder.Append(']');
            }

            private void SerializeString(string str) {
                m_builder.Append('\"');

                char[] charArray = str.ToCharArray();
                for (int i=0; i<charArray.Length; i++) {
                    char c = charArray[i];
                    switch (c) {
                    case '"':
                        m_builder.Append("\\\"");
                        break;
                    case '\\':
                        m_builder.Append("\\\\");
                        break;
                    case '\b':
                        m_builder.Append("\\b");
                        break;
                    case '\f':
                        m_builder.Append("\\f");
                        break;
                    case '\n':
                        m_builder.Append("\\n");
                        break;
                    case '\r':
                        m_builder.Append("\\r");
                        break;
                    case '\t':
                        m_builder.Append("\\t");
                        break;
                    default:
                        int codepoint = Convert.ToInt32(c);
                        if ((codepoint >= 32) && (codepoint <= 126)) {
                            m_builder.Append(c);
                        } else {
                            m_builder.Append("\\u");
                            m_builder.Append(codepoint.ToString("x4"));
                        }
                        break;
                    }
                }

                m_builder.Append('\"');
            }

            private void SerializeOther(object value) {
                // NOTE: decimals lose precision during serialization.
                // They always have, I'm just letting you know.
                // Previously floats and doubles lost precision too.
                if (value is float) {
                    m_builder.Append(((float) value).ToString("R", System.Globalization.CultureInfo.InvariantCulture));
                } else if (value is int
                    || value is uint
                    || value is long
                    || value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is ulong) {
                    m_builder.Append(value);
                } else if (value is double
                    || value is decimal) {
                    m_builder.Append(Convert.ToDouble(value).ToString("R", System.Globalization.CultureInfo.InvariantCulture));
                } else {
                    SerializeString(value.ToString());
                }
            }
        }
    }
}
