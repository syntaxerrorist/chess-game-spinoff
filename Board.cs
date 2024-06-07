using System.Drawing;

namespace AdvanceGame
{
    /// <summary>
    /// Represents a game board consisting of squares stored inside a 1D array.
    /// </summary>
    public class Board
    {

        public const int Size = 9; 

        private Square[] squares = new Square[Size * Size]; // board size is 9x9

        /// <summary>
        /// Gets the collection of squares on the board.
        /// Using enum encapsulates the square[] data
        /// </summary>
        /// <returns>The collection of squares on the board.</returns>
        public IEnumerable<Square> Squares
        {
            get { return squares; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// Performs a double sweep to assign each square its own set of row and column co-ordinate values
        /// </summary>
        public Board()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    Square newSquare = new Square(this, row, col);
                    Set(row, col, newSquare); // calls the Set() method to assign co-ordinate for a given square on the board
                }
            }
        }

        /// <summary>
        /// Sets the specified square at the given row and column coordinates.
        /// </summary>
        /// <param name="row">The row coordinate.</param>
        /// <param name="col">The column coordinate.</param>
        /// <param name="newSquare">The square to set.</param>
        /// <exception cref="ArgumentException">Thrown when the row or column is out of bounds.</exception>
        private void Set(int row, int col, Square newSquare)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
                throw new ArgumentException($"Position row = {row}, col = {col} is out of bounds");
            squares[row * Size + col] = newSquare;
        }

        /// <summary>
        /// Gets the square at the specified row and column coordinates.
        /// </summary>
        /// <param name="row">The row coordinate.</param>
        /// <param name="col">The column coordinate.</param>
        /// <returns>The square at the specified coordinates, or null if the coordinates are out of bounds.</returns>
        public Square? Get(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size) return null;
            return squares[row * Size + col];
        }

        /// <summary>
        /// Returns a string representation of the board.
        /// </summary>
        /// <returns>A string representation of the board.</returns>
        public override string ToString()
        {
            return $"Board:\n{string.Join<Square>("\n", squares)}";
        }
    }
}     