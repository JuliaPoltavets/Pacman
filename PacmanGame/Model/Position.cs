namespace PacmanGame.Model
{
    public struct Position
    {
        public int _x;
        public int _y;
        
        public void ChangePosition(MoveDirections direction, int step)
        {
            if ((direction == MoveDirections.Left) || (direction == MoveDirections.Right))
            {
                _x += step;
            }
            else
            {
                _y += step;
            }
        }

        public bool Equals(Position otherPosition)
        {
            return ((_x == otherPosition._x) && (_y == otherPosition._y));
        }
    }
}