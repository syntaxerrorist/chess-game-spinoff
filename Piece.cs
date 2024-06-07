namespace AdvanceGame
{
    /// <summary>
    /// Represents an abstract base class for a game piece.
    /// </summary>
    public abstract class Piece
    {
        /// <summary>
        /// Gets or sets the player who owns the piece.
        /// </summary>   
        protected virtual Player? Player { get; set; }
        
        /// <summary>
        /// Gets the square that the piece occupies.
        /// </summary>
        public Square? Square { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Piece"/> class with the specified player and square location.
        /// </summary>
        /// <param name="player">The player who owns the piece.</param>
        /// <param name="squareLocation">The square location that the piece occupies.</param>
        public Piece(Player player, Square squareLocation)
        {
            Player = player; 
            PieceSquare(squareLocation); 
        }

        /// <summary>
        /// Assigns the piece to the specified square with their own set of co-ordinates, updating its square location.
        /// </summary>
        /// <param name="square">The square that the piece occupies.</param>
        internal void PieceSquare(Square square)
        {
            square.Occupy(this); // this piece will now take properties (co-ordinates) of this 'square' object
            Square = square;

        }

        /// <summary>
        /// Determines whether the piece is currently on the game board.
        /// </summary>
        /// <returns><c>true</c> if the piece is on the board; otherwise, <c>false</c>.</returns>
        public bool OnBoard
        {
            get
            {
                return Square != null;
            }
        }

        /// <summary>
        /// Gets the icon representation of the piece on the board. Player 'white' is assigned uppercase and 'black' is lowercase
        /// </summary>
        /// <returns>The icon character representing the piece.</value>
        public virtual char Icon
        {
            get
            {
                char firstLetter;

                if (Player.Colour == Colour.Wall) // icon of '#' cannot be assigned to either white or black therefore only assigned to player 'Wall'
                {
                    firstLetter = '#';
                }
                else
                {
                    firstLetter = GetType().Name[0];

                    if (Player.Colour == Colour.Black)
                    {
                        firstLetter = char.ToLower(firstLetter); // assign lowercase letters if player colour is black
                    }
                    else
                    {
                        firstLetter = char.ToUpper(firstLetter); // else assign uppercase letters if player colour is white
                    }
                }

                return firstLetter;
            }
        }

        /// <summary>
        /// Removes the piece from its current square when it moves or gets taken by another player.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the piece is not on the board.</exception>
        public void LeaveSquare()
        {
            if (Square == null)
                throw new ArgumentNullException("Piece cannot be removed if it is not on the board.");
            Square.Remove();
            Square = null;
        }

        /// <summary>
        /// Moves the piece to the specified square, updating the current board state.
        /// </summary>
        /// <param name="newSquare">The square to which the piece will be moved.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if the piece cannot move to the designated square or if the destination square is occupied.
        public virtual void MoveTo(Square newSquare)
        {
            if (!CanMoveTo(newSquare)) 
                throw new ArgumentException("Piece cannot move to designated square");

            if (newSquare.IsOccupied) 
                throw new ArgumentException("Destination square must not be occupied");

            LeaveSquare();
            PieceSquare(newSquare);
        }


        /// <summary>
        /// Attacks the specified square, removing the target piece and updating the current board state.
        /// </summary>
        /// <param name="targetSquare">The square to be attacked.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if the target square cannot be attacked, is empty, or is the same as the current square.
        public virtual void Attack(Square targetSquare)
        {
            if (!CanAttack(targetSquare)) 
                throw new ArgumentException("Target square cannot be attacked.");

            if (targetSquare.IsFree) 
                throw new ArgumentException("Cannot attack an empty square.");

            if (targetSquare == Square)
                throw new ArgumentException("Cannot attack itself.");

            targetSquare.Occupant?.LeaveSquare(); // if occupant exists on a square and is attacked, then it leaves the square
            LeaveSquare(); // the attacking piece leaves the board
            PieceSquare(targetSquare); // attacking piece is now assigned to the target square
            
        }

        /// <summary>
        /// Checks if the target square is protected by a Sentinel piece.
        /// </summary>
        /// <param name="targetSquare">The square to check for Sentinel protection.</param>
        /// <returns><c>true</c> if the target square is protected by a Sentinel; otherwise, <c>false</c>.</returns>
        protected bool SentinelCheck(Square targetSquare)
        {
            // check all orthogonally adjacent squares of target square
            return targetSquare.OrthogonalSquares.Any(square =>
                square.Occupant is Sentinel && square.Occupant.GetPlayer() == Player.Opponent);
        }

        /// <summary>
        /// Gets the player associated with the piece.
        /// </summary>
        /// <returns>The player associated with the piece, or <c>null</c> if no player is assigned.</returns>
        public Player? GetPlayer()
        {
            if (Player == null) return null;
            return Player;
        }

        /// <summary>
        /// Gets the square and its properties currently occupied by the piece.
        /// </summary>
        /// <returns>The square currently occupied by the piece, or <c>null</c> if the piece is not on it.</returns>
        public Square? GetSquare()
        {
            return Square;
        }

        /// <summary>
        /// Evaluates whether the piece can move to a new destination square.
        /// </summary>
        /// <param name="newSquare">The new destination square to evaluate.</param>
        /// <returns><c>true</c> if the piece can move to the new square; otherwise, <c>false</c>.</returns>
        public abstract bool CanMoveTo(Square newSquare);

        /// <summary>
        /// Evaluates whether the piece can attack a piece on a different square.
        /// </summary>
        /// <param name="newSquare">The square containing the target piece to evaluate.</param>
        /// <returns><c>true</c> if the piece can attack the target piece; otherwise, <c>false</c>.</returns>
        public abstract bool CanAttack(Square newSquare);


        /// <summary>
        /// Evaluates whether the piece can be swapped with another piece on a different square.
        /// </summary>
        /// <param name="newSquare">The square containing the piece to swap with.</param>
        /// <returns><c>true</c> if the piece can be swapped; otherwise, <c>false</c>.</returns>
        public abstract bool CanSwap(Square newSquare);

        
        /// <summary>
        /// Gets or sets a value indicating whether the piece can perform a conversion (e.g. Jester).
        /// </summary>
        public bool CanConvert = false;


        /// <summary>
        /// Changes the player associated with the piece to the opponent player.
        /// </summary>
        public void Defect()
        {
            this.Player = Player.Opponent;
        }
        
        
    }
}