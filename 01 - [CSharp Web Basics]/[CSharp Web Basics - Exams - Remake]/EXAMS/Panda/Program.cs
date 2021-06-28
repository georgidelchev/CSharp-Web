using SUS.MvcFramework;
using System.Threading.Tasks;

namespace Panda
{
    public class Program
    {
        public static async Task Main()
        {
            await Host.CreateHostAsync(new Startup());
        }
    }
}
