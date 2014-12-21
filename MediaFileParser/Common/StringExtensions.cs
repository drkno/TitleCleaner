using System.Collections.Generic;

namespace MediaFileParser.Common
{
    /// <summary>
    /// Contains extensions to the standard string class used within the library.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets a string between two matching brackets.
        /// </summary>
        /// <param name="input">Input string to get substring from.</param>
        /// <param name="openBracket">Index of opening bracket.</param>
        /// <returns>Resulting string from inside the brackets. If no matching bracket is found 
        /// the output value will be null.</returns>
        public static string GetBracketedString(this string input, int openBracket)
        {
            Dictionary<string, string> lookupDictionary = null;
            return GetBracketedString(input, openBracket, ref lookupDictionary);
        }

        /// <summary>
        /// Gets a string between two matching brackets.
        /// </summary>
        /// <param name="input">Input string to get substring from.</param>
        /// <param name="openBracket">Index of opening bracket.</param>
        /// <param name="lookupDictionary">Dictionary to use for caching repetitive lookups.</param>
        /// <returns>Resulting string from inside the brackets. If no matching bracket is found 
        /// the output value will be null.</returns>
        public static string GetBracketedString(this string input, int openBracket, ref Dictionary<string, string> lookupDictionary)
        {
            var lookup = input.Substring(openBracket);
            if (lookupDictionary != null && lookupDictionary.ContainsKey(lookup))
            {
                return lookupDictionary[lookup];
            }

            string output;
            var ind1 = openBracket + 1;
            var stack = new Stack<char>();
            stack.Push(input[openBracket]);
            for (; ind1 < input.Length; ind1++)
            {
                if (stack.Count == 0)
                {
                    ind1--;
                    break;
                }
                switch (input[ind1])
                {
                    case '(':
                        {
                            stack.Push(input[ind1]);
                            break;
                        }
                    case ')':
                        {
                            stack.Pop();
                            break;
                        }
                }
            }

            if (stack.Count == 0)
            {
                output = input.Substring(openBracket + 1, ind1 - openBracket - 1);
                if (lookupDictionary != null)
                {
                    lookupDictionary[lookup] = output;
                }
            }
            else
            {
                output = null;
            }

            return output;
        }
    }
}
