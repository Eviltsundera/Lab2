﻿using System;

namespace Lab2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            V2DataCollection obj1 = new V2DataCollection("data.txt");
            Console.WriteLine("Object created from file\n");
            Console.WriteLine(obj1.ToLongString());
            Console.WriteLine("...\n");

            V2DataOnGrid obj2 = new V2DataOnGrid("data2.txt");
            Console.WriteLine("Object created from file 2\n");
            Console.WriteLine(obj2.ToLongString());
            Console.WriteLine("...\n");
            
            V2DataMainCollection MainCol = new V2DataMainCollection();
            MainCol.AddDefaults();
            MainCol.Add(new V2DataCollection("a", 1));
            MainCol.Add(new V2DataOnGrid("a", 0, new Grid1D(0, 0), new Grid1D(0, 0)));
            
            Console.WriteLine(MainCol.ToLongString());
            
            
            MainCol = new V2DataMainCollection();
            MainCol.NewAddDefaults();
            
            Console.WriteLine("...\n");
            Console.WriteLine("Average\nLINQ: ");
            Console.WriteLine(MainCol.AverageAbsValue);
            Console.WriteLine("Stupid:");
            Console.WriteLine(MainCol.StupidAverage());
            
            Console.WriteLine("Nearest:");
            Console.WriteLine(MainCol.NearestToAverage);
            
            Console.WriteLine("...\n");
            
            Console.WriteLine(MainCol.ToLongString());
            
            Console.WriteLine("Apperars everywhere:");
            var a = MainCol.AppearEveryWhere;
            foreach (var item in a)
            {
                Console.WriteLine(item);
            }
        }
    }
}