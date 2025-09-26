using Names.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Names.Repositories
{
    public class ArgumentRepository : IArgumentRepository
    {
        public ArgList GetArguments(IList<string> args)
        {
            // TODO: add multiple arguments S = subdir, W = write only
            var subFolders = false;
            var changeFileNames = false;
            var properCase = false;

            if (args.Count == 1)
            {
                var arg = args[0].ToLowerInvariant();
                if (arg.Contains("s"))
                {
                    subFolders = true;
                }

                if (arg.Contains("w"))
                {
                    changeFileNames = true;
                }

                if (arg.Contains("p"))
                {
                    properCase = true;
                }
            }

            var argList = new ArgList(subFolders, changeFileNames, properCase);

            return argList;
        }
    }
}
