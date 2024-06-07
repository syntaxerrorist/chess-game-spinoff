using AdvanceGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvanceGame
{
    /// <summary>
    /// Represents a Zombie piece in the game.
    /// </summary>
    public class Zombie : Piece
    {

        /// <summary>
        /// Initializes a new instance of the Zombie class with the specified player and initial square.
        /// </summary>
        /// <param name="player">The player that owns the Zombie.</param>
        /// <param name="initialSquare">The initial square occupied by the Zombie.</param>
        public Zombie(Player player, Square initialSquare) : base(player, initialSquare)
        {
        }

        /// <summary>
        /// Determines if the Zombie can attack the target square.
        /// </summary>
        /// <param name="targetSquare">The target square to attack.</param>
        /// <returns>True if the attack is valid, false otherwise.</returns>
        public override bool CanAttack(Square targetSquare)
        {

            // if any of these are true, return false meaning Zombie doesn't have a valid attack
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

            // consider leap attack
            if ((colDiff == -2 && rowDiff == Player.Direction * 2 && !Square.Neighbour(-Player.Direction, 1).IsOccupied) ||
                (colDiff == 2 && rowDiff == Player.Direction * 2 && !Square.Neighbour(-Player.Direction, -1).IsOccupied) ||
                (rowDiff == Player.Direction * 2 && colDiff == 0 && !Square.Neighbour(-Player.Direction, 0).IsOccupied) ||
                 CanMoveTo(targetSquare))
            {
                return true;
            }

            
            return false;
        }

        /// <summary>
        /// Determines if the Zombie can move to the target square.
        /// </summary>
        /// <param name="targetSquare">The target square to move to.</param>
        /// <returns>True if the move is valid, false otherwise.</returns>
        public override bool CanMoveTo(Square targetSquare)
        {
            if (Square == null)
            {
                return false;
            }

            int rowDiff = targetSquare.GetRow() - Square.GetRow();
            int colDiff = targetSquare.GetCol() - Square.GetCol();

            // returns true if either of these are satisfied, else return false
            return (rowDiff == Player.Direction && colDiff == 0) ||
                   (rowDiff == Player.Direction && Math.Abs(colDiff) == 1);
        }

        /// <summary>
        /// Determines whether the Zombie can swap positions with the specified square.
        /// </summary>
        /// <param name="targetSquare">The square to swap positions with.</param>
        /// <returns>
        ///   <c>false</c> since the Zombie cannot perform swapping.
        /// </returns>
        public override bool CanSwap(Square targetSquare)
        {
            return false;
        }
    }
}