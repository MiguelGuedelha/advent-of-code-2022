using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var input = reader.ReadToEnd();

var pairs = input.Split(Environment.NewLine);

var result = pairs
    .Select(x => GenerateRanges(x))
    .Count(x => x.Item1.Range.Intersect(x.Item2.Range).Any());

Console.WriteLine(result);
Console.ReadLine();

ValueTuple<CleaningRange, CleaningRange> GenerateRanges(string pair)
{
    var split = pair.Split(",").Select(x => x.Split("-")).ToList();
    var elfOne = new CleaningRange(int.Parse(split[0][0]), int.Parse(split[0][1]));
    var elfTwo = new CleaningRange(int.Parse(split[1][0]), int.Parse(split[1][1]));
    return (elfOne, elfTwo);
}

record CleaningRange(int Start, int End)
{
    private int Size => End - Start + 1;
    public List<int> Range => Enumerable.Range(Start, Size).ToList();
}