namespace AdvanceGame
{
    /// <summary>
    /// Represents a Miner piece in the game.
    /// </summary>
    public class Miner : Piece
    {

        /// <summary>
        /// Initializes a new instance of the Miner class with the specified player and initial square.
        /// </summary>
        /// <param name="player">The player to which the Miner belongs.</param>
        /// <param name="initialSquare">The initial square occupied by the Miner.</param>
        public Miner(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }

        /// <summary>
        /// Determines whether the Miner can attack the specified target square.
        /// </summary>
        /// <param name="targetSquare">The target square to attack.</param>
        /// <returns>true if the Miner can attack the target square; otherwise, false.</returns>
        public override bool CanAttack(Square targetSquare)
        {
            // if any of these are true, return false meaning Miner doesn't have a valid attack
            if (Square == null ||
                targetSquare.IsFree ||
                targetSquare.Occupant.GetPlayer() == this.Player ||
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
        /// Determines whether the Miner can move to the specified target square.
        /// Checks to see if there are any obstructions between itself and target square.
        /// Moves along rows and columns.
        /// </summary>
        /// <param name="targetSquare">The target square to move to.</param>
        /// <returns>
        ///   <c>true</c> if the Miner can move to the target square; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanMoveTo(Square targetSquare)
        {
            if (Square == null ||
                targetSquare == Square ||
                targetSquare.GetRow() == Square.GetRow() && targetSquare.GetCol() == Square.GetCol())
            {
                return false;
            }
            
            // Check to see if Miner is on the same row as target square
            if (targetSquare.GetRow() == Square.GetRow())
            {
                // set miner column movement direction to either 1 or -1
                int colDirection = (Square.GetCol() < targetSquare.GetCol()) ? 1 : -1;
                int startCol = Square.GetCol() + colDirection; // add colDirection to startcol to traverse through columns
                int endCol = targetSquare.GetCol(); // endCol is the column of the target square

                // if statement that covers both directions
                // logic for moving right
                if (colDirection > 0)
                {
                    for (int i = startCol; i < endCol; i++)
                    {
                        // check to see if squares in between miner and target square is occupied
                        if (targetSquare.GetBoard(Square.GetRow(), i).IsOccupied)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    // logic for moving left
                    for (int i = startCol; i > endCol; i--)
                    {
                        if (targetSquare.GetBoard(Square.GetRow(), i).IsOccupied)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            /*
             * Mirrored logic but this 'if else' statement checks
             * to see if Miner is on the same column as target square
             */
            if (targetSquare.GetCol() == Square.GetCol())
            {
                
                int rowDirection = (Square.GetRow() < targetSquare.GetRow()) ? 1 : -1; // set miner row movement direction to either 1 or -1
                int startRow = Square.GetRow() + rowDirection; // add rowDirection to current square to allow it to traverse along column(through rows)
                int endRow = targetSquare.GetRow();

                // if else statement that covers both directions
                // logic for moving down (+ve)
                if (rowDirection > 0)
                {
                    for (int i = startRow; i < endRow; i++)
                    {
                        if (targetSquare.GetBoard(i, Square.GetCol()).IsOccupied)
                        {
                            return false;
                        }
                    }
                }
                else
                {   
                    // logic for moving up (-ve)
                    for (int i = startRow; i > endRow; i--)
                    {
                        if (targetSquare.GetBoard(i, Square.GetCol()).IsOccupied)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }


        /// <summary>
        /// Determines whether the Miner can swap positions with the specified square.
        /// </summary>
        /// <param name="targetSquare">The square to swap positions with.</param>
        /// <returns>
        ///   <c>false</c> since the Miner cannot perform swapping.
        /// </returns>
        public override bool CanSwap(Square targetSquare)
        {
            return false;
        }
    }
}