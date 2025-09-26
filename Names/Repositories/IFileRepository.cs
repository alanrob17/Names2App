using Names.Models;
using System.Collections.Generic;

namespace Names.Repositories
{
    public interface IFileRepository
    {
        IEnumerable<string> GetFileList(string rootFolder, ArgList argList);
        IEnumerable<Item> GetItemList(IEnumerable<string> fileList);
        string RemovePhrase(string filename, ICollection<string> phrases);
        void ModifyStatus(IEnumerable<Item> items);
        void ChangeFileNames(IEnumerable<Item> items);
        string ModifyName(string filename, string pattern);
        string AddSpaces(string filename);
        string FixTerms(string filename);
        string FixCase(string filename);
        string CleanFileName(string filename);
        string RemoveDiacritics(string filename);
        string RemoveEmojis(string filename);
        void WriteReport(IEnumerable<Item> items);
        string RemoveNames(string filename);
    }
}
