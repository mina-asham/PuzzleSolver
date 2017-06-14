using System;
using System.Linq;

namespace PuzzleSolver
{
    class PuzzleSolver
    {
        private enum Validation { Underflow, Exact, Overflow }

        private readonly int _size;
        private readonly int[] _sumRows;
        private readonly int[] _sumCols;
        private readonly int[,] _puzzle;

        public PuzzleSolver(int[] sumRows, int[] sumCols, int[,] initialPuzzle = null)
        {
            Check<ArgumentNullException>(sumRows != null);
            Check<ArgumentNullException>(sumCols != null);
            Check<ArgumentException>(sumRows.Length == sumCols.Length);
            Check<ArgumentException>(sumRows.All(sum => sum >= 0));
            Check<ArgumentException>(sumCols.All(sum => sum >= 0));
            Check<ArgumentException>(initialPuzzle == null || Validate(initialPuzzle, sumRows, sumCols) != Validation.Overflow);

            _size = sumRows.Length;
            _sumRows = sumRows;
            _sumCols = sumCols;
            _puzzle = InitializeMatrix(initialPuzzle, _size);
        }

        public int[,] Solve()
        {
            throw new NotImplementedException();
        }

        private static Validation Validate(int[,] puzzle, int[] sumRows, int[] sumCols)
        {
            bool isExact = true;
            for (int row = 0; row < puzzle.GetLength(0); row++)
            {
                int sum = 0;
                for (int col = 0; col < puzzle.GetLength(1); col++)
                {
                    sum += puzzle[row, col];
                    if (sum > sumRows[row])
                    {
                        return Validation.Overflow;
                    }
                }
                if (sum < sumRows[row])
                {
                    isExact = false;
                }
            }

            for (int col = 0; col < puzzle.GetLength(1); col++)
            {
                int sum = 0;
                for (int row = 0; row < puzzle.GetLength(0); row++)
                {
                    sum += puzzle[row, col];
                    if (sum > sumCols[col])
                    {
                        return Validation.Overflow;
                    }
                }
                if (sum < sumCols[col])
                {
                    isExact = false;
                }
            }

            return isExact ? Validation.Exact : Validation.Underflow;
        }

        private static int[,] InitializeMatrix(int[,] initialPuzzle, int size)
        {
            if (initialPuzzle != null)
            {
                return initialPuzzle.Clone() as int[,];
            }

            return new int[size, size];
        }

        private static void Check<T>(bool condition) where T : Exception, new()
        {
            if (!condition)
            {
                throw new T();
            }
        }
    }
}
