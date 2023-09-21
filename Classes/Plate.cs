using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace bs_plates.Classes
{
    public class Plate
    {
        private int _plateSize;
        private List<List<string>> _samples;
        private List<List<string>> _reagents;
        private List<int> _replications;
        private int _maxPlates;

        private static readonly List<int> AllowedPlateSizes = new List<int> { 96, 384 };
        private List<Well> _wells = new List<Well>();

        public Plate()
        {
            // Get initialization parameters.
            GetInputs();
            // Print the inputs.
            PrintInputs();

            // Optimize setup.
            Preprocess();
            OptimizeSetup();
        }

        /// <summary>
        /// Gets input parameters.
        /// </summary>
        private void GetInputs()
        {
            // Read plate size.
            // _plateSize = InputReader.GetInteger("Enter plate size: ", AllowedPlateSizes);
            _plateSize = 96;
            // Read experiment names.
            // _samples = InputReader.GetJsonString<List<List<string>>>("Enter sample names: ");
            _samples = new List<List<string>>
            {
                new List<string> { "Sample-1", "Sample-2", "Sample-3" },
                new List<string> { "Sample-1", "Sample-2", "Sample-3" }
            };

            // Read reagents.
            // _reagents = InputReader.GetJsonString<List<List<string>>>("Enter reagents: ");
            _reagents = new List<List<string>>
            {
                new List<string> { "pink" },
                new List<string> { "green" }
            };
            // Read replications.
            // _replications = InputReader.GetJsonString<List<int>>("Enter replication list: ");
            _replications = new List<int> { 3, 2 };
            // Read max number of plates.
            // _maxPlates = InputReader.GetInteger("Enter maximum number of plates: ", null);
            _maxPlates = 1;
        }

        /// <summary>
        /// Prints input parameters.
        /// </summary>
        private void PrintInputs()
        {
            Console.WriteLine("Your input: ");
            Console.WriteLine("  - plate size: {0}", _plateSize);
            Console.WriteLine("  - sample names: {0}", JsonConvert.SerializeObject(_samples));
            Console.WriteLine("  - reagent names: {0}", JsonConvert.SerializeObject(_reagents));
            Console.WriteLine("  - replications: {0}", JsonConvert.SerializeObject(_replications));
            Console.WriteLine("  - maximum number of plates: {0}", _maxPlates);
        }

        private void OptimizeSetup()
        {
            Console.WriteLine("Optimizing ....");

            // Set plate size.
            var nRows = (_plateSize == 96) ? 8 : 12;
            var nCols = 2 * nRows;
            var wellMatrix = new Well[nRows, nCols];

            int row = 0, col = 0;

            for (var experimentIndex = 0; experimentIndex < _replications.Count; experimentIndex++)
            {
                var samples = _samples[experimentIndex];
                var reagents = _reagents[experimentIndex];
                var replicationSize = _replications[experimentIndex];

                // Iterate over all samples.
                foreach (var sampleName in samples)
                {
                    foreach (var reagentName in reagents)
                    {
                        for (var repCount = 0; repCount < replicationSize; repCount++)
                        {
                            var well = new Well(sampleName, reagentName,
                                string.Format("{0}/{1}", repCount + 1, replicationSize), new Tuple<int, int>(row, col));

                            wellMatrix[row, col] = well;
                            col++;
                        }
                    }

                    row++;
                    col = 0;
                }
            }

            Postprocess(wellMatrix);
        }

        private void Preprocess()
        {
            Console.WriteLine("Preprocessing ...");
        }

        private void Postprocess(Well[,] wells)
        {
            Console.WriteLine("hI");
            for (var row = 0; row < wells.GetLength(0); row++)
            {
                for (int col = 0; col < wells.GetLength(1); col++)
                {
                    if (wells[row, col] != null)
                    {
                        _wells.Add(wells[row, col]);
                    }
                }
            }

            PrintPlate();
        }

        private void PrintPlate()
        {
            Console.WriteLine("Postprocessing ...");
            var sortedWells = _wells.OrderBy(w => w.Location.Item1)
                .ThenBy(w => w.Location.Item2)
                .ToList();

            for (var i = 0; i < sortedWells.Count; i++)
            {
                if (i > 0)
                {
                    if (sortedWells[i - 1].Location.Item1 != sortedWells[i].Location.Item1)
                    {
                        Console.Write("\n");
                    }
                }

                Console.Write("{0}; ", sortedWells[i]);
            }

            Console.Write("\n");
        }
    }
}