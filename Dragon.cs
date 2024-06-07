namespace AdvanceGame
{
    /// <summary>
    /// Represents a Dragon piece in the game.
    /// </summary>
    public class Dragon : Piece
    {
        /// <summary>
        /// Initializes a new instance of the Dragon class.
        /// </summary>
        /// <param name="player">The player who owns the Dragon piece.</param>
        /// <param name="initialSquare">The initial square where the Dragon is placed.</param>
        public Dragon(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }

        /// <summary>
        /// Determines whether the Dragon can attack a target square.
        /// </summary>
        /// <param name="targetSquare">The target square to attack.</param>
        /// <returns>True if the Dragon can attack the target square; otherwise, false.</returns>
        public override bool CanAttack(Square targetSquare)
        {
            // if any of these are true, return false meaning Dragon doesn't have a valid attack
            if (Square == null ||
                targetSquare.IsFree ||
                targetSquare.Occupant.GetPlayer() == this.Player ||
                targetSquare.Occupant is Wall ||
                SentinelCheck(targetSquare))
            {
                return false;
            }
            /*
                * !Contains() method checks to see if targetSquare is present inside list of adjacent squares
                *  and negates it since a dragon cannot move to a square adjacent to it. If both of these
                *  are satisfied and that the target square is indeed not one of the adjacent squares and the
                *  dragon can move to it, evaluate expression to true meaning that it is considered a valid attack.
                */
            return !this.Square.AdjacentSquares.Contains(targetSquare) && CanMoveTo(targetSquare);

        }

        /// <summary>
        /// Determines whether the Dragon can move to a target square.
        /// </summary>
        /// <param name="targetSquare">The target square to move to.</param>
        /// <returns>True if the Dragon can move to the target square; otherwise, false.</returns>
        public override bool CanMoveTo(Square targetSquare)
        {
            if (Square == null || targetSquare.GetRow() == Square.GetRow() && targetSquare.GetCol() == Square.GetCol())
            {
                return false; // Dragon is not placed on any square or target square is the same as the current square
            }

                int rowDiff = targetSquare.GetRow() - Square.GetRow();
                int colDiff = targetSquare.GetCol() - Square.GetCol();


            // Moving horizontally
            if (rowDiff == 0)
            {
                int colDirection = Math.Sign(colDiff);
                for (int i = Square.GetCol() + colDirection; i != targetSquare.GetCol(); i += colDirection)
                {
                    if (targetSquare.GetBoard(Square.GetRow(), i).IsOccupied)
                    {
                        return false;
                    }
                }
                return true;
            }

            // Moving vertically
            if (colDiff == 0)
            {
                int rowDirection = Math.Sign(rowDiff);
                for (int i = Square.GetRow() + rowDirection; i != targetSquare.GetRow(); i += rowDirection)
                {
                    if (targetSquare.GetBoard(i, Square.GetCol()).IsOccupied)
                    {
                        return false;
                    }
                }
                return true;
            }

            // Moving diagonally
            int deltaRow = targetSquare.GetRow() - Square.GetRow();
            int deltaCol = targetSquare.GetCol() - Square.GetCol();

            if (Math.Abs(deltaRow) == Math.Abs(deltaCol))
            {
                int rowDirection = Math.Sign(deltaRow);
                int colDirection = Math.Sign(deltaCol);
                int row = Square.GetRow() + rowDirection;
                int col = Square.GetCol() + colDirection;

                while (row != targetSquare.GetRow() || col != targetSquare.GetCol())
                {
                    if (targetSquare.GetBoard(row, col).IsOccupied)
                    {
                        return false; // There is an obstruction in the path
                    }
                    row += rowDirection;
                    col += colDirection;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the Dragon can perform a swap with a target square.
        /// </summary>
        /// <param name="targetSquare">The target square to swap with.</param>
        /// <returns>False, as Dragon cannot perform swaps.</returns>
        public override bool CanSwap(Square targetSquare)
        {
            return false;
        }
    }   
}