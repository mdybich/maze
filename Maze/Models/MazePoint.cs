using Maze.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze.Models
{
    public class MazePoint : Point
    {
        public MazePoint(int x, int y, List<Direction> directions) : base(x, y)
        {
            Directions = directions;
        }

        public List<Direction> Directions { get; set; }
    }
}
