﻿using SUS.MvcFramework;

namespace Andreys.App
{

    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main()
        {
            await Host.CreateHostAsync(new Startup());
        }
    }
}
