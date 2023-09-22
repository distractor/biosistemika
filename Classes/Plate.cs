using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bs_plates.Classes
{
    public class Plate : PlateSetup
    {
        private readonly List<Well> _wells = new List<Well>();

        /// <summary>
        /// PLate class.
        /// </summary>
        /// <param name="paramsPath">Path to JSON. Empty string if input from terminal.</param>
        public Plate(string paramsPath)
        {
            // Get initialization parameters.
            GetInputs(paramsPath);
            // Print the inputs.
            PrintInputs();

            // Optimize setup.
            OptimizeSetup();
        }

        /// <summary>
        /// Gets input parameters.
        /// </summary>
        /// <param name="paramsPath">Path to JSON. Empty string if input from terminal.</param>
        private void GetInputs(string paramsPath)
        {
            if (paramsPath == string.Empty)
            {
                // Request input from user via terminal.
                // Read plate size.
                PlateSize = InputReader.GetInteger("Enter plate size: ", AllowedPlateSizes);
                // Read experiment names.
                Samples = InputReader.GetJsonString<List<List<string>>>("Enter sample names: ");
                // Read reagents.
                Reagents = InputReader.GetJsonString<List<List<string>>>("Enter reagents: ");
                // Read replications.
                Replications = InputReader.GetJsonString<List<int>>("Enter replication list: ");
                // Read max number of plates.
                MaxPlates = InputReader.GetInteger("Enter maximum number of plates: ", null);
            }
            else
            {
                // Read inputs from JSON.
                using (var r = new StreamReader(paramsPath))
                {
                    var jsonString = r.ReadToEnd();
                    dynamic jsonObject;
                    try
                    {
                        jsonObject = JObject.Parse(jsonString);
                    }
                    catch (JsonReaderException e)
                    {
                        Console.WriteLine("Invalid json. Error: '{0}'.", e.Message);
                        throw;
                    }

                    PlateSize = jsonObject.plate_size;
                    Samples = JsonConvert.DeserializeObject<List<List<string>>>(jsonObject.samples.ToString());
                    Reagents = JsonConvert.DeserializeObject<List<List<string>>>(jsonObject.reagents.ToString());
                    Replications = JsonConvert.DeserializeObject<List<int>>(jsonObject.replications.ToString());
                    MaxPlates = jsonObject.max_plates;
                }
            }
        }

        /// <summary>
        /// Prints input parameters.
        /// </summary>
        private void PrintInputs()
        {
            Console.WriteLine("Your input: ");
            Console.WriteLine("  - plate size: {0}", PlateSize);
            Console.WriteLine("  - sample names: {0}", JsonConvert.SerializeObject(Samples));
            Console.WriteLine("  - reagent names: {0}", JsonConvert.SerializeObject(Reagents));
            Console.WriteLine("  - replications: {0}", JsonConvert.SerializeObject(Replications));
            Console.WriteLine("  - maximum number of plates: {0}", MaxPlates);
        }

        /// <summary>
        /// Optimizes plate-well entries.
        /// </summary>
        private void OptimizeSetup()
        {
            Console.WriteLine("Optimizing ....");

            // Set plate size.
            var nRows = (PlateSize == 96) ? 8 : 12;
            var nCols = 2 * nRows;
            var wellMatrix = new Well[nRows, nCols];

            int row = 0, col = 0;

            for (var experimentIndex = 0; experimentIndex < Replications.Count; experimentIndex++)
            {
                var samples = Samples[experimentIndex];
                var reagents = Reagents[experimentIndex];
                var replicationSize = Replications[experimentIndex];

                // Iterate over all samples.
                foreach (var sampleName in samples)
                {
                    foreach (var reagentName in reagents)
                    {
                        for (var repCount = 0; repCount < replicationSize; repCount++)
                        {
                            var well = new Well(sampleName, reagentName, $"{repCount + 1}/{replicationSize}",
                                new Tuple<int, int>(row, col));

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

        /// <summary>
        /// Postprocessing. Prints out plate.
        /// </summary>
        /// <param name="wells">Matrix of wells.</param>
        private void Postprocess(Well[,] wells)
        {
            Console.WriteLine("Preprocessing ...");

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

        /// <summary>
        /// Prints plate entries.
        /// </summary>
        private void PrintPlate()
        {
            // Sort wells by rows and cols.
            var sortedWells = _wells.OrderBy(w => w.Location.Item1)
                .ThenBy(w => w.Location.Item2)
                .ToList();

            // Prints plate.
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