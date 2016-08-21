using System;
using System.Runtime.CompilerServices;
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

        //public static Dot[] Remove(this Dot[] coordsArray, Position dotToRemove)
        //{
        //    Dot[] returnArray;
        //    if (coordsArray == null)
        //    {
        //        returnArray = null;
        //    }
        //    else
        //    {
        //        returnArray = new Dot[coordsArray.Length - 1];
        //        int? dotToRemoveIndex;
        //        if (TryFindDotInArray(coordsArray, dotToRemove, out dotToRemoveIndex))
        //        {
        //            Array.Copy(coordsArray, 0, returnArray, 0, dotToRemoveIndex);
        //            Array.Copy(_students, stIndex + 1, updatedGroup, stIndex, (_students.Length - 1) - stIndex);
        //            _students = updatedGroup;
        //        }
                
        //        returnArray[returnArray.Length - 1] = newPosition;
        //    }
        //    return returnArray;
        //}

        public static Dot[] RemoveDotByIndex(this Dot[] dotsArray, int indexToRemove)
        {
            Dot[] returnArray = null;
            if ((indexToRemove < 0) || (indexToRemove > dotsArray.Length))
            {
                returnArray = new Dot[dotsArray.Length];
                Array.Copy(dotsArray, 0, returnArray, 0, dotsArray.Length);
            }
            else
            {
                returnArray = new Dot[dotsArray.Length - 1];
                Array.Copy(dotsArray, 0, returnArray, 0, indexToRemove);
                Array.Copy(dotsArray, indexToRemove + 1, returnArray, indexToRemove,
                    (dotsArray.Length - 1) - indexToRemove);
            }
            return returnArray;
        }

        public static Dot[] RemoveDotByPosition(this Dot[] dotsArray, Position position)
        {
            int? index = Int32.MinValue;
            Dot[] returnArray = null;
            if (TryFindDotInArray(dotsArray, position, out index))
            {
                returnArray = RemoveDotByIndex(dotsArray, index.Value);
            }
            return returnArray;
        }


        private static bool TryFindDotInArray(Dot[] coordsArray, Position dotToRemove, out int? index)
        {
            bool itemFound = false;
            index = null;
            for (int i = 0; i < coordsArray.Length; i++)
            {
                if (coordsArray[i]._position.Equals(dotToRemove))
                {
                    itemFound = true;
                    index = i;
                }
            }
            return itemFound;
        }
    }
}