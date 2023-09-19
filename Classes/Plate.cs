using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace bs_plates.Classes
{
    public class Plate
    {
        private int _plateSize;
        private List<List<string>> _experiments;
        private List<List<string>> _reagents;
        private List<int> _replications;
        private int _maxPlates;

        private static readonly List<int> _allowedPlateSizes = new List<int> { 96, 384 };

        public Plate()
        {
            // Get initialization parameters.
            GetInputs();
            // Print the inputs.
            PrintInputs();

            // Optimize setup.
            OptimizeSetup();
        }

        /// <summary>
        /// Gets input parameters.
        /// </summary>
        private void GetInputs()
        {
            // Read plate size.
            _plateSize = InputReader.GetInteger("Enter plate size: ", _allowedPlateSizes);
            // Read experiment names.
            _experiments = InputReader.GetJsonString<List<List<string>>>("Enter experiment names: ");
            // Read reagents.
            _reagents = InputReader.GetJsonString<List<List<string>>>("Enter reagents: ");
            // Read replications.
            _replications = InputReader.GetJsonString<List<int>>("Enter replication list: ");
            // Read max number of plates.
            _maxPlates = InputReader.GetInteger("Enter maximum number of plates: ", null);
        }

        /// <summary>
        /// Prints input parameters.
        /// </summary>
        private void PrintInputs()
        {
            Console.WriteLine("Your input: ");
            Console.WriteLine("  - plate size: {0}", _plateSize);
            Console.WriteLine("  - experiment names: {0}", JsonConvert.SerializeObject(_experiments));
            Console.WriteLine("  - reagent names: {0}", JsonConvert.SerializeObject(_reagents));
            Console.WriteLine("  - replications: {0}", JsonConvert.SerializeObject(_replications));
            Console.WriteLine("  - maximum number of plates: {0}", _maxPlates);
        }

        private void OptimizeSetup()
        {
            Console.WriteLine("Optimizing ....");
        }
    }
}