using System.Numerics;

namespace Lab2
{
    public struct DataItem
    {
        public Vector2 Coord { get; set; }
        public Complex Field { get; set; }

        public DataItem(Vector2 point, Complex fld)
        {
            Coord = point;
            Field = fld;
        }

        public override string ToString()
        {
            return $"Point({Coord.X}, {Coord.Y}) Field: {Field}";
        }
        
        public string ToString(string format) {
            return $"Point({Coord.ToString(format)}0\nField: {Field.ToString(format)}\n" +
                   $"|Field| = {Field.Magnitude.ToString(format)}\n";
        }
    }
}