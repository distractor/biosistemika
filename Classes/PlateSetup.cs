using System.Collections.Generic;

namespace bs_plates.Classes
{
    public class PlateSetup
    {
        protected int PlateSize;
        protected List<List<string>> Samples;
        protected List<List<string>> Reagents;
        protected List<int> Replications;
        protected int MaxPlates;

        protected readonly List<int> AllowedPlateSizes = new List<int> { 96, 384 };
    }
}