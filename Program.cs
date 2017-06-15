using System;
using System.Threading;

namespace PuzzleSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            new PuzzleSolver(new int[] { 3, 28, 29 }, new int[] { 20, 1, 4 }).Solve();
        }
    }
}
