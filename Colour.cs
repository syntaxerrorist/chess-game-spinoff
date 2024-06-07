namespace AdvanceGame

    /// <summary>
    /// Represents the colour of each player in the game. Wall is classified its own since neither players can control it.
    /// </summary>
{
    public enum Colour
    {
        White,       
        Black,       
        Wall, // walls are assigned to a non-controllable player of colour 'Wall'
    }
}
