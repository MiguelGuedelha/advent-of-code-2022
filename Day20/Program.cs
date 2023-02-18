using Day20;
using Shared.Extensions;
using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd();

var file = inputString.Split(Environment.NewLine).Select(x => new Number { Value = int.Parse(x) }).ToList();

//file = new[] { 8, 2, 32, -41, 6, 29, -4, 6, -8, 8, -3, -8, 3, -5, 0, -1, 2, 1, 10, -9 }.Select(x => new Number { Value = x }).ToList();

var order = new List<Number>(file);

//Console.WriteLine(string.Join(", ", file.Select(x => x.Value)));
foreach (var number in order)
{
    var index = file.IndexOf(number);
    MoveValue(index, file);
    //Console.WriteLine(string.Join(", ", file.Select(x => x.Value)));
}

var finalOutFile = file.Select(x => x.Value).ToList();
var firstZeroIndex = file.IndexOf(file.First(x => x.Value == 0));
var resultP1 = new[] { 1000, 2000, 3000 }.Select(x => file[(firstZeroIndex + x) % file.Count].Value).Sum();

Console.ReadLine();

static void MoveValue(int index, List<Number> circularFile)
{
    var number = circularFile[index];
    var value = number.Value;

    if (value == 0)
    {
        return;
    }

    var size = circularFile.Count;

    if (value > 0)
    {
        if (value > size)
        {
            size--;
        }
        var newIndex = (index + value).Mod(size) < index ? (index + value + 1).Mod(size) : (index + value).Mod(size);
        circularFile.RemoveAt(index);
        circularFile.Insert(newIndex, number);
    }
    else
    {
        var newIndex = index + value <= 0 ? (index + value - 1).Mod(size) : (index + value).Mod(size);
        if (Math.Abs(value) > size)
        {
            size--;
            newIndex = (index + value).Mod(size);
        }
        circularFile.RemoveAt(index);
        circularFile.Insert(newIndex, number);
    }
}