using Names.Models;
using System.Collections.Generic;

namespace Names.Repositories
{
    public interface IFileRepository
    {
        IEnumerable<string> GetFileList(string rootFolder, ArgList argList);
    }
}
