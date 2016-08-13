using System;
using PacmanGame.Model;

namespace PacmanGame.Utilities
{
    public static class ArrayExtensions
    {
        public static Position[] Add(this Position[] coordsArray, Position newPosition)
        {
            Position[] returnArray;
            if (coordsArray == null)
            {
                returnArray = new[]
                {
                    newPosition
                };
            }
            else
            {
                returnArray = new Position[coordsArray.Length + 1];
                Array.Copy(coordsArray, 0, returnArray, 0, coordsArray.Length);
                returnArray[returnArray.Length - 1] = newPosition;
            }
            return returnArray;
        }
        public static Dot[] Add(this Dot[] coordsArray, Dot newPosition)
        {
            Dot[] returnArray;
            if (coordsArray == null)
            {
                returnArray = new[]
                {
                    newPosition
                };
            }
            else
            {
                returnArray = new Dot[coordsArray.Length + 1];
                Array.Copy(coordsArray, 0, returnArray, 0, coordsArray.Length);
                returnArray[returnArray.Length - 1] = newPosition;
            }
            return returnArray;
        }

        public static Obstacle[] Add(this Obstacle[] coordsArray, Obstacle newPosition)
        {
            Obstacle[] returnArray;
            if (coordsArray == null)
            {
                returnArray = new[]
                {
                    newPosition
                };
            }
            else
            {
                returnArray = new Obstacle[coordsArray.Length + 1];
                Array.Copy(coordsArray, 0, returnArray, 0, coordsArray.Length);
                returnArray[returnArray.Length - 1] = newPosition;
            }
            return returnArray;
        }
    }
}