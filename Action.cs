using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvanceGame
{

    /// <summary>
    /// Abstract base class for game actions.
    /// </summary>
    public abstract class Action
    {
        /// <summary>
        /// The piece performing the action.
        /// </summary>
        protected Piece Piece { get; }

        /// <summary>
        /// The target square of where the action is performed on.
        /// </summary>
        public Square Target { get; }

        /// <summary>
        /// The score associated with the move.
        /// </summary>
        public int moveScore = 0;

        /// <summary>
        /// Map of piece names to their respective scores.
        /// </summary>
        protected IDictionary<string, int> pieceScoresMap = new Dictionary<string, int>()
        {
            ["Zombie"] = 1,
            ["Builder"] = 2,
            ["Jester"] = 3,
            ["Miner"] = 4,
            ["Sentinel"] = 5,
            ["Catapult"] = 6,
            ["Dragon"] = 7,
            ["General"] = 10
        };

        /// <summary>
        /// Constructs an Action object.
        /// </summary>
        /// <param name="actor">The piece performing the action.</param>
        /// <param name="target">The target square of the action.</param>
        public Action(Piece actor, Square target)
        {
            Piece = actor;
            Target = target;
        }

        /*
         * Below are declared methods inside the Action class intended to be overriden
         * by the derived classes that inherit from it
         * These derived classes are: Move, Attack, Convert, Swap, BuildWall and DestroyWall
         */

        /// <summary>
        /// Performs the action.
        /// </summary>
        public abstract void DoAction();

        /// <summary>
        /// Undoes the action.
        /// </summary>
        public abstract void UndoAction();

        /// <summary>
        /// Sets the move score for the action.
        /// </summary>
        protected abstract void SetMoveScore();

    }


    /// <summary>
    /// Represents a Move action, where a piece is moved to a target square.
    /// </summary>
    public class Move : Action
    {
        // The original square from which the piece was moved (nullable)
        Square? originalSquare { get; }

        /// <summary>
        /// Constructs a Move action with its corresponding score value.
        /// </summary>
        /// <param name="piece">The piece being moved.</param>
        /// <param name="target">The target square to move the piece to.</param>
        public Move(Piece piece, Square target) : base(piece, target)
        {
            originalSquare = (piece != null) ? piece.GetSquare() : null;
            SetMoveScore();
        }

        /// <summary>
        /// Performs the move action by moving the piece to the target square.
        /// </summary>
        public override void DoAction()
        {
            Piece.MoveTo(Target);
        }

        /// <summary>
        /// Undoes the move action by moving the piece back to its original square (if available).
        /// </summary>
        public override void UndoAction()
        {
            Piece.LeaveSquare();

            if (originalSquare != null)
            {
                Piece.PieceSquare(originalSquare);
            }
        }

        /// <summary>
        /// Sets the move score for the move action. In this case, the move score is set to 0.
        /// </summary>
        protected override void SetMoveScore()
        {
            moveScore = 0;
        }

    }

    /// <summary>
    /// Represents an Attack action, where a piece attacks an opponent piece on a target square.
    /// </summary>
    public class Attack : Action
    {
        Square? originalSquare;
        Piece? opponentPiece;
        Player? opponent;


        /// <summary>
        /// Constructs an Attack action.
        /// </summary>
        /// <param name="piece">The attacking piece.</param>
        /// <param name="target">The target square to attack.</param>
        public Attack(Piece piece, Square target) : base(piece, target)
        {
            originalSquare = piece.GetSquare();
            opponentPiece = target.Occupant;
            opponent = opponentPiece?.GetPlayer();
            SetMoveScore();
        }


        /// <summary>
        /// Performs the attack action by attacking the target square.
        /// </summary>
        public override void DoAction()
        {
            Piece.Attack(Target);
        }


        /// <summary>
        /// Undoes the attack action by reverting the changes made during the attack.
        /// </summary>
        public override void UndoAction()
        {
            Piece.LeaveSquare();

            if (opponentPiece != null)
            {
                if (opponentPiece.OnBoard) {
                    opponentPiece.LeaveSquare();
                }

                opponentPiece.PieceSquare(Target);

                if (opponentPiece.GetPlayer() != opponent)
                {
                    opponentPiece.Defect();
                }
            }

            Piece.PieceSquare(originalSquare);
        }

        /// <summary>
        /// Sets the move score for the attack action based on the type of opponent piece.
        /// </summary>
        protected override void SetMoveScore()
        {
            int score = 0;
            if (!Target.IsFree)
            {
                string targetPieceType = Target.GetPieceType().Name;
                foreach (var pair in pieceScoresMap)
                {
                    if (pair.Key == targetPieceType)
                    {
                        score += pair.Value;
                    }
                }
            }

            moveScore += score;
        }

    }

    /// <summary>
    /// Represents a Convert action, where a piece converts the opponent's piece on the target square.
    /// </summary>
    public class Convert : Action
    {

        /// <summary>
        /// The piece that will be converted.
        /// </summary>
        Piece? ConvertedPiece { get; }

        /// <summary>
        /// Constructs a Convert action.
        /// </summary>
        /// <param name="piece">The piece performing the conversion.</param>
        /// <param name="target">The target square to convert the opponent's piece.</param>
        public Convert(Piece piece, Square target) : base(piece, target)
        {
            ConvertedPiece = target.Occupant;
            SetMoveScore();
        }


        /// <summary>
        /// Performs the convert action by converting the opponent's piece on the target square.
        /// </summary>
        public override void DoAction()
        {
            if (Target != null && ConvertedPiece != null)
            {
                ConvertedPiece.Defect();
            }
        }

        /// <summary>
        /// Undoes the convert action by reapplying the conversion.
        /// </summary>
        public override void UndoAction()
        {
            DoAction();
        }

        /// <summary>
        /// Sets the move score for the convert action based on the type of opponent's piece.
        /// </summary>
        protected override void SetMoveScore()
        {
            int score = 0;
            if (!Target.IsFree)
            {
                string targetPieceType = Target.GetPieceType().Name;
                foreach (var pair in pieceScoresMap)
                {
                    if (pair.Key == targetPieceType)
                    {
                        score += pair.Value * 2;
                    }
                }
            }
            moveScore += score;
        }
    }

    /// <summary>
    /// Represents a Swap action, where a piece swaps places with the piece on the target square.
    /// </summary>
    public class Swap : Action
    {

        /// <summary>
        /// The piece that is swapped with the current piece.
        /// </summary>
        Piece? SwappedPiece { get; }

        /// <summary>
        /// The original square of the current piece before the swap.
        /// </summary>
        Square? OriginalSquare;


        /// <summary>
        /// Constructs a Swap action.
        /// </summary>
        /// <param name="piece">The piece performing the swap.</param>
        /// <param name="target">The target square to swap with.</param>
        public Swap(Piece piece, Square target) : base(piece, target)
        {
            SwappedPiece = Target.Occupant;
            OriginalSquare = Piece.GetSquare();
            SetMoveScore();
        }

        /// <summary>
        /// Performs the swap action by swapping places between the current piece and the piece on the target square.
        /// </summary>     
        public override void DoAction()
        {
            if (Target.Occupant != null)
                Target.Occupant.LeaveSquare();

            Piece.MoveTo(Target);

            if (SwappedPiece != null)
                SwappedPiece.PieceSquare(OriginalSquare);
        }


        /// <summary>
        /// Undoes the swap action by reverting the swap and placing the pieces back to their original squares.
        /// </summary>
        public override void UndoAction()
        {
            if (SwappedPiece != null)
                SwappedPiece.LeaveSquare();

            Piece.MoveTo(OriginalSquare);

            if (SwappedPiece != null)
                SwappedPiece.PieceSquare(Target);
        }

        /// <summary>
        /// Sets the move score for the swap action based on the type of the piece on the target square.
        /// </summary>
        protected override void SetMoveScore()
        {
            int score = 0;
            if (!Target.IsFree)
            {
                string targetPieceType = Target.GetPieceType().Name;
                foreach (var pair in pieceScoresMap)
                {
                    if (pair.Key == targetPieceType)
                    {
                        score += 1;
                    }
                }
            }

            moveScore += score;
        }
    }

    /// <summary>
    /// Represents a BuildWall action, where a piece builds a wall on the target square.
    /// </summary>
    public class BuildWall : Action
    {

        /// <summary>
        /// Constructs a BuildWall action.
        /// </summary>
        /// <param name="piece">The piece performing the wall build.</param>
        /// <param name="target">The target square to build the wall on.</param>
        public BuildWall(Piece piece, Square target) : base(piece, target) { SetMoveScore(); }

        /// <summary>
        /// Performs the build wall action by creating a wall on the target square.
        /// </summary>
        public override void DoAction()
        {

            var wall = new Wall(Piece.GetPlayer(), Target); // Wall class will take over so that Wall piece is assigned to neither players
        }

        /// <summary>
        /// Undoes the build wall action by removing the wall from the target square.
        /// </summary>
        public override void UndoAction()
        {
            if (Target.Occupant != null)
            {
                Target.Occupant.LeaveSquare();
            }
        }

        /// <summary>
        /// Sets the move score for the build wall action.
        /// </summary>
        protected override void SetMoveScore()
        {
            moveScore += 1;
        }
    }
    /// <summary>
    /// Represents a DestroyWall action, where a piece destroys a wall and occupies the target square.
    /// This can only be performed by a Miner piece
    /// </summary>
    public class DestroyWall : Action
    {

        /// <summary>
        /// The original square of the current piece before the destroy operation.
        /// </summary>
        Square? OriginalSquare;

        /// <summary>
        /// The original occupant (piece) of the target square before the destroy operation.
        /// </summary>
        Piece? OriginalOccupant;

        /// <summary>
        /// Constructs a DestroyWall action.
        /// </summary>
        /// <param name="piece">The piece performing the destroy operation.</param>
        /// <param name="target">The target square to destroy the wall and occupy.</param>
        public DestroyWall(Piece piece, Square target) : base(piece, target)
        {
            OriginalSquare = Piece.GetSquare();
            OriginalOccupant = Target.Occupant;
        }

        /// <summary>
        /// Performs the destroy wall action by destroying the wall on the target square and occupying it.
        /// </summary>
        public override void DoAction()
        {
            if (!(Target.Occupant is Wall))
                throw new InvalidOperationException("Target must be a Wall.");

            Target.Occupant.LeaveSquare();
            Piece.MoveTo(Target);
        }

        /// <summary>
        /// Undoes the destroy wall action by reverting the destroy operation and restoring the original pieces 
        /// to their respective squares.
        /// Throws an <see cref="InvalidOperationException"/> if the original piece does not exist or if the square
        /// is invalid and not on the board.
        public override void UndoAction()
        {
            if (OriginalOccupant == null)
                throw new InvalidOperationException("Original Piece does not exist!");

            if (OriginalSquare == null)
                throw new InvalidOperationException("Invalid square. Square not on board!");

            Piece.MoveTo(OriginalSquare);
            OriginalOccupant.PieceSquare(Target);
        }

        /// <summary>
        /// Sets the move score for the destroy wall action.
        /// </summary>
        protected override void SetMoveScore()
        {
            moveScore += 1;
        }
    }
}
