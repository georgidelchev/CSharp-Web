using System.Threading.Tasks;
using SUS.MvcFramework;

namespace MishMashWebApp
{
    public class Program
    {
        public static async Task Main()
        {
            Host.CreateHostAsync(new StartUp());
        }
    }
}
