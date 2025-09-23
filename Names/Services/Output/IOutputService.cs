using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Names.Services.Output
{
    public interface IOutputService
    {
        Task WriteLineAsync(string message);
        Task WriteErrorAsync(string message);
    }
}
