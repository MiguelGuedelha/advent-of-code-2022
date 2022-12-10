using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var input = reader.ReadToEnd().ToCharArray();
var sequenceLength = 14;

var result = -1;

for (var i = 0; i < input.Length; i++)
{
    var end = i + sequenceLength;
    var sequence = input[i..end];
    var isStart = sequence.Distinct().Count() == sequenceLength;
    if (isStart)
    {
        result = end;
        break;
    }
}

Console.WriteLine(result);
Console.ReadLine();