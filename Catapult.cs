namespace AdvanceGame
{

    /// <summary>
    /// Represents a Catapult piece in the game.
    /// </summary>
    public class Catapult : Piece
        {

            /// <summary>
            /// Initializes a new instance of the Catapult class.
            /// </summary>
            /// <param name="player">The player that owns the Catapult.</param>
            /// <param name="initialSquare">The initial square where the Catapult is placed.</param>
            public Catapult(Player player, Square initialSquare) : base(player, initialSquare)
            {
            }

            /// <summary>
            /// Determines whether the Catapult can attack a target square.
            /// </summary>
            /// <param name="targetSquare">The target square to attack.</param>
            /// <returns>True if the Catapult can attack the target square; otherwise, false.</returns>
            public override bool CanAttack(Square targetSquare)
            {
                // if any of these are true, return false meaning Catapult doesn't have a valid attack
                if (Square == null ||
                    targetSquare.IsFree ||
                    targetSquare.Occupant.GetPlayer() == this.Player ||
                    targetSquare.Occupant is Wall ||
                    SentinelCheck(targetSquare))
                {
                    return false;
                }

                int rowDiff = targetSquare.GetRow() - Square.GetRow();
                int colDiff = targetSquare.GetCol() - Square.GetCol();

                // Check for valid attack conditions
                return targetSquare.Occupant is Wall ||
                        (rowDiff == Player.Direction * 3 || rowDiff == Player.Direction * -3) && colDiff == 0 ||
                        (colDiff == Player.Direction * 3 || colDiff == Player.Direction * -3) && rowDiff == 0 ||
                        Math.Abs(rowDiff) == 2 && Math.Abs(colDiff) == 2;

            }

            /// <summary>
            /// Determines whether the Catapult can move to a target square.
            /// </summary>
            /// <param name="targetSquare">The target square to move to.</param>
            /// <returns>True if the Catapult can move to the target square; otherwise, false.</returns>
            public override bool CanMoveTo(Square targetSquare)
            {

                if (Square == null)
                { return false; }

                int rowDiff = targetSquare.GetRow() - Square.GetRow();
                int colDiff = targetSquare.GetCol() - Square.GetCol();

                // Check for valid movement conditions
                return !targetSquare.IsOccupied &&
                       ((rowDiff == Player.Direction || 
                         rowDiff == -Player.Direction) && colDiff == 0) ||
                        (colDiff == Player.Direction && rowDiff == 0) ||
                        (colDiff == -Player.Direction && rowDiff == 0);
            }

        /// <summary>
        /// Determines whether the Catapult can perform a swap with a target square.
        /// </summary>
        /// <param name="targetSquare">The target square to swap with.</param>
        /// <returns>False, as Catapult cannot perform swaps.</returns>
        public override bool CanSwap(Square taretSquare)
        {
            return false;
        }
    }
}
