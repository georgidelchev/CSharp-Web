using SUS.MvcFramework;
using System.Threading.Tasks;

namespace Andreys
{
    public class Program
    {
        public static async Task Main()
        {
            await Host.CreateHostAsync(new Startup());
        }
    }
}
