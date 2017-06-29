namespace Maze
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileReader = new FileReader();
            var mazeData = fileReader.ReadMazeData("FullMazeData.txt");

            var wallFolower = new WallFollower(mazeData);
            wallFolower.SolveMaze();
            wallFolower.DisplaySolution();
        }
    }
}
