using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd();

var heightGrid = inputString.Split(Environment.NewLine)
    .Select(x => x.ToCharArray())
    .Select(x => Array.ConvertAll(x, c => (int)char.GetNumericValue(c)))
    .ToArray();

var scoreGrid = new int[heightGrid.Length, heightGrid[0].Length];

for (var i = 0; i < heightGrid.Length; i++)
{
    for (var j = 0; j < heightGrid[0].Length; j++)
    {
        scoreGrid[i, j] = ScenicScore(heightGrid, i, j);
    }
}

var max = 0;
foreach (var score in scoreGrid)
{
    if (score > max)
    {
        max = score;
    }
}

Console.WriteLine(max);
Console.ReadLine();

int ScenicScore(int[][] grid, int row, int col)
{
    var visible = new[] { 0, 0, 0, 0 };

    for (var i = row + 1; i < grid.Length; i++)
    {
        visible[0] = Math.Abs(i - row);
        if (grid[i][col] >= grid[row][col])
        {
            break;
        }
    }

    for (var i = row - 1; i >= 0; i--)
    {
        visible[1] = Math.Abs(i - row);
        if (grid[i][col] >= grid[row][col])
        {
            break;
        }
    }

    for (var i = col + 1; i < grid[row].Length; i++)
    {
        visible[2] = Math.Abs(i - col);
        if (grid[row][i] >= grid[row][col])
        {
            break;
        }
    }

    for (var i = col - 1; i >= 0; i--)
    {
        visible[3] = Math.Abs(i - col);
        if (grid[row][i] >= grid[row][col])
        {
            break;
        }
    }

    return visible.Aggregate(1, (total, current) => total * current);
}