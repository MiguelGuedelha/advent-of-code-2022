using OneOf;
using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd();

var result = new List<int>();

var pairs = inputString
    .Split($"{Environment.NewLine}{Environment.NewLine}")
    .Select(x => x.Split(Environment.NewLine))
    .ToList();


for (var i = 0; i < pairs.Count; i++)
{
    var leftChars = pairs[i][0].ToCharArray();
    var leftBorders = ArrayBorders(leftChars);
    var leftArray = new List<dynamic>();

    var rightChars = pairs[i][1].ToCharArray();
    var rightBorders = ArrayBorders(rightChars);
    var rightArray = new List<dynamic>();


    for (int j = 0; j < leftChars.Length; j++)
    {

    }
}

List<(int, int)> ArrayBorders(char[] chars)
{
    var starts = new Stack<int>();
    var result = new List<(int, int)>();
    for (var i = 0; i < chars.Length; i++)
    {
        if (chars[i] == '[')
        {
            starts.Push(i);
        }

        if (chars[i] == ']')
        {
            result.Add((starts.Pop(), i));
        }
    }

    return result.OrderBy(x => x.Item1).ToList();
}

class Element
{
    public OneOf<int, Element[]> Value { get; set; }
}