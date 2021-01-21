// <copyright file="CSV.cs" company="Hilres">
// Copyright (c) Hilres. All rights reserved.
// </copyright>

namespace Hilres.TiingoApi
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Comma Separated Value (CSV) class.
    /// - Each record is one line.
    /// - Fields are separated with commas.
    /// - Leading and trailing space-characters adjacent to comma field
    ///   separators are ignored.
    /// - Fields with embedded commas must be delimited with double-quote
    ///   characters.
    /// - Fields that contain double quote characters must be surrounded
    ///   by double-quotes, and the embedded double-quotes must each be
    ///   represented by a pair of consecutive double quotes.
    /// - A field that contains embedded line-breaks must be surrounded by
    ///   double-quotes.
    /// - Fields with leading or trailing spaces must be delimited with
    ///   double-quote characters.
    /// - Fields may always be delimited with double quotes.
    /// - The first record in a CSV file may be a header record containing
    ///   column (field) names.
    ///   Based on <!-- http://www.creativyst.com/Doc/Articles/CSV/CSV01.htm -->
    ///   Original created in 2005.
    /// </summary>
    public static class CSV
    {
        /// <summary>
        /// This will convert an array of objects into a single CSV line.
        /// </summary>
        /// <param name="values">A list of values.</param>
        /// <param name="fieldSeparator">The field separator character.</param>
        /// <param name="trimWhiteSpace">Trim() the fields of white spaces.</param>
        /// <param name="replace">Text to replace in a value.</param>
        /// <returns>CSV string.</returns>
        public static string Export(IEnumerable values, char fieldSeparator = ',', bool trimWhiteSpace = true, KeyValuePair<string, string>[] replace = null)
        {
            char[] escapeChars = new char[] { fieldSeparator, '"', '\n', '\r' };

            bool needComma = false;
            StringBuilder text = new StringBuilder();

            foreach (object item in values)
            {
                // Need a field separator?
                if (needComma)
                {
                    // Yes.
                    text.Append(fieldSeparator);
                }

                needComma = true;

                // Find out if the value is null.
                bool isNull = item == null;
                if (isNull == false)
                {
                    // Some class can say they are null.
                    isNull = item is INullable nullValue && nullValue.IsNull;
                }

                // Is the value null?
                if (isNull == false)
                {
                    // No.
                    string value = trimWhiteSpace ? item.ToString().Trim() : item.ToString();

                    // Are we replacing some text in the value?
                    if (replace != null)
                    {
                        // Yes.
                        foreach (KeyValuePair<string, string> replaceItem in replace)
                        {
                            value = value.Replace(replaceItem.Key, replaceItem.Value);
                        }
                    }

                    // Is this a zero length value?
                    if (value.Length > 0)
                    {
                        // No.
                        // Are there escape characters in the string?
                        if (value.IndexOfAny(escapeChars) >= 0)
                        {
                            // Yes.
                            AddQuotes(text, value);
                        }
                        else
                        {
                            // No.
                            // Are there white space at ether end of the string?
                            if (char.IsWhiteSpace(value[0])
                                || char.IsWhiteSpace(value[^1]))
                            {
                                // Yes.
                                AddQuotes(text, value);
                            }
                            else
                            {
                                // No.
                                text.Append(value);
                            }
                        }
                    }
                }
            }

            return text.ToString();
        }

        /// <summary>
        /// This will read from a TextReader that has CSV data and convert
        /// one data row to an array of strings.  Each time this is called,
        /// it will convert another data row.
        /// </summary>
        /// <param name="reader">A TextReader to read in one row of data.</param>
        /// <param name="fieldSeparator">The field separator character.</param>
        /// <param name="trimWhiteSpace">Trim() the fields of white spaces.</param>
        /// <param name="replace">Text to replace in a value.</param>
        /// <returns>string array of fields, or a null reference if done.</returns>
        public static string[] Import(TextReader reader, char fieldSeparator = ',', bool trimWhiteSpace = true, KeyValuePair<string, string>[] replace = null)
        {
            // Get the next line to work with.
            string text = reader.ReadLine();

            // Anything to process?
            if (text == null)
            {
                // No.
                return null;
            }

            List<string> fields = new List<string>();
            int textLength = text.Length;
            int idx = 0;

            while (idx < textLength)
            {
                // Skip over white space.
                while (idx < textLength && char.IsWhiteSpace(text, idx))
                {
                    idx++;
                }

                string value;

                // At end of string?
                if (idx < textLength)
                {
                    // No.
                    // Start of a quote?
                    if (text[idx] == '"')
                    {
                        // Yes.
                        idx++;

                        // Remember the left side.
                        int leftIdx = idx;
                        value = string.Empty;

                        while (idx < textLength)
                        {
                            // Hunt for the right quote.
                            idx = text.IndexOf('"', idx);

                            // Find it?
                            if (idx < 0)
                            {
                                // No.
                                // Save this part.
                                value += text[leftIdx..textLength] + Environment.NewLine;

                                // Move to next line.
                                text = reader.ReadLine();
                                while ((text != null) && (text.Length == 0))
                                {
                                    value += Environment.NewLine;
                                    text = reader.ReadLine();
                                }

                                textLength = text == null ? 0 : text.Length;
                                idx = 0;
                                leftIdx = 0;
                            }
                            else
                            {
                                // Yes.
                                // Are there two quotes in a row?
                                if ((idx + 1 < textLength) && (text[idx + 1] == '"'))
                                {
                                    // Yes.
                                    idx++;

                                    // Save what we have so far with one quote.
                                    value += text[leftIdx..idx];

                                    // Skip over the extra quote.
                                    idx++;

                                    // At end of string?
                                    if (idx >= textLength)
                                    {
                                        // Yes.
                                        value += Environment.NewLine;

                                        // Move to next line.
                                        text = reader.ReadLine();
                                        textLength = text.Length;
                                        idx = 0;
                                    }

                                    leftIdx = idx;
                                }
                                else
                                {
                                    // No.
                                    break;
                                }
                            }
                        }

                        if (text != null)
                        {
                            // Save the value.
                            value += text[leftIdx..idx];
                        }

                        idx++;
                        if (idx < textLength)
                        {
                            // Find the field separator.
                            idx = text.IndexOf(fieldSeparator, idx);
                            if (idx < 0)
                            {
                                idx = textLength;
                            }

                            idx++;
                        }
                    }
                    else
                    {
                        // No, just data.
                        int leftIdx = idx;
                        idx = text.IndexOf(fieldSeparator, idx);
                        if (idx < 0)
                        {
                            idx = textLength;
                        }

                        value = text[leftIdx..idx].Trim();
                        idx++;
                    }
                }
                else
                {
                    // Yes.
                    value = string.Empty;
                }

                value = trimWhiteSpace ? value.Trim() : value;

                // Are we replacing some text in the value?
                if (replace != null)
                {
                    // Yes.
                    foreach (KeyValuePair<string, string> replaceItem in replace)
                    {
                        value = value.Replace(replaceItem.Key, replaceItem.Value);
                    }
                }

                // Got a field, save it.
                fields.Add(value);
            }

            // Pickup trailing field separator.
            if (textLength > 0 && text[textLength - 1] == fieldSeparator)
            {
                fields.Add(string.Empty);
            }

            return fields.ToArray();
        }

        /// <summary>
        /// This will read from a TextReader that has CSV data and convert
        /// all the data row to an array of string arrays.  Each row will
        /// be a string array.
        /// </summary>
        /// <param name="reader">A TextReader to read in one row of data.</param>
        /// <param name="fieldSeparator">The field separator character.</param>
        /// <param name="trimWhiteSpace">Trim() the fields of white spaces.</param>
        /// <param name="replace">Text to replace in a value.</param>
        /// <returns>IEnumerable of string arrays.</returns>
        public static IEnumerable<string[]> ImportToArray(TextReader reader, char fieldSeparator = ',', bool trimWhiteSpace = true, KeyValuePair<string, string>[] replace = null)
        {
            string[] values = CSV.Import(reader, fieldSeparator, trimWhiteSpace, replace);
            while (values != null)
            {
                yield return values;
                values = CSV.Import(reader, fieldSeparator, trimWhiteSpace, replace);
            }
        }

        /// <summary>
        /// Append a string to a string builder.
        /// Put quotes around the string and change quotes in string to double quotes.
        /// If zero length string, no quotes.
        /// </summary>
        /// <param name="text">Place to put the text.</param>
        /// <param name="value">Value t put quotes around.</param>
        private static void AddQuotes(StringBuilder text, string value)
        {
            int valueLength = value.Length;

            // Do we have anything to parse?
            if (valueLength > 0)
            {
                // Yes.
                text.Append('"');

                int idx = 0;
                int leftIdx = 0;

                while (leftIdx < valueLength)
                {
                    idx = value.IndexOf('"', idx);

                    // Did we find a quote?
                    if (idx >= 0)
                    {
                        // Yes.
                        int leftLength = idx - leftIdx;
                        if (leftLength > 0)
                        {
                            text.Append(value.Substring(leftIdx, leftLength));
                        }

                        text.Append("\"\"");
                        idx++;
                        leftIdx = idx;
                    }
                    else
                    {
                        // No.
                        text.Append(value[leftIdx..valueLength]);
                        break;
                    }
                }

                text.Append('"');
            }
        }
    }
}