using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacmanGame.Model;

namespace PacmanGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Position[] d = PacmanGame.DataLayer.GetLevelData.GetObstaclesPositions(0);

        }
    }
}
