using System;
using System.Linq;
using System.Threading;

namespace PuzzleSolver
{
    class PuzzleSolver
    {
        private enum Validation { Underflow, Exact, Overflow }

        private readonly int _size;
        private readonly string _lineSeparator;
        private readonly int _cursorLeft;
        private readonly int _cursorTop;
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
            _lineSeparator = new string(':', 3 * _size + 2);
            _cursorLeft = Console.CursorLeft;
            _cursorTop = Console.CursorTop;
            _sumRows = sumRows;
            _sumCols = sumCols;
            _puzzle = InitializeMatrix(initialPuzzle, _size);
        }

        public int[,] Solve()
        {
            DateTime start = DateTime.Now;
            Solve(0);
            DateTime end = DateTime.Now;
            Console.WriteLine($"Finished in: {end - start}");
            return _puzzle;
        }

        private bool Solve(int level)
        {
            Print();

            Validation validation = Validate();
            switch (validation)
            {
                case Validation.Exact:
                    return true;
                case Validation.Overflow:
                    return false;
            }

            if (level >= _size * _size)
            {
                return false;
            }

            int row = level / _size;
            int col = level % _size;
            int value = _puzzle[row, col];

            foreach (int newValue in Enumerable.Range(value, 10 - value).OrderBy(ignored => Guid.NewGuid()))
            {
                _puzzle[row, col] = newValue;
                if (Solve(level + 1))
                {
                    return true;
                }
            }

            _puzzle[row, col] = value;

            return false;
        }

        private void Print()
        {
            Console.SetCursorPosition(_cursorLeft, _cursorTop);

            Console.WriteLine(_lineSeparator);
            for (int row = 0; row < _puzzle.GetLength(0); row++)
            {
                for (int col = 0; col < _puzzle.GetLength(1); col++)
                {
                    Console.Write("::{0}", _puzzle[row, col]);
                }
                Console.WriteLine("::");
                Console.WriteLine(_lineSeparator);
            }
        }

        private Validation Validate()
        {
            return Validate(_puzzle, _sumRows, _sumCols);
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
