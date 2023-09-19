using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace bs_plates.Classes
{
    public class InputReader
    {
        /// <summary>
        /// Reads integer from the terminal
        /// </summary>
        /// <param name="enquiry">Enquiry message.</param>
        /// <param name="allowedValues">List of allowed values.</param>
        /// <returns>Integer.</returns>
        public static int GetInteger(string enquiry, List<int> allowedValues)
        {
            var input = string.Empty;
            var value = 0;

            // Get plate size. Repeat until valid entry.
            while (input == string.Empty)
            {
                Console.Write(enquiry);
                input = Console.ReadLine();
                try
                {
                    // Try convert to integer.
                    value = Convert.ToInt32(input);
                }
                catch
                {
                    input = string.Empty;
                }

                if (allowedValues == null) continue;
                if (allowedValues.Contains(value)) continue; // Check validity.
                Console.WriteLine("  Only integers {0} allowed.", JsonConvert.SerializeObject(allowedValues));
                input = string.Empty;
            }

            return value;
        }

        /// <summary>
        /// Get json string from terminal.
        /// </summary>
        /// <param name="enquiry">Enquiry message.</param>
        /// <typeparam name="T">Return type.</typeparam>
        /// <returns>Return type.</returns>
        public static T GetJsonString<T>(string enquiry)
        {
            // Get experiment names. Repeat until valid entry.
            var input = string.Empty;
            T value = default;
            while (input == string.Empty)
            {
                Console.Write(enquiry);
                input = Console.ReadLine();
                try
                {
                    // Try parse to list.
                    value = JsonConvert.DeserializeObject<T>(input);
                }
                catch
                {
                    Console.WriteLine("  Could not parse the JSON string. Try again.");
                    input = string.Empty;
                }
            }

            return value;
        }
    }
}