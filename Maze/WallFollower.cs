using Maze.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maze
{
    public class WallFollower
    {
        private readonly List<MazePoint> _mazeData;

        private List<Point> _movments = new List<Point>();

        public WallFollower(List<MazePoint> mazeData)
        {
            _mazeData = mazeData;
            BuildMaze(mazeData);
        }

        private void BuildMaze(List<MazePoint> mazeData)
        {
            var maxX = mazeData.OrderByDescending(m => m.X).FirstOrDefault().X;
            var maxY = mazeData.OrderByDescending(m => m.Y).FirstOrDefault().Y;
            
            AddLastColumn(maxX, maxY);
            AddLastRow(maxX, maxY);
            AddMissingDirectionToMazeData();

            _mazeData.Add(new MazePoint(maxX + 1, maxY + 1, new List<Maze.Direction> { Direction.North, Direction.West }));

        }

        public void SolveMaze()
        {
            if (_mazeData == null || _mazeData.Count == 0)
            {
                throw new Exception("Maze Data could not be empty!");
            }

            Console.WriteLine("Solving the maze. Please wait...");

            var startPosition = GetStartPosition();
            var endPosition = GetEndPosition();

            var currentPosition = startPosition;
            var previousPosition = new Point(1, 0);

            _movments.Add(currentPosition);

            while (true)
            {
                var deltaX = currentPosition.X - previousPosition.X;
                var deltaY = currentPosition.Y - previousPosition.Y;
                var facingDirection = GetFacingDirection(deltaX, deltaY);

                var movmentPossibilities = GetMovmentPossibilities(facingDirection);

                var isEndReached = false;

                foreach (var movmentPossibility in movmentPossibilities)
                {
                    var pointFromMazeData = _mazeData.FirstOrDefault(m => m.X == currentPosition.X && m.Y == currentPosition.Y);            

                    if  (pointFromMazeData.Directions.Contains(movmentPossibility))
                    {
                        previousPosition = currentPosition;
                        currentPosition = GetNewPointBasedOnDirection(currentPosition, movmentPossibility);
                        _movments.Add(currentPosition);

                        if (currentPosition.X == endPosition.X && currentPosition.Y == endPosition.Y)
                        {
                            isEndReached = true;
                        }
                        break;
                    }
                }

                if (isEndReached)
                {
                    break;
                }

            }
        }

        public void DisplaySolution()
        {
            Console.WriteLine("**** STEP TO SOLVE THE MAZE ****\n\n");

            int step = 1;
            foreach (var move in _movments)
            {
                Console.WriteLine($"{step} STEP: X = {move.X}, Y = {move.Y}");
                ++step;
            }
        }

        private Point GetStartPosition()
        {
            return _mazeData.OrderBy(m => m.X).ThenBy(m => m.Y).FirstOrDefault();
        }

        private Point GetEndPosition()
        {
            return _mazeData.OrderByDescending(m => m.X).ThenByDescending(m => m.Y).FirstOrDefault();
        }

        private Direction GetFacingDirection(int deltaX, int deltaY)
        {
            if (deltaX == 1)
            {
                return Direction.East;
            }
            else if (deltaX == -1)
            {
                return Direction.West;
            }
            else
            {
                if (deltaY == 1)
                {
                    return Direction.South;
                }
                else if (deltaY == -1)
                {
                    return Direction.North;
                }
            }

            throw new Exception("Can not compute facing direction!");
        }

        private IEnumerable<Direction> GetMovmentPossibilities(Direction facingDirection)
        {
            var movmentPossibility = new Direction[4];

            switch (facingDirection)
            {
                case Direction.North:
                    movmentPossibility[0] = Direction.West;
                    movmentPossibility[1] = Direction.North;
                    movmentPossibility[2] = Direction.East;
                    movmentPossibility[3] = Direction.South;
                    break;
                case Direction.South:
                    movmentPossibility[0] = Direction.East;
                    movmentPossibility[1] = Direction.South;
                    movmentPossibility[2] = Direction.West;
                    movmentPossibility[3] = Direction.North;
                    break;
                case Direction.East:
                    movmentPossibility[0] = Direction.North;
                    movmentPossibility[1] = Direction.East;
                    movmentPossibility[2] = Direction.South;
                    movmentPossibility[3] = Direction.West;
                    break;
                case Direction.West:
                    movmentPossibility[0] = Direction.South;
                    movmentPossibility[1] = Direction.West;
                    movmentPossibility[2] = Direction.North;
                    movmentPossibility[3] = Direction.East;
                    break;
                default:
                    break;
            }

            return movmentPossibility;
        }

        private Point GetNewPointBasedOnDirection(Point point, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return new Point(point.X, point.Y - 1);
                case Direction.South:
                    return new Point(point.X, point.Y + 1);
                case Direction.East:
                    return new Point(point.X + 1, point.Y);
                case Direction.West:
                    return new Point(point.X -1, point.Y);
            }

            throw new Exception("Could not compute new point!");
        }

        private void AddMissingDirectionToMazeData()
        {
            foreach (var maze in _mazeData)
            {
                if (maze.Directions.Contains(Direction.East))
                {
                    var foundElement = _mazeData.FirstOrDefault(m => m.X == maze.X + 1 && m.Y == maze.Y);
                    if (foundElement != null)
                    {
                        if (!foundElement.Directions.Contains(Direction.West))
                        {
                            foundElement.Directions.Add(Direction.West);
                        }
                    }
                }

                if (maze.Directions.Contains(Direction.South))
                {
                    var foundElement = _mazeData.FirstOrDefault(m => m.X == maze.X && m.Y == maze.Y + 1);
                    if (foundElement != null)
                    {
                        if (!foundElement.Directions.Contains(Direction.North))
                        {
                            foundElement.Directions.Add(Direction.North);
                        }
                    }
                }
            }
        }

        private void AddLastColumn(int maxX, int maxY)
        {
            for (int i = 0; i <= maxY; i++)
            {
                var foundMazePoint = _mazeData.FirstOrDefault(m => m.Y == i && m.X == maxX);
                var directionsToAdd = new List<Direction>
                {
                    Direction.South
                };
                if (foundMazePoint != null)
                {
                    if (foundMazePoint.Directions.Contains(Direction.East))
                    {
                        directionsToAdd.Add(Direction.West);
                    }
                }

                _mazeData.Add(new MazePoint(maxX + 1, i, directionsToAdd));
            }
        }

        private void AddLastRow(int maxX, int maxY)
        {
            for (int i = 0; i <= maxX; i++)
            {
                var foundMazePoint = _mazeData.FirstOrDefault(m => m.X == i && m.Y == maxY);
                var directionsToAdd = new List<Direction>
                {
                    Direction.East
                };

                if (foundMazePoint != null)
                {
                    if (foundMazePoint.Directions.Contains(Direction.South))
                    {
                        directionsToAdd.Add(Direction.North);
                    }
                }

                _mazeData.Add(new MazePoint(i, maxY + 1, directionsToAdd));
            }
        }
    }
}
