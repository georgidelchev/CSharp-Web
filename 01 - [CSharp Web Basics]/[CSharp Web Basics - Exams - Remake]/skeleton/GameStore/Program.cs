using SUS.MvcFramework;

namespace GameStore
{
    public class Program
    {
        public static void Main()
        {
            Host.CreateHostAsync(new Startup());
        }
    }
}
