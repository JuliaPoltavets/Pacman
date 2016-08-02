namespace PacmanGame.Model
{
    public struct Cell
    {
        /// <summary>
        /// All characters have these properties
        /// </summary>
        public Position _position;
        public UniqueTypeIdentifiers _characterId; // in case of Cell we will have here id of character that occupies the cell
    }
}