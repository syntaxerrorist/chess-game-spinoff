namespace AdvanceGame
{
    /// <summary>
    /// The main program class.
    /// </summary>
    class Program
    {
        const string botName = "Cagnus Marlsen Bot"; // bot name


        /// <summary>
        /// The entry point of the program.
        /// 
        /// This method is responsible for parsing command line arguments, loading a game from a file, performing a move by the current player,
        /// generating the game board, and handling various exceptions that may occur during the process.
        /// 
        /// <param name="args">An array of command line arguments.
        ///     The arguments should follow the format: [color] [inFile] [outFile].
        ///     The 'color' argument specifies the current player's color ("white" or "black").
        ///     The 'inFile' argument is the path to the file containing the initial game state.
        ///     The 'outFile' argument is the path to the file where the updated game board will be saved.</param>
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                // return bot name when argument supplied is "name"
                if (args.Length == 1 && args[0].ToLower() == "name")
                {
                    Console.WriteLine(botName);
                    return;
                }

                // if number of command line arguments is not equal to 3, throw error
                if (args.Length != 3)
                {
                    throw new ArgumentException("Invalid number of arguments");
                }

                var game = new Game();
                var colour = args[0].ToLower();
                var inFile = args[1];
                var outFile = args[2];

                // Load the game from file
                game.LoadFile(inFile);

                // Choose the current player and perform the move
                var currentPlayer = colour == "white" ? game.White : game.Black;
                currentPlayer.ChooseMove().DoAction();

                // Generate the game board and save it to file
                game.GenerateGameBoard(outFile);

            }
            // Handle file not found exception
            catch (FileNotFoundException ex) 
            {
                Console.WriteLine($"File not found: {ex.FileName}");
            }
            // Handle I/O error exception
            catch (IOException ex)
            {
                Console.WriteLine($"An I/O error occurred: {ex.Message}");
            }
            // Handle other exceptions
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.ToString()}");
            }
        }
    }
}
