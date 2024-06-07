namespace AdvanceGame
{
    /// <summary>
    /// Represents a Jester piece in the game.
    /// </summary>
    public class Jester : Piece
        {
        /// <summary>
        /// Initializes a new instance of the <see cref="Jester"/> class.
        /// </summary>
        /// <param name="player">The player that owns the Jester.</param>
        /// <param name="initialSquare">The initial square of the Jester.</param>
        public Jester(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }

        /// <summary>
        /// Determines whether the Jester can attack the specified target square.
        /// </summary>
        /// <param name="targetSquare">The target square.</param>
        /// <returns><c>true</c> if the Jester can attack the target square; otherwise, <c>false</c>.</returns>
        public override bool CanAttack(Square targetSquare)
        {
            // if any of these are true, return false meaning Jester doesn't have a valid attack
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

            if (CanMoveTo(targetSquare))
            {
                CanConvert = ConvertPiece(targetSquare);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Determines whether the Jester can convert a piece in the specified target square.
        /// </summary>
        /// <param name="targetSquare">The target square.</param>
        /// <returns><c>true</c> if the Jester can convert a piece in the target square; otherwise, <c>false</c>.</returns>
        private bool ConvertPiece(Square targetSquare)
            {
                if (targetSquare.IsFree)
                {
                    return false;
                }
                if (targetSquare.Occupant?.GetPlayer() != this.Player && 
                    !(targetSquare.Occupant is Wall))
                {
                    return true;
                }
                               
                return false;         
            }

        /// <summary>
        /// Determines whether the Jester can move to the specified target square.
        /// </summary>
        /// <param name="targetSquare">The target square.</param>
        /// <returns><c>true</c> if the Jester can move to the target square; otherwise, <c>false</c>.</returns>
        public override bool CanMoveTo(Square targetSquare)
            {
            if (Square == null)
            { return false; }

                int rowDiff = targetSquare.GetRow() - Square.GetRow();
                int colDiff = targetSquare.GetCol() - Square.GetCol();

                return (Math.Abs(rowDiff) == 1 && colDiff == 0) ||
                        (Math.Abs(colDiff) == 1 && rowDiff == 0) ||
                        (Math.Abs(colDiff) == 1 && Math.Abs(rowDiff) == 1);
            
                    // return false when any of these conditions aren't true
            }

        /// <summary>
        /// Determines whether the Jester can swap places with the piece in the specified target square.
        /// </summary>
        /// <param name="targetSquare">The target square.</param>
        /// <returns><c>true</c> if the Jester can swap places with the piece in the target square; otherwise, <c>false</c>.</returns>
        public override bool CanSwap(Square targetSquare)
        {
        return !targetSquare.IsFree && targetSquare.Occupant?.GetPlayer() == this.Player && CanMoveTo(targetSquare);
        }
    }
}
