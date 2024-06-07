namespace AdvanceGame
{
    /// <summary>
    /// Represents the Wall piece on the game board.
    /// </summary>
    public class Wall : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Wall"/> class with the specified player and initial square.
        /// </summary>
        /// <param name="player">The player to whom the wall belongs.</param>
        /// <param name="initialSquare">The initial square occupied by the wall.</param>
        public Wall(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }

        /// <summary>
        /// Gets the icon representation of the wall.
        /// </summary>
        public override char Icon
        {
            get { return '#'; }
        }

        /// <summary>
        /// Gets or sets the player of the wall. In this case, it is always null.
        /// </summary>
        protected override Player? Player
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Determines whether the wall can attack a specified square.
        /// </summary>
        /// <param name="targetSquare">The square to be attacked.</param>
        /// <returns>Always returns false, as the wall cannot attack.</returns>
        public override bool CanAttack(Square targetSquare)
        {
            return false;
        }

        /// <summary>
        /// Determines whether the wall can move to a specified square.
        /// </summary>
        /// <param name="targetSquare">The square to move to.</param>
        /// <returns>Always returns false, as the wall cannot move.</returns>
        public override bool CanMoveTo(Square targetSquare)
        {
            return false;
        }

        /// <summary>
        /// Determines whether the wall can be swapped with a piece on a specified square.
        /// </summary>
        /// <param name="targetSquare">The square to be swapped with.</param>
        /// <returns>Always returns false, as the wall cannot swap.</returns>
        public override bool CanSwap(Square targetSquare)
        {
            return false;
        }
    }
}