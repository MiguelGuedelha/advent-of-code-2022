using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd();

var monkeyInput = inputString
    .Split($"{Environment.NewLine}{Environment.NewLine}")
    .Select(x => x.Split(Environment.NewLine));

var operations = new Dictionary<string, Func<long, long, long>>
{
    { "*", (a, b) => a*b },
    { "+", (a, b) => a+b },
};

var monkeys = new List<Monkey>();

foreach (var monkeyArray in monkeyInput)
{
    var startingItems = monkeyArray[1]
        .Replace(",", "")
        .Split(" ")
        .Skip(4)
        .Select(x => long.Parse(x))
        .ToList();

    var operation = monkeyArray[2]
        .Split(" = ")[1]
        .Split(" ");

    var test = long.Parse(monkeyArray[3].Split(" ")[^1]);

    var trueMonkey = int.Parse(monkeyArray[4].Split(" ")[^1]);
    var falseMonkey = int.Parse(monkeyArray[5].Split(" ")[^1]);

    monkeys.Add(new Monkey(startingItems, operations[operation[1]], operation[0], operation[2], test, trueMonkey, falseMonkey));
}

var lcm = Lcm(monkeys.Select(x => x.TestValue).ToArray());

for (int i = 0; i < 10000; i++)
{
    foreach (var monkey in monkeys)
    {
        while (monkey.Items.Any())
        {
            var (value, monkeyIndex) = monkey.RunOperation(lcm);
            monkeys[monkeyIndex].Items.Add(value);
        }
    }
}

var result = monkeys.Select(x => x.Inspections).OrderByDescending(x => x).Take(2).Aggregate((long)1, (total, current) => total * current);

Console.WriteLine(result);
Console.ReadLine();

static long Gcd(long n1, long n2)
{
    if (n2 == 0)
    {
        return n1;
    }
    else
    {
        return Gcd(n2, n1 % n2);
    }
}

static long Lcm(long[] numbers)
{
    return numbers.Aggregate((S, val) => S * val / Gcd(S, val));
}

record Monkey(
    List<long> Items,
    Func<long, long, long> Operation,
    string ArgumentOne,
    string ArgumentTwo,
    long TestValue,
    int TrueMonkey,
    int FalseMonkey)
{
    private long _inspections = 0;

    public long Inspections
    {
        get { return _inspections; }
    }

    public (long, int) RunOperation(long lcm)
    {
        _inspections++;

        var old = Items[0];
        Items.RemoveAt(0);

        var argOne = ArgumentOne == "old" ? old : int.Parse(ArgumentOne);
        var argTwo = ArgumentTwo == "old" ? old : int.Parse(ArgumentTwo);
        var value = Operation(argOne, argTwo) % lcm;

        var canBeDivided = value % TestValue == 0;

        var monkey = canBeDivided ? TrueMonkey : FalseMonkey;


        return (value, monkey);
    }
}