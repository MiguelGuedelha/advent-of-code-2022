using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd();

var ops = inputString.Split(Environment.NewLine);

var registerValues = new Dictionary<int, int> { { 1, 1 } };
var currentCycle = 1;

for (int i = 0; i < ops.Length; i++)
{
    var op = ops[i].Substring(0, 4);
    if (op == "addx")
    {
        var value = int.Parse(ops[i].Substring(5));
        registerValues[currentCycle + 1] = registerValues[currentCycle];
        registerValues[currentCycle + 2] = registerValues[currentCycle] + value;
        currentCycle += 2;
    }
    else
    {
        registerValues[currentCycle + 1] = registerValues[currentCycle];
        currentCycle++;
    }
}

var strengthSum = 0;
var crtScreen = new string[6, 40];
currentCycle = 1;

for (int i = 0; i < 6; i++)
{
    for (int j = 0; j < 40; j++)
    {
        var registerVal = registerValues[currentCycle] - 1;
        var spritePos = new[] { registerVal, registerVal + 1, registerVal + 2 };

        Console.Write(spritePos.Contains(j) ? "#" : ".");
        currentCycle++;

    }
    Console.WriteLine();
}

Console.WriteLine(strengthSum);
Console.ReadLine();