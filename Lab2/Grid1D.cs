namespace Lab2
{
    struct Grid1D
    {
        public float Step { get; set; }
        public int Num { get; set; }

        public const float Begin = 0;

        public Grid1D(float step, int num)
        {
            Step = step;
            Num = num;
        }
        
        public override string ToString() {
            return $"Step = {Step} Number of nodes = {Num}";
        }
    }
}