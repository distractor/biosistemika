using System;
using System.IO;
using bs_plates.Classes;

namespace Biosistemika_plates
{
    internal class Program
    {
        /**
         * Initialization function that prints basic data to console and starts timer.
         */
        private static DateTime Initialize()
        {
            var now = DateTime.UtcNow; // UTC time now.

            // Print execution start time to console.
            var msg = string.Format("Execution started on {0:d} at {0:t}.", now);
            Console.WriteLine(msg);

            // Print separator (for clarity) to console.
            Console.WriteLine("-----------------------------------------------");

            return now;
        }

        /**
         * Finalize function that concludes te execution.
         * Total duration is computed and printed to console.
         */
        private static void Finalize(DateTime utcStart)
        {
            var now = DateTime.UtcNow; // UTC time now.

            // Print separator to console (for clarity).
            Console.WriteLine("-----------------------------------------------");

            // Print end time to console.
            var msg = string.Format("Execution finished on {0:d} at {0:t}.", now);
            Console.WriteLine(msg);

            // Print total duration to console.
            msg = string.Format("Duration: {0}.", (now - utcStart).Duration());
            Console.WriteLine(msg);
        }

        public static void Main(string[] args)
        {
            // Initialize.
            var utcStart = Initialize();

            // Parse run arguments.
            var jsonPath = string.Empty;
            if (args.Length > 0)
            {
                jsonPath = args[0];
                if (!File.Exists(jsonPath))
                {
                    throw new FileNotFoundException("JSON file '{0}' not found.", jsonPath);
                }
            }

            // Main logic.
            var plate = new Plate(jsonPath);

            // Finalize.
            Finalize(utcStart);
        }
    }
}