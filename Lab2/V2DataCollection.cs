using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Lab2
{
    class V2DataCollection : V2Data
    {
        public List<DataItem> DataItems { get; set; }
        
        public V2DataCollection(string x, double y) : base(x, y) 
        {
            DataItems = new List<DataItem>();
        }

        public void InitRandom(int nItems, float xmax, float ymax, double minValue, double maxValue)
        {
            Random rnd = new Random();

            for (int i = 0; i < nItems; ++i)
            {
                float x = (float)rnd.NextDouble() * xmax;
                float y = (float)rnd.NextDouble() * ymax;
                Vector2 vec = new Vector2(x, y);
                
                Complex z = new Complex(rnd.NextDouble() * (maxValue - minValue) + minValue,
                    rnd.NextDouble() * (maxValue - minValue) + minValue);
                
                DataItems.Add(new DataItem(vec, z));
            }
        }

        public override Complex[] NearAverage(float eps)
        {
            double average = 0;
            for (int i = 0; i < DataItems.Count; ++i)
            {
                average += DataItems[i].Field.Real;
            }

            average /= DataItems.Count;

            int counter = 0;
            
            for (int i = 0; i < DataItems.Count; ++i)
            {
                if (Math.Abs(average - DataItems[i].Field.Real) < eps)
                {
                    ++counter;
                }
            }
            
            Complex[] answer = new Complex[counter];
            counter = 0;
            
            for (int i = 0; i < DataItems.Count; ++i)
            {
                if (Math.Abs(average - DataItems[i].Field.Real) < eps)
                {
                    answer[counter] = DataItems[i].Field;
                    ++counter;
                }
            }

            return answer;
        }

        public override string ToString()
        {
            return $"V2DataCollection\n" + base.ToString() + $"\nNumber of elements: {DataItems.Count}\n";
        }

        public override string ToLongString()
        {
            string answer = this.ToString();

            for (int i = 0; i < DataItems.Count; ++i)
            {
                answer += $"Point({DataItems[i].Coord.X}, {DataItems[i].Coord.Y}) Field = {DataItems[i].Field}\n";
            }

            return answer;
        }
        
        public override string ToLongString(string format) {
            string res = $"V2DataCollection\nInfo = {Info}\nField frequency = {FieldFrequency.ToString(format)}\n" +
                         $"Number of elems in the List<DataItem> = {DataItems.Count}\n";
            
            foreach (var item in DataItems) {
                res += item.ToString(format);
            }

            return res;
        }

        public V2DataCollection(string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                StreamReader streamReader = new StreamReader(fs);

                Info = streamReader.ReadLine();
                string tmp = streamReader.ReadLine();
                FieldFrequency = float.Parse(tmp);
                DataItems = new List<DataItem>();

                string buffer = streamReader.ReadLine();

                while (buffer != null)
                {
                    string[] lst = buffer.Split(' ');
                    
                    Vector2 point = new Vector2(float.Parse(lst[0]), float.Parse(lst[1]));
                    Complex field = new Complex(Convert.ToDouble(lst[2]), Convert.ToDouble(lst[3]));
                    DataItem new_obj = new DataItem(point, field);
                    
                    DataItems.Add(new_obj);
                    
                    buffer = streamReader.ReadLine();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
                if (fs != null) {
                    fs.Close();
                }
            }
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            return DataItems.GetEnumerator();
        }
    }
}