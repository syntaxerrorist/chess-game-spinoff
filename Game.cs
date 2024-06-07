using System.ComponentModel;

namespace AdvanceGame
{
    /// <summary>
    /// Represents a game of Advance.
    /// </summary>
    public class Game
    {

        /// <summary>
        /// Gets the player controlling the White pieces.
        /// </summary>
        public Player White { get; }

        /// <summary>
        /// Gets the player controlling the Black pieces.
        /// </summary
        public Player Black { get; }

        /// <summary>
        /// Gets the game board.
        /// </summary>
        public Board Board { get; }

        /// <summary>
        /// Initializes a new instance of the Game class.
        /// </summary>
        private Player? Wall { get; }

        public Game()
        {
            Board = new Board();
            White = new Player(Colour.White, this);
            Black = new Player(Colour.Black, this);           
            Wall = new Player(Colour.Wall, this); // wall player to assign for wall pieces
        }

        /// <summary>
        /// Loads a game board from a file.
        /// </summary>
        /// <param name="filePath">The path of the file to load.</param>
        /// <exception cref="FileNotFoundException">Thrown when the specified file is not found.</exception>
        /// <exception cref="IOException">Thrown when an I/O error occurs while reading the file.</exception>
        /// <exception cref="Exception">Thrown when an error occurs while loading the file.</exception>
        internal void LoadFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {

                try
                {
                    for (int row = 0; row < 9; row++)
                    {
                        string line = sr.ReadLine();
                        for (int col = 0; col < 9; col++)
                        {
                            Square currentSquare = Board.Get(row, col);
                            char icon = line[col];
                            if (icon != '.' && icon != ' ')
                            {
                                if (icon == '#')
                                {
                                    Wall.Army.AssignPiecesToPlayer(icon, currentSquare);
                                }
                                else
                                {
                                    Player currentPlayer = Char.IsLower(icon) ? Black : White;
                                    currentPlayer.Army.AssignPiecesToPlayer(icon, currentSquare);
                                }
                            }
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"File not found: {ex.FileName}");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"An I/O error occurred while reading the file: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while loading the file: {ex.ToString()}");
                }
            }
        }


        /// <summary>
        /// Generates a game board and writes it to a file.
        /// </summary>
        /// <param name="outfile">The path of the output file.</param>
        /// <exception cref="UnauthorizedAccessException">Thrown when unauthorized access occurs while writing the file.</exception>
        /// <exception cref="Exception">Thrown when an error occurs while writing the file.</exception>
        internal void GenerateGameBoard(string outfile)
        {
            try
            {
                using StreamWriter f = new StreamWriter(outfile);

                for (int row = 0; row < Board.Size; row++)
                {
                    for (int col = 0; col < Board.Size; col++)
                    {
                        Square currentSquare = Board.Get(row, col);

                        if (currentSquare.IsOccupied)
                        {
                            f.Write(currentSquare.Occupant.Icon);
                        }
                        else
                        {
                            f.Write('.'); // empty squares are signified with '.'
                        }
                    }

                    f.WriteLine();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Unauthorized access error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing file '{outfile}': {ex.Message}");
            }
        }

        /// <summary>
        /// Returns a string representation of the Game object.
        /// </summary>
        /// <returns>A string representation of the Game object.</returns>
        public override string ToString()
        {
            return $"Game:\n{White}\n{Black}\n{Board}";
        }
    }
}
