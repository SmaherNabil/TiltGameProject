using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;


class Program
{
    static int dimensions;
    static char[,] initialBoard;
    static List<(int x, int y)> initialSliders = new List<(int, int)>();
    static (int xt, int yt) target;

    public static void Main(string[] args)
    {
        Console.WriteLine("Press 1 to run sample cases, 2 to run complete tests");
        string input = Console.ReadLine();
        string folderPath = "";
        
        if (input == "1")
        {

            folderPath = @"D:\d\fcis 2025\3rd year\semester 2\algo\Project\TiltGameProject\TiltGame\Test Cases\Sample Tests\";
            string[] filePaths = Directory.EnumerateFiles(folderPath, "*.txt").Where(file => !file.Contains("-output")).ToArray();
            int numCase = 0;
            foreach (string filePath in filePaths)
            {
                initialSliders.Clear();
                ReadBoard(filePath);
                Console.WriteLine("Case " + (numCase + 1) + " Running now Please Wait");

                Stopwatch stopwatch = Stopwatch.StartNew();
                List<string> solution = Solve();
                stopwatch.Stop();
                Console.WriteLine($"--- Time '{stopwatch.ElapsedMilliseconds / 1000}'Seconds ---");
                numCase++;
                //path for new file
                string filePathOUT = @"D:\d\fcis 2025\3rd year\semester 2\algo\Project\TiltGameProject\TiltGame\Test Cases\Sample Tests\";
                //change filePathOut variable to where you want to create teh new file

                //loop through each case
                int numberOfCases = 6;
                    // Get the name of the input file without the extension
                    string inputFileName = Path.GetFileNameWithoutExtension(filePath);

                    // Use the name of the input file to generate the name of the output file
                    string fileName = $"{inputFileName}-CaseOutput";
                for (int i = 1; i <= numberOfCases; i++)
                {
                    string outputFilePath = Path.Combine(filePathOUT, $"{fileName}.txt");

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputFilePath))
                        {
                            if (solution[0] == "Unsolvable")
                            {
                                writer.WriteLine("Unsolvable");
                                continue;
                            }
                            else
                            {
                                writer.WriteLine("Solvable");
                                writer.WriteLine($"Min number of moves: {solution.Count}");
                                writer.Write("Sequence of moves: ");
                                foreach (string move in solution)
                                {
                                    writer.Write(move + ", ");
                                }
                                writer.WriteLine();
                                //write the initial board state
                                writer.WriteLine("Initial");
                                char[,] initBoard = FillBoardWithSliders(initialSliders);
                                PrintBoardToFile(initBoard, writer);

                                //write each move and the resulting board configuration
                                List<(int x, int y)> currentSliders = new List<(int x, int y)>(initialSliders);
                                foreach (string move in solution)
                                {
                                    writer.WriteLine(move);
                                    currentSliders = Move(currentSliders, move);
                                    char[,] boardAfterMove = FillBoardWithSliders(currentSliders);
                                    PrintBoardToFile(boardAfterMove, writer);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error writing to file: " + ex.Message);
                    }
                }
                Console.WriteLine($"Successfully wrote to {fileName}.txt!");
                Console.WriteLine();
            }
        }
        else if (input == "2")
        {

            folderPath = @"D:\d\fcis 2025\3rd year\semester 2\algo\Project\TiltGameProject\TiltGame\Test Cases\Complete Tests\";
            string[] filePaths = Directory.EnumerateFiles(folderPath, "*.txt").Where(file => !file.Contains("-output")).ToArray();
            Console.WriteLine(filePaths.Length);
            int numCase=0;
            for(int z=0;z<filePaths.Length;z++)
            {
                initialSliders.Clear();
                ReadBoard(filePaths[z]);
                //Console.WriteLine($"--- Content of '{filePath}' ---");
                //printing the runtime for the complete test cases
                if(z==0)
                {
                    Console.WriteLine("Small Cases Execution");
                    
                }
                if (z == 3)
                {
                    Console.WriteLine();
                    Console.WriteLine("Meduim Cases Execution");
                    numCase = 0;
                }
                if (z == 6)
                {
                    Console.WriteLine();
                    Console.WriteLine("Large Cases Execution");
                    numCase = 0;
                }
                Console.WriteLine("Case "+(numCase+1)+" Running now Please Wait");
                Stopwatch stopwatch = Stopwatch.StartNew();
                List<string> solution = Solve();
                stopwatch.Stop();
                Console.WriteLine($"--- Time '{stopwatch.ElapsedMilliseconds / 1000}'Seconds ---");

                numCase++;
                //path for new file
                string filePathOUT = @"D:\d\fcis 2025\3rd year\semester 2\algo\Project\TiltGameProject\TiltGame\Test Cases\Complete Tests\";
                //change filePathOut variable to where you want to create teh new file

                //loop through each case
                int numberOfCases = 8;
                // Get the name of the input file without the extension
                string inputFileName = Path.GetFileNameWithoutExtension(filePaths[z]);

                // Use the name of the input file to generate the name of the output file
                string fileName = $"{inputFileName}-CaseOutput";
                for (int i = 1; i <= numberOfCases; i++)
                {
                    string outputFilePath = Path.Combine(filePathOUT, $"{fileName}.txt");

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputFilePath))
                        {
                            if (solution[0] == "Unsolvable")
                            {
                                writer.WriteLine("Unsolvable");
                                continue;
                            }
                            else
                            {
                                writer.WriteLine("Solvable");
                                writer.WriteLine($"Min number of moves: {solution.Count}");
                                writer.Write("Sequence of moves: ");
                                foreach (string move in solution)
                                {
                                    writer.Write(move + ", ");
                                }
                                writer.WriteLine();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error writing to file: " + ex.Message);
                    }
                }
                Console.WriteLine($"Successfully wrote to {fileName}.txt!");
            }
        }
        else
        {
            Console.WriteLine("Wrong input please Press 1 to run sample cases, 2 to run complete tests");
        }
    }
    public static void PrintBoardToFile(char[,] board, StreamWriter writer)
    {
        int dimensions = board.GetLength(0); // Assuming the board is a square

        for (int i = 0; i < dimensions; i++)
        {
            for (int j = 0; j < dimensions; j++)
            {
                // Write each cell followed by a comma and a space
                writer.Write(board[i, j] + ", ");
            }
            // Write a new line at the end of each row
            writer.WriteLine();
        }
    }
    public static void ReadBoard(string filePath)
    {
        string[] fileContent = File.ReadAllLines(filePath);
        dimensions = int.Parse(fileContent[0]);
        initialBoard = new char[dimensions, dimensions];
        for (int i = 1; i <= dimensions; i++)
        {
            char[] row = fileContent[i].Replace(", ", "").ToCharArray();
            for (int j = 0; j < dimensions; j++)
            {
                //every slider will be a . to save the board without any sliders 
                if (row[j] == 'o')
                {
                    initialSliders.Add(new ValueTuple<int, int>(i - 1, j));
                    initialBoard[i - 1, j] = '.';
                }
                else
                {
                    //save the values without changing anything
                    initialBoard[i - 1, j] = row[j];
                }
            }
        }

        string[] targetLocation = fileContent[fileContent.Length - 1].Split(',');
        target = (int.Parse(targetLocation[0]), int.Parse(targetLocation[1]));
        //Console.WriteLine($"Target location: {target.xt}, {target.yt}");
    }
    public static void PrintBoard(char[,] boardToDisplay)
    {
        // char[,] boardToDisplay= FillBoardWithSliders(newSliders);

        for (int i = 0; i < dimensions; i++)
        {
            for (int j = 0; j < dimensions; j++)
            {
                Console.Write(boardToDisplay[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
    public static char[,] FillBoardWithSliders(List<(int x, int y)> newSliders)
    {
        char[,] boardToFill = (char[,])initialBoard.Clone();
        for (int i = 0; i < newSliders.Count; i++)
        {
            boardToFill[newSliders[i].x, newSliders[i].y] = 'o';
        }
        return boardToFill;
    }
    public static List<string> valid(string lastMove)
    {
        List<string> result = new List<string>();
        if (lastMove == null)
        {
            result.Add("up");
            result.Add("down");
            result.Add("right");
            result.Add("left");
        }
        else
        {
            if (lastMove == "up" || lastMove == "down")
            {
                result.Add("right");
                result.Add("left");
            }
            else
            {
                result.Add("up");
                result.Add("down");
            }
        }
        return result;
    }
    public static List<(int x, int y)> Move(List<(int x, int y)> beforeSliders, string direction)
    {
        List<(int x, int y)> afterMoveSliders = new List<(int x, int y)>();
        char[,] boardToMove = FillBoardWithSliders(beforeSliders);
        int pointToReplace = -1;

        //direction up 
        if (direction == "up")
        {
            afterMoveSliders.Clear();
            for (int col = 0; col < dimensions; col++)
            {
                pointToReplace = -1;
                for (int row = 0; row < dimensions; row++)
                {
                    //to save the last space that can replaced with slider
                    if (boardToMove[row, col] == '.')
                    {
                        //and pointToreplace=-1
                        if (pointToReplace == -1)
                        {
                            pointToReplace = row;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (boardToMove[row, col] == 'o')
                    {
                        if (pointToReplace != -1)
                        {
                            boardToMove[row, col] = '.';
                            boardToMove[pointToReplace, col] = 'o';
                            afterMoveSliders.Add((pointToReplace, col));
                            pointToReplace++;
                        }
                        else
                        {
                            afterMoveSliders.Add((row, col));
                        }

                    }
                    else
                    {
                        pointToReplace = -1;
                    }

                }
            }
        }
        //direction down 
        if (direction == "down")
        {
            afterMoveSliders.Clear();
            //Console.WriteLine("in down");
            for (int col = 0; col < dimensions; col++)
            {
                pointToReplace = -1; // Reset this at start of each column.
                for (int row = dimensions - 1; row >= 0; row--) // Start from the last row.
                {
                    //to save the last space that can replaced with slider
                    if (boardToMove[row, col] == '.')
                    {
                        //and pointToReplace=-1
                        if (pointToReplace == -1)
                        {
                            pointToReplace = row;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (boardToMove[row, col] == 'o')
                    {
                        if (pointToReplace != -1)
                        {
                            boardToMove[row, col] = '.';
                            boardToMove[pointToReplace, col] = 'o';
                            afterMoveSliders.Add((pointToReplace, col));
                            pointToReplace--; // Decrease the replaceable point row index
                        }
                        else
                        {
                            afterMoveSliders.Add((row, col));
                        }
                    }
                    else
                    {
                        pointToReplace = -1; // Reset the replaceable point
                    }
                }
            }
        }
        //direction right
        if (direction == "right")
        {
            afterMoveSliders.Clear();
            // Console.WriteLine("in right");

            for (int row = 0; row < dimensions; row++)
            {
                pointToReplace = -1; // Reset this at start of each row.
                for (int col = dimensions - 1; col >= 0; col--) // Start from the last column.
                {
                    // Save the last space that can be replaced with slider
                    if (boardToMove[row, col] == '.')
                    {
                        // PointToReplace = -1
                        if (pointToReplace == -1)
                        {
                            pointToReplace = col;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (boardToMove[row, col] == 'o')
                    {
                        if (pointToReplace != -1)
                        {
                            // Move the slider to the right
                            boardToMove[row, col] = '.';
                            boardToMove[row, pointToReplace] = 'o';
                            afterMoveSliders.Add((row, pointToReplace));
                            pointToReplace--; // Move the replaceable point column index to the left.
                        }
                        else
                        {
                            afterMoveSliders.Add((row, col));
                        }
                    }
                    else
                    {
                        pointToReplace = -1; // Reset the replaceable point
                    }
                }
            }
        }
        // direction left
        if (direction == "left")
        {
            afterMoveSliders.Clear();
            // Console.WriteLine("in left");
            for (int row = 0; row < dimensions; row++)
            {
                pointToReplace = -1; // Reset this at start of each row.
                for (int col = 0; col < dimensions; col++) // Start from the first column.
                {
                    // to save the last space that can be replaced with slider
                    if (boardToMove[row, col] == '.')
                    {
                        // and pointToReplace=-1
                        if (pointToReplace == -1)
                        {
                            pointToReplace = col;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (boardToMove[row, col] == 'o')
                    {
                        if (pointToReplace != -1)
                        {
                            boardToMove[row, col] = '.';
                            boardToMove[row, pointToReplace] = 'o';
                            afterMoveSliders.Add((row, pointToReplace));
                            pointToReplace++; // Increment the replaceable point column index.
                        }
                        else
                        {
                            afterMoveSliders.Add((row, col));
                        }
                    }
                    else //obstacle 
                    {
                        pointToReplace = -1; // Reset the replaceable point
                    }
                }
            }
        }
        return afterMoveSliders;
    }
    public class BoardConfig
    {
        public List<string> moveSeq;
        public List<(int x, int y)> sliderPos;
    }
     public class SliderListComparer : IEqualityComparer<List<(int, int)>>
    {
        
        public bool Equals(List<(int, int)> x, List<(int, int)> y)
        {
            if (x.Count != y.Count)
                return false;

            for (int i = 0; i < x.Count; ++i)
            {
                if (x[i] != y[i])
                    return false;
            }

            return true;
        }

        public int GetHashCode(List<(int, int)> obj)
        {
            int hash = 19;

            foreach (var p in obj)
            {
                hash = hash * 31 + p.GetHashCode();
            }

            return hash;
        }
    }
    public static List<string> Solve()
    {

        Dictionary<List<(int, int)>, (List<(int, int)>, string)> route = new Dictionary<List<(int, int)>, (List<(int, int)>, string)>(new SliderListComparer());
        Queue<BoardConfig> myQ = new Queue<BoardConfig>();

        BoardConfig orig = new BoardConfig
        {
            moveSeq = new List<string>(),
            sliderPos = initialSliders
        };

        route[initialSliders] = (null, null);
        myQ.Enqueue(orig);

        while (myQ.Count > 0)
        {
            BoardConfig currentBMS = myQ.Dequeue();
            List<string> possibleMoves = valid(currentBMS.moveSeq.Count == 0 ? null : currentBMS.moveSeq.Last());

            foreach (string move in possibleMoves)
            {
                List<(int x, int y)> movedSliders = Move(new List<(int x, int y)>(currentBMS.sliderPos), move);

                if (!route.ContainsKey(movedSliders))
                {
                    route[movedSliders] = (currentBMS.sliderPos, move);
                    List<string> newMoveSeq = new List<string>(currentBMS.moveSeq) { move };

                    myQ.Enqueue(new BoardConfig { moveSeq = newMoveSeq, sliderPos = movedSliders });

                    for (int i = 0; i < movedSliders.Count; i++)
                    {
                       if( movedSliders[i].y== target.xt&& movedSliders[i].x == target.yt)
                        {
                            return newMoveSeq;
                        }
                    }
                }
            }
        }
        return new List<string> { "Unsolvable" };
    }
}