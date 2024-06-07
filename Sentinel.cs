namespace AdvanceGame
{
    /// <summary>
    /// Represents a Sentinel piece in the game.
    /// </summary>
    public class Sentinel : Piece
    {
        /// <summary>
        /// Initializes a new instance of the Sentinel class.
        /// </summary>
        /// <param name="player">The player that owns the Sentinel.</param>
        /// <param name="initialSquare">The initial square occupied by the Sentinel.</param>
        public Sentinel(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }


        /// <summary>
        /// Determines whether the Sentinel can attack a target square.
        /// </summary>
        /// <param name="targetSquare">The target square to attack.</param>
        /// <returns>True if the Sentinel can attack the target square, False otherwise.</returns>
        public override bool CanAttack(Square targetSquare)
        {
            // if any of these conditions are true, return false meaning Sentinel doesn't have a valid attack

            if (Square == null)
            { return false; }

            if (targetSquare.IsFree || targetSquare.Occupant.GetPlayer() == this.Player || targetSquare.Occupant is Wall || SentinelCheck(targetSquare))
            {
                return false;
            }
                
            return CanMoveTo(targetSquare);
        }


        /// <summary>
        /// Determines whether the Sentinel can move to a target square.
        /// </summary>
        /// <param name="targetSquare">The target square to move to.</param>
        /// <returns>True if the Sentinel can move to the target square, False otherwise.</returns>
        public override bool CanMoveTo(Square targetSquare)
        {
        if (Square == null)
        { return false; }

            int rowDiff = Math.Abs(targetSquare.GetRow() - Square.GetRow());
            int colDiff = Math.Abs(targetSquare.GetCol() - Square.GetCol());

            return (rowDiff == 1 && colDiff == 2) || (rowDiff == 2 && colDiff == 1);

            // return false when neither of these conditions are satisfied
        }

        /// <summary>
        /// Determines whether the Sentinel can perform a swap with a target square.
        /// </summary>
        /// <param name="targetSquare">The target square to swap with.</param>
        /// <returns>False, as Sentinels cannot perform swaps.</returns>
        public override bool CanSwap(Square targetSquare)
        {
            return false;
        }
    }
}
