using SUS.MvcFramework;
using System.Threading.Tasks;

namespace MishMash
{
    public class Program
    {
        public static async Task Main()
        {
            await Host.CreateHostAsync(new StartUp());
        }
    }
}
