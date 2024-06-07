namespace AdvanceGame
{
    /// <summary>
    /// Represents a collection of pieces that belong to a player's army.
    /// </summary>
    public class Army
    {
        /// <summary>
        /// Gets the collection of pieces in the army.
        /// </summary>
        /// <returns>The collection of pieces in the army.</returns>
        public IEnumerable<Piece> Pieces
        {
            get
            {
                return pieces;
            }
        }
        private List<Piece> pieces = new List<Piece>();
        private Player Player { get; }
        private Board Board { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Army"/> class.
        /// </summary>
        /// <param name="player">The player associated with the army.</param>
        /// <param name="board">The game board.</param>
        public Army(Player player, Board board)
        {
            Player = player;
            Board = board;
        }

        /// <summary>
        /// Assigns a piece to the player and adds it to the army based on the specified icon and current square.
        /// Each piece object is then created using the Activator.CreateInstance()method
        /// </summary>
        /// <param name="icon">The icon representing the piece.</param>
        /// <param name="currentSquare">The current square occupied by the piece.</param>
        internal void AssignPiecesToPlayer(char icon, Square? currentSquare)
        {
            char symbol = Char.ToLower(icon);
            Piece? newPiece = null;

            Dictionary<char, Type> pieceTypes = new Dictionary<char, Type>()
            {
                {'#', typeof(Wall)},
                {'z', typeof(Zombie)},
                {'g', typeof(General)},
                {'b', typeof(Builder)},
                {'c', typeof(Catapult)},
                {'s', typeof(Sentinel)},
                {'d', typeof(Dragon)},
                {'m', typeof(Miner)},
                {'j', typeof(Jester)}
            };

            if (pieceTypes.ContainsKey(symbol))
            {
                // Creates an instance of each piece type which is detected on the game board
                Type pieceType = pieceTypes[symbol];
                newPiece = Activator.CreateInstance(pieceType, Player, currentSquare) as Piece;
                AddPiece(newPiece);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Adds a piece to the army by storing it in a list.
        /// </summary>
        /// <param name="piece">The piece to add.</param>
        public void AddPiece(Piece piece)
        {
            pieces.Add(piece);
        }
    }
}