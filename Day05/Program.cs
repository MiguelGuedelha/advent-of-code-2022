using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var input = reader.ReadToEnd().Split($"{Environment.NewLine}{Environment.NewLine}");

var initialStackString = input[0].Split(Environment.NewLine);

var commands = input[1].Split(Environment.NewLine);

var stacks = new List<Stack<string>>();

var stackAmount = initialStackString[^1].Split(" ").Count(x => int.TryParse(x, out _));

for (int i = 0; i < stackAmount; i++)
{
    stacks.Add(new Stack<string>());
}

var bottomIndex = initialStackString.Length - 2;

for (int i = bottomIndex; i >= 0; i--)
{

    int index = 0;
    for (int j = 1; j < initialStackString[bottomIndex - 1].Length; j += 4)
    {
        var value = initialStackString[i][j].ToString();
        if (!string.IsNullOrWhiteSpace(value))
        {
            stacks[index].Push(initialStackString[i][j].ToString());
        }
        index++;
    }
}

foreach (var command in commands)
{
    ProcessCommand(command, stacks);
}

var result = GetTopOfStacks(stacks);

Console.ReadLine();

void ProcessCommand(string command, List<Stack<string>> stackList)
{
    var values = command.Split(" ");
    var quantity = int.Parse(values[1]);
    var from = int.Parse(values[3]) - 1;
    var to = int.Parse(values[5]) - 1;

    var temp = new Stack<string>();

    for (int i = 0; i < quantity; i++)
    {
        temp.Push(stacks[from].Pop());
    }

    for (int i = 0; i < quantity; i++)
    {
        stacks[to].Push(temp.Pop());
    }
}

string GetTopOfStacks(List<Stack<string>> stackList)
{
    return stackList
        .Select(x => x.Peek())
        .Aggregate("", (total, current) => $"{total}{current}");
}