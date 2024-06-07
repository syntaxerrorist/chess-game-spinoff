namespace AdvanceGame
{

    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets the color of the player.
        /// </summary>
        public Colour Colour { get; }

        /// <summary>
        /// Gets the army of units controlled by the player.
        /// </summary>
        public Army Army { get; }

        /// <summary>
        /// Gets the game instance the player belongs to.
        /// </summary>
        private Game Game { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="colour">The color of the player.</param>
        /// <param name="game">The game instance the player belongs to.</param>
        public Player(Colour colour, Game game) // may have to include Board board here
        {
            Colour = colour;
            Game = game;
            Army = new Army(this, Game.Board);
        }

        /// <summary>
        /// Retrieves the color of the player.
        /// </summary>
        /// <returns>The color of the player.</returns>
        public Colour PlayerColour()
        {
            return Colour;
        }

        /// <summary>
        /// Gets the direction value based on the player's color.
        /// </summary>
        /// <returns>The direction value (+1 or -1) indicating the direction based on the player's color.</returns>
        public int Direction
        {
            get
            {
                return Colour == Colour.Black ? +1 : -1;
            }
        }



        /// <summary>
        /// Converts the player object to its string representation.
        /// </summary>
        /// <returns>A string representation of the player object.</returns>
        public override string ToString()
        {
            return $"Player {Colour}";
        }

        /// <summary>
        /// Gets the opponent player of the current player based on current player's colour.
        /// </summary>
        /// <returns>The opponent player of the current player.</returns>
        public Player Opponent
        {
            get
            {
                if (Game.White.Colour == this.Colour)
                {
                    // if current player is white then opponent colour is black
                    return Game.Black;
                }
                else
                {   // if current player is black then opponent is white
                    return Game.White;
                }
            }
        }


        /// <summary>
        /// Chooses the best move for the player.
        /// </summary>
        /// <param name="lookAhead">The depth of the lookahead for evaluating moves.</param>
        /// <returns>The selected action representing the best move.</returns>
        public Action? ChooseMove(int lookAhead = 1)
        {

            // Store possible moves to choose from inside a list
            var possibleActions = new List<Action>();

            // Generate possible actions for each piece
            foreach (Piece piece in Army.Pieces)
            {
                foreach (var square in Game.Board.Squares)
                {
                    if (piece.CanMoveTo(square) && square.IsFree)
                    {
                        possibleActions.Add(new Move(piece, square));

                        if (piece is Builder && square.IsFree)
                            // Add build wall action to the possible actions list if the piece is a Builder and the square is free
                            possibleActions.Add(new BuildWall(piece, square));
                    }
                    else if (piece.CanAttack(square) && square.IsOccupied)
                    {
                        if (square.Occupant is Wall && piece is Miner)
                            // Add destroy wall action to the possible actions list if the piece is a Miner and the square has a Wall
                            possibleActions.Add(new DestroyWall(piece, square));
                        else if (piece.CanConvert)
                            // Add convert action to the possible actions list if the piece can convert
                            possibleActions.Add(new Convert(piece, square));
                        else
                            // Add attack action to the possible actions list
                            possibleActions.Add(new Attack(piece, square));
                    }
                    else if (piece.CanSwap(square))
                    {
                        // Add swap action to the possible actions list if the piece can perform a swap
                        possibleActions.Add(new Swap(piece, square));
                    }
                }
            }
            // Check if the General is under threat and remove actions that put the General under threat
            CheckGeneral(possibleActions);

            // Return the best possible move by calling the BestMove() method
            return BestMove(lookAhead, possibleActions);
        }

        /// <summary>
        /// Checks if the General is under threat for each possible action.
        /// </summary>
        /// <param name="possibleActions">List of possible actions to check.</param>
        public void CheckGeneral(List<Action> possibleActions)
        {
            // Create a copy of the piece list
            List<Piece> pieceList = Army.Pieces.ToList();
            int i = possibleActions.Count - 1; // increment backwards from last element within list
            while (i >= 0)
            {
                possibleActions[i].DoAction();


                // return true if action puts General under threat and if so, remove it from possible actions
                bool remove = pieceList.Any(piece => piece is General && piece.GetSquare().IsThreatened);

                possibleActions[i].UndoAction();

                // Remove the action from possible actions if the General is under threat
                if (remove)
                    if (remove)
                {
                    possibleActions.RemoveAt(i);
                }

                i--;
            }
        }


        /// <summary>
        /// Determines the best move by evaluating possible actions.
        /// </summary>
        /// <param name="lookAhead">Number of levels to look ahead in the move evaluation.</param>
        /// <param name="possibleActions">List of possible actions to evaluate.</param>
        /// <returns>The best action to take.</returns>
        public Action BestMove(int lookAhead, List<Action> possibleActions)
        {
            // If there is only one possible action, return it
            if (possibleActions.Count == 1)
            {
                return possibleActions[0];
            }

            Action bestMove = null;
            int score = 0;

            foreach (Action action in possibleActions)
            {
                action.DoAction();

                int currentScore = action.moveScore;

                if (lookAhead > 0)
                {
                    // Evaluate opponent's move recursively
                    Action enemyAction = Opponent.ChooseMove(lookAhead - 1);

                    // If no enemy action is available, return the current action
                    if (enemyAction == null)
                    {
                        action.UndoAction();
                        return action;
                    }
                    currentScore -= enemyAction.moveScore;
                }

                action.UndoAction();

                if (score == 0 && currentScore == 0)
                {
                    // If both the current score and the previous best score are 0,
                    // set the score to 1 to prioritize actions with non-zero scores.
                    score = 1;
                    bestMove = action;
                }
                else if (currentScore > score)
                {
                    // If the current score is greater than the previous best score,
                    // update the score to the current score and set the current action as the new best move.
                    score = currentScore;
                    bestMove = action;
                }
            }

            return bestMove;
        }
    }    
}