using System.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Lab2
{
    class V2DataOnGrid : V2Data, IEnumerable<DataItem>
    {
        public Grid1D[] Grid2D { get; set; }
        public Complex[,] FieldOnGrid { get; set; }

        private class Enumerator : IEnumerator<DataItem>
        {
            private Grid1D[] Grid2D;
            private Complex[,] FieldOnGrid;

            public Enumerator(Grid1D[] axes, Complex[,] field)
            {
                Grid2D = axes;
                FieldOnGrid = field;
            }

            private int x = 0;
            private int y = 0;
            private bool begin = false;
            
            public DataItem Current {
                get {
                    Vector2 coord = new Vector2(Grid2D[0].Step * x, Grid2D[1].Step * y);
                    Complex EM_field = FieldOnGrid[x, y]; // если выйдем за пределы бросит исключение
                    return new DataItem(coord, EM_field);
                }
            }

            public bool MoveNext()
            {
                if (x >= Grid2D[0].Num)
                {
                    return false;
                }

                if (x == Grid2D[0].Num - 1 && y == Grid2D[1].Num - 1)
                {
                    return false;
                }

                if (!begin)
                {
                    begin = true;
                    return true;
                }

                if (y >= Grid2D[1].Num - 1)
                {
                    ++x;
                    y = 0;
                    return true;
                }

                ++y;
                return true;
            }
            
            public void Reset() {
                x = 0;
                y = 0;
                begin = false;
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                FieldOnGrid = null;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(Grid2D, FieldOnGrid);
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {            
            return new Enumerator(Grid2D, FieldOnGrid);
        }


        public V2DataOnGrid(string info, double freq, Grid1D X, Grid1D Y) : base(info, freq)
        {
            Grid2D = new Grid1D[2];
            Grid2D[0] = X;
            Grid2D[1] = Y;

            FieldOnGrid = new Complex[Grid2D[0].Num, Grid2D[1].Num];
        }

        public void InitRandom(double minValue, double maxValue)
        {
            Random rnd = new Random();
            
            for (int i = 0; i < Grid2D[0].Num; ++i)
            {
                for (int j = 0; j < Grid2D[1].Num; j++)
                {
                    FieldOnGrid[i, j] = new Complex(rnd.NextDouble() * (maxValue - minValue) + minValue,
                        rnd.NextDouble() * (maxValue - minValue) + minValue);
                }
            }
        }

        public override Complex[] NearAverage(float eps)
        {
            double average = 0;
            for (int i = 0; i < Grid2D[0].Num; ++i)
            {
                for (int j = 0; j < Grid2D[1].Num; j++)
                {
                    average += FieldOnGrid[i, j].Real;
                }
            }

            average /= Grid2D[0].Num * Grid2D[1].Num;

            int counter = 0;
            
            for (int i = 0; i < Grid2D[0].Num; ++i)
            {
                for (int j = 0; j < Grid2D[1].Num; j++)
                {
                    if (Math.Abs(average - FieldOnGrid[i, j].Real) < eps)
                    {
                        ++counter;
                    }
                }
            }

            Complex[] answer = new Complex[counter];
            counter = 0;
            
            for (int i = 0; i < Grid2D[0].Num; ++i)
            {
                for (int j = 0; j < Grid2D[1].Num; j++)
                {
                    if (Math.Abs(average - FieldOnGrid[i, j].Real) < eps)
                    {
                        answer[counter] = FieldOnGrid[i, j];
                        ++counter;
                    }
                }
            }

            return answer;
        }

        public override string ToLongString()
        {
            string answer = this.ToString();

            for (int i = 0; i < Grid2D[0].Num; ++i)
            {
                for (int j = 0; j < Grid2D[1].Num; ++j)
                {
                    answer += $"Point({i * Grid2D[0].Step}, {j * Grid2D[1].Step}) Field = {FieldOnGrid[i, j]}\n";
                }
            }

            return answer;
        }

        public override string ToString()
        {
            return $"V2DataOnGrid\n" + base.ToString() + "\n" +$"OX GridData: Step = {Grid2D[0].Step}, Number of Nodes = {Grid2D[0].Num}\n" +
                   $"OY GridData: Step = {Grid2D[1].Step}, Number of Nodes = {Grid2D[1].Num}\n";
        }
        
        public static explicit operator V2DataCollection(V2DataOnGrid obj) {
            V2DataCollection res = new V2DataCollection(obj.Info, obj.FieldFrequency);

            for (int i = 0; i < obj.Grid2D[0].Num; i++) {
                for (int j = 0; j < obj.Grid2D[1].Num; j++) {
                    Vector2 vec = new Vector2((float)i * obj.Grid2D[0].Step, (float)j * obj.Grid2D[1].Step);
                    Complex field = obj.FieldOnGrid[i, j];
                    res.DataItems.Add(new DataItem(vec, field));
                }
            }

            return res;
        }
        
        public override string ToLongString(string format) {
            string res = $"V2DataOnGrid\n{Info}\nField frequency is {FieldFrequency.ToString(format)}\n" +
                         $"OX GridData: Step = {Grid2D[0].Step.ToString(format)}, Number of Nodes = {Grid2D[0].Num}\n" +
                         $"OY GridData: Step = {Grid2D[1].Step.ToString(format)}, Number of Nodes = {Grid2D[1].Num}\n";

            for (int i = 0; i < Grid2D[0].Num; i++) {
                for (int j = 0; j < Grid2D[1].Num; j++) {
                    res += $"Point({i}, {j}) Field = {FieldOnGrid[i, j].ToString(format)}, |Field| = {FieldOnGrid[i, j].Magnitude.ToString(format)}\n";
                }
            }

            return res;

        }

        public V2DataOnGrid(string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                StreamReader streamReader = new StreamReader(fs);

                Info = streamReader.ReadLine();
                FieldFrequency = float.Parse(streamReader.ReadLine());
                
                Grid2D = new Grid1D[2];

                string buffer = streamReader.ReadLine();
                string[] lst = buffer.Split(' ');
                Grid2D[0] = new Grid1D(float.Parse(lst[0]), int.Parse(lst[1]));
                
                buffer = streamReader.ReadLine();
                lst = buffer.Split(' ');
                Grid2D[1] = new Grid1D(float.Parse(lst[0]), int.Parse(lst[1]));
                
                FieldOnGrid = new Complex[Grid2D[0].Num, Grid2D[1].Num];

                for (int i = 0; i < Grid2D[0].Num; ++i)
                {
                    for (int j = 0; j < Grid2D[1].Num; ++j)
                    {
                        buffer = streamReader.ReadLine();
                        lst = buffer.Split(' ');
                        Complex field = new Complex(Convert.ToDouble(lst[0]), Convert.ToDouble(lst[1]));

                        FieldOnGrid[i, j] = field;
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            } finally {
                if (fs != null) {
                    fs.Close();
                }
            }
        }
    }
}