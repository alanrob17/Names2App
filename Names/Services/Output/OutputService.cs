using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Names.Services.Output
{
    public class OutputService : IOutputService
    {
        public async Task WriteErrorAsync(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteLineAsync(message);
            Console.ForegroundColor = originalColor;
        }

        public async Task WriteLineAsync(string message)
        {
            await Console.Out.WriteLineAsync(message);
        }
    }
}
