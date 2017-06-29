using Maze.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Maze
{
    public class FileReader
    {
        public List<MazePoint> ReadMazeData(string fileName)
        {
            var mazeData = new List<MazePoint>();

            var lines = File.ReadLines(fileName);
            foreach (var line in lines)
            {
                var elements = line.Split(' ');
                if (elements.Length != 3)
                {
                    throw new Exception("Incorrect file line!");
                }

                var directions = GetPointDirections(elements[2]);

                mazeData.Add(new MazePoint(int.Parse(elements[0]), int.Parse(elements[1]), directions));
            }

            return mazeData;
        }

        private List<Direction> GetPointDirections(string directionsInString)
        {
            var directions = new List<Direction>();
            foreach (var sign in directionsInString)
            {
                switch (sign)
                {
                    case 'P':
                        directions.Add(Direction.East);
                        break;
                    case 'D':
                        directions.Add(Direction.South);
                        break;
                    default:
                        break;
                }
            }

            return directions;
        }
    }
}
