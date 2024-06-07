namespace AdvanceGame
{

    /// <summary>
    /// Represents a square on the game board.
    /// </summary>
    public class Square
    {
        /// <summary>
        /// Gets the board to which this square belongs.
        /// </summary>
        private Board Board { get; }

        /// <summary>
        /// Gets the row index of the square.
        /// </summary>
        private int Row { get; }

        /// <summary>
        /// Gets the column index of the square.
        /// </summary>
        private int Col { get; }


        // 'occupant' field of 'Square' object holds reference to a 'Piece' object
        // Since the piece class has access to the square object, it can access info about the square it occupies
        /// <summary>
        /// Gets or sets the occupant piece of the square.
        /// </summary>
        private Piece? occupant;

        // Initialize properties
        /// <summary>
        /// Initializes a new instance of the <see cref="Square"/> class.
        /// </summary>
        /// <param name="board">The board to which this square belongs.</param>
        /// <param name="row">The row index of the square.</param>
        /// <param name="col">The column index of the square.</param>

        public Square(Board board, int row, int col)
        {
            Board = board;
            Row = row;
            Col = col;
        }


        /// <summary>
        /// Gets or sets the occupant piece of the square.
        /// </summary>
        /// <value>The occupant piece of the square.</value>
        /// <returns>The occupant piece of the square when getting the value.</returns>
        public Piece? Occupant
        {
            // Returns if the square has an occupant or not
            get { return occupant; }
            private set
            {
                if (value == null) throw new ArgumentNullException();
                occupant = value;
            }
        }


        /// <summary>
        /// Retrieves the square at the specified row and column coordinates on the board.
        /// </summary>
        /// <param name="row">The row index of the square.</param>
        /// <param name="col">The column index of the square.</param>
        /// <returns>The square at the specified coordinates, or null if it doesn't exist.</returns>
        public Square? GetBoard(int row, int col)
        {
            return Board.Get(row, col);
        }


        /// <summary>
        /// Occupies the square with the specified piece.
        /// </summary>
        /// <param name="piece">The piece to be placed in the square.</param>
        /// <exception cref="ArgumentException">Thrown if the square is already occupied.</exception>
        public void Occupy(Piece piece)
        {
            if (IsOccupied) throw new ArgumentException("Piece cannot be placed in occupied square.");
            Occupant = piece;
        }


        /// <summary>
        /// Determines if the square has no occupant/piece.
        /// </summary>
        /// <value><c>true</c> if the square has no occupant/piece; otherwise, <c>false</c>.</value>
        /// <returns>Returns <c>true</c> if the square has no occupant/piece; otherwise, returns <c>false</c>.</returns>
        public bool IsFree
        {
            // Returns 'null' if square has no occupant/ piece
            get { return occupant == null; }
        }

        /// <summary>
        /// Determines if the square is occupied by a piece.
        /// </summary>
        /// <value><c>true</c> if the square is occupied by a piece; otherwise, <c>false</c>.</value>
        /// <returns>Returns <c>true</c> if the square is occupied by a piece; otherwise, returns <c>false</c>.</returns>
        public bool IsOccupied
        {
            get
            {
                return !IsFree;
            }
        }

        /// <summary>
        /// Removes the piece from the square.
        /// </summary>
        public void Remove()
        {
            occupant = null;
        }

        /// <summary>
        /// Gets the current row index of the square.
        /// </summary>
        /// <returns>The current row index of the square.</returns>
        public int GetRow()
        {
            return Row;
        }
        
        /// <summary>
        /// Gets the current column index of the square.
        /// </summary>
        /// <returns>The current column index of the square.</returns>
        public int GetCol()
        {
            return Col;
        }

        
        /// <summary>
        /// Gets the neighbouring square and its co-ordinates based on the given row and column offsets.
        /// </summary>
        /// <param name="rowOffset">The row offset relative to the current square.</param>
        /// <param name="colOffset">The column offset relative to the current square.</param>
        /// <returns>The neighbouring square if it exists within the bounds of the board; otherwise, null.</returns>
        public Square? Neighbour(int rowOffset, int colOffset)
        {
            if ((Row == 0 && rowOffset > 0) || (Col == 0 && colOffset > 0))
            {
                return null;
            }

            int adjRow = Row - rowOffset;
            int adjCol = Col - colOffset;

            if (adjRow < 0 || adjCol < 0 || adjRow > 8 || adjCol > 8)
            {
                return null;
            }

            return Board.Get(adjRow, adjCol);
        }


        /// <summary>
        /// Gets a list of all adjacent (including diagonal) squares that the current square is located.
        /// </summary>
        /// <value>An enumerable collection of adjacent squares. Null squares represent squares that are out of bounds or invalid.</value>
        /// <returns>An enumerable collection of adjacent squares, excluding null squares representing out of bounds or invalid squares.</returns>
        public IEnumerable<Square?> AdjacentSquares
        {
            get
            {
                List<Square?> allAdjacentSquares = new List<Square?>
                {
                    Neighbour(1, 1), Neighbour(-1, 1), Neighbour(1, -1), Neighbour(-1, -1),
                    Neighbour(1, 0), Neighbour(-1, 0), Neighbour(0, 1), Neighbour(0, -1)
                };

                List<Square?> validSquares = new List<Square?>();
                foreach (Square? square in allAdjacentSquares)
                {
                    if (square != null)
                    {
                        validSquares.Add(square);
                    }
                }

                return validSquares;
            }
        }

        /// <summary>
        /// Gets a list of all orthogonal neighbouring squares (like a rook) of the current square.
        /// </summary>
        /// <value>An enumerable collection of orthogonal neighbouring squares. Null squares represent squares that are out of bounds or invalid.</value>
        /// <returns>An enumerable collection of orthogonal neighbouring squares, excluding null squares representing out of bounds or invalid squares.</returns>
        public IEnumerable<Square?> OrthogonalSquares
        {
            get
            {
                List<Square?> allOrthogonalSquares = new List<Square?>
                {
                    Neighbour(1, 0), Neighbour(-1, 0), Neighbour(0, 1), Neighbour(0, -1)
                };

                List<Square?> validSquares = new List<Square?>();
                foreach (Square? square in allOrthogonalSquares)
                {
                    if (square != null)
                    {
                        validSquares.Add(square);
                    }
                }

                return validSquares;
            }
        }

        /// <summary>
        /// Gets the type of the piece.
        /// </summary>
        /// <returns>The type of the piece.</returns>
        public Type GetPieceType()
        {
            if (Occupant == null) 
            {
                throw new Exception("There is no piece type of null value.");
            }
            else 
            {
                return Occupant.GetType();
            }       
        }

        /// <summary>
        /// Returns a string representation of the square.
        /// </summary>
        /// <returns>A string representation of the square, including its row and column.</returns>
        public override string ToString()
        {
            return $"Square row = {Row}, column = {Col}";

        }

        /// <summary>
        /// Determines if the square is threatened by any pieces.
        /// </summary>
        /// <returns>Returns <c>true</c> if the square is threatened by any pieces; otherwise, returns <c>false</c>.</returns>
        public bool IsThreatened
        {
            get
            {
                return ThreateningPieces.Any();
            }
        }


        /// <summary>
        /// Gets the list of threatening pieces for the current square.
        /// </summary>
        /// <returns>An enumerable collection of threatening pieces.</returns>
        public IEnumerable<Piece> ThreateningPieces
        {
            get
            {
                List<Piece> threateningPieces = new List<Piece>();

                foreach (Square square in Board.Squares)
                {
                    Piece? piece = square.Occupant;
                    if (piece != null && piece.CanAttack(this)) // checks if there is a piece and can attack current square
                    {
                        threateningPieces.Add(square.Occupant);
                    }
                }

                // if current square is occupied, filter out previous list to retrieve only opposing pieces
                if (Occupant != null)
                {
                    List<Piece> filteredPieces = new List<Piece>();
                    foreach (Piece piece in threateningPieces)
                    {
                        /* checks and adds pieces as actual threats if threatening pieces are
                         * controlled by opposing player
                         */
                        if (piece.GetPlayer() != Occupant.GetPlayer()) 
                        {
                            filteredPieces.Add(piece);
                        }
                    }
                    return filteredPieces;
                }
                // otherwise if current square is empty, simply return list containing unfiltered list
                return threateningPieces;
            }
        }
    }
}
