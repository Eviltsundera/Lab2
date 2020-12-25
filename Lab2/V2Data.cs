using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace Lab2
{
    public abstract class V2Data : IEnumerable<DataItem>
    {
        public string Info { get; set; }
        public double FieldFrequency { get; set; }

        public V2Data()
        {
            Info = "";
            FieldFrequency = 0;
        }

        public V2Data(string info, double freq)
        {
            Info = info;
            FieldFrequency = freq;
        }
        
        public abstract Complex[] NearAverage(float eps);
        public abstract string ToLongString();

        public override string ToString()
        {
            return $"{Info}\nField frequency is {FieldFrequency}";
        }
        
        public abstract string ToLongString(string format);
        public abstract IEnumerator<DataItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}