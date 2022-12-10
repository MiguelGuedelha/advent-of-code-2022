using System.Text;
using System.Text.RegularExpressions;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var input = reader.ReadToEnd().Split(Environment.NewLine);

var tree = new Tree<string>("/", null, true, null);
var totalSize = 70000000;
var requiredUnused = 30000000;


ProcessInput(input, tree);

var currentUsed = tree.Size;
var currentUnused = totalSize - currentUsed;

var result = tree.Flatten().Where(x => x.IsDirectory).OrderBy(x => x.Size).FirstOrDefault(x => currentUnused + x.Size >= requiredUnused)?.Size;

Console.WriteLine(result);
Console.ReadLine();

void ProcessInput(string[] inputArray, Tree<string> root)
{
    var cdPattern = @"^\$ cd (.+)$";
    var dirPattern = @"^dir (.+)$";
    var filePattern = @"^(\d+) (.+)$";
    var cdRegex = new Regex(cdPattern, RegexOptions.IgnoreCase);
    var dirRegex = new Regex(dirPattern, RegexOptions.IgnoreCase);
    var fileRegex = new Regex(filePattern, RegexOptions.IgnoreCase);
    var current = root;
    for (var i = 1; i < inputArray.Length; i++)
    {
        var tempInput = inputArray[i];
        var cdMatch = cdRegex.Match(tempInput);
        if (cdMatch.Success)
        {
            var path = cdMatch.Groups[1].Value;
            current = path.Equals("..") ? current!.Parent : current!.Children[path];
            continue;
        }
        var dirMatch = dirRegex.Match(tempInput);
        if (dirMatch.Success)
        {
            var value = dirMatch.Groups[1].Value;
            current!.Insert(value, true, null);
            continue;
        }

        var fileMatch = fileRegex.Match(tempInput);
        if (fileMatch.Success)
        {
            var size = fileMatch.Groups[1].Value;
            var name = fileMatch.Groups[2].Value;
            current!.Insert(name, false, int.Parse(size));
        }
    }
}

public class Tree<T> where T : notnull
{
    public T Value { get; init; }

    public bool IsDirectory { get; init; }

    private int? _size;

    public int? Size
    {
        get => GetSize();
        init => _size = value;
    }

    public Tree<T>? Parent { get; set; }

    public Dictionary<T, Tree<T>> Children { get; init; } = new();

    public Tree(T value, Tree<T>? parent, bool isDirectory, int? size)
    {
        Value = value;
        Parent = parent;
        IsDirectory = isDirectory;
        _size = size;
    }

    public Tree<T> Insert(T value, bool isDirectory, int? size)
    {
        var result = new Tree<T>(value, this, isDirectory, size);
        Children.TryAdd(value, result);
        return result;
    }

    private int GetSize()
    {
        if (!IsDirectory)
        {
            return _size!.Value;
        }

        return Children.Values.Select(x => x.GetSize()).Sum();

    }

    public IEnumerable<Tree<T>> Flatten()
    {
        return Children.Values.SelectMany(c => c.Flatten()).Concat(new[] { this });
    }
}