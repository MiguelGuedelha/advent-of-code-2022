namespace Shared.Extensions
{
    public static class ArithmeticExtensions
    {
        public static int Mod(this int first, int second)
        {
            var r = first % second;
            return r < 0 ? r + second : r;
        }
    }
}
