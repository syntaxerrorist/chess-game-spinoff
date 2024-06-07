namespace AdvanceGame
{
    /// <summary>
    /// Represents a General piece in the game.
    /// </summary>
    public class General : Piece
    {
        /// <summary>
        /// Initializes a new instance of the General class.
        /// </summary>
        /// <param name="player">The player who owns the General.</param>
        /// <param name="initialSquare">The initial square occupied by the General.</param>
        public General(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }

        /// <summary>
        /// Determines whether the General can attack the specified square.
        /// </summary>
        /// <param name="targetSquare">The target square to check for attack.</param>
        /// <returns><c>true</c> if the General can attack the target square; otherwise, <c>false</c>.</returns>
        public override bool CanAttack(Square targetSquare)
        {
            // if any of these are true, return false meaning General doesn't have a valid attack
            if (Square == null ||
                targetSquare.IsFree ||
                targetSquare.Occupant.GetPlayer() == this.Player ||
                targetSquare.Occupant is Wall ||
                SentinelCheck(targetSquare))
            {
                return false;
            }
            /* 
            * calls CanMoveTo() method to check if Miner can move to target square, 
            * if it can, then it is considered a valid attack
            */
            return CanMoveTo(targetSquare);
        }

        /// <summary>
        /// Determines whether the General can move to the specified square.
        /// </summary>
        /// <param name="targetSquare">The target square to check for movement.</param>
        /// <returns><c>true</c> if the General can move to the target square; otherwise, <c>false</c>.</returns>
        public override bool CanMoveTo(Square targetSquare)
        {
            if (Square == null) throw new Exception("Cannot move with piece that is off the board!");

            int rowDiff = targetSquare.GetRow() - Square.GetRow();
            int colDiff = targetSquare.GetCol() - Square.GetCol();

            if (Math.Abs(rowDiff) == 1 && colDiff == 0 ||
                Math.Abs(colDiff) == 1 && rowDiff == 0 ||
                Math.Abs(colDiff) == 1 && Math.Abs(rowDiff) == 1)
            {
                return true;
            }
            
            return false;
        }


        /// <summary>
        /// Determines whether the General can swap positions with the specified square.
        /// </summary>
        /// <param name="targetSquare">The square to swap positions with.</param>
        /// <returns>
        ///   <c>false</c> since the General cannot perform swapping.
        /// </returns>
        public override bool CanSwap(Square targetSquare)
        {
            return false;
        }
    }
}
