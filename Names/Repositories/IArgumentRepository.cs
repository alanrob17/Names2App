using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Names.Repositories
{
    public interface IArgumentRepository
    {
        ArgList GetArguments(IList<string> args);
    }
}
