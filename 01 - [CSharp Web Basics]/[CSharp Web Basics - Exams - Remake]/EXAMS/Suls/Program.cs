using System.Threading.Tasks;
using SUS.MvcFramework;

namespace SulsApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await Host.CreateHostAsync(new Startup());
        }
    }
}
