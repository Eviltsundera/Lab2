using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Lab2
{
    public class V2DataMainCollection
    {
        public List<V2Data> DataItems;
        
        public interface IEnumerable
        {
            IEnumerable GetInumerator();
        }
        
        public IEnumerator<V2Data> GetEnumerator() {
            return DataItems.GetEnumerator();
        }
        
        public V2DataMainCollection() {
            DataItems = new List<V2Data>();
        }
        
        public int Count {
            get { return DataItems.Count; }
        }
        
        public void Add(V2Data item) {
            DataItems.Add(item);
        }
        
        public bool Remove(string id, double w) {
            int numRemovedItems = DataItems.RemoveAll(item => item.Info == id && item.FieldFrequency == w);
            return numRemovedItems > 0;
        }
        
        public override string ToString() {
            string res = "";
            foreach (var item in DataItems) {
                res += item.ToString() + "\n";
            }

            return res;
        }
        
        public string ToLongString() {
            string res = "";
            foreach (var item in DataItems) {
                res += item.ToLongString() + "\n";
            }

            return res;
        }
        
        public string ToLongString(string format) {
            string res = "";
            foreach (var item in DataItems) {
                res += item.ToLongString(format);
            }
            return res;
        }
        
        public void AddDefaults() {
            Random rnd = new Random();
            int num = 3 + rnd.Next(0, 7);
            int numOnGrid = rnd.Next(1, num - 1);
            int numCol = num - numOnGrid;
            double minVal = 1.0;
            double maxVal = 32.5;
            float maxValFloat = (float)100.0;

            for (int i = 0; i < numOnGrid; i++) {
                Grid1D tmp1 = new Grid1D((float)rnd.NextDouble() * maxValFloat, rnd.Next(0, 32));
                Grid1D tmp2 = new Grid1D((float)rnd.NextDouble() * maxValFloat, rnd.Next(0, 32));
                V2DataOnGrid objOnGrid = new V2DataOnGrid("new default V2DataOnGrid object", 
                    (float)rnd.NextDouble() * maxValFloat, tmp1, tmp2);
                objOnGrid.InitRandom(minVal, maxVal);
                DataItems.Add(objOnGrid);
            }

            for (int i = 0; i < numCol; i++) {
                V2DataCollection objCol = new V2DataCollection("new default V2DataCollection object", rnd.NextDouble() * 100.0);
                objCol.InitRandom(rnd.Next(1, 10), (float)rnd.NextDouble() * maxValFloat, 
                    (float)rnd.NextDouble() * maxValFloat, minVal, maxVal);
                DataItems.Add(objCol);
            }
        }
        
        public double AverageAbsValue {
            get
            {
                var fields = from v2data in DataItems from data in v2data select data.Field.Magnitude;
                return fields.Average();
            }
        }

        public DataItem NearestToAverage
        {
            get
            {
                double avg = this.AverageAbsValue;
                
                var fields = from v2data in DataItems from data in v2data select data;
                var query = from data in fields orderby Math.Abs(data.Field.Magnitude - avg) select data;
                return query.First();
            }
        }
        
        public IEnumerable<Vector2> AppearEveryWhere {
            get {
                var res = from x in DataItems from item in x group item by item.Coord into g where g.Count() == DataItems.Count select g.Key;
                return res;
            }
        }
    }
}