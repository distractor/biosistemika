using System;
using System.Runtime.InteropServices;

namespace bs_plates.Classes
{
    public class Well
    {
        public string Sample;
        public string Reagent;
        public string ReplicationId;
        public Tuple<int, int> Location;

        public Well()
        {
            Sample = null;
            Reagent = null;
            ReplicationId = null;
            Location = null;
        }

        public Well(string sampleName, string reagentName, string replicationString, Tuple<int, int> plateLocation)
        {
            Sample = sampleName;
            Reagent = reagentName;
            ReplicationId = replicationString;
            Location = plateLocation;
        }

        public override string ToString()
        {
            var text = string.Format("{0},{1},{2},({3},{4})", Sample, Reagent, ReplicationId, Location.Item1,
                Location.Item2);
            return text;
        }

        public void setLocation(Tuple<int, int> loc)
        {
            Location = loc;
        }
    }
}