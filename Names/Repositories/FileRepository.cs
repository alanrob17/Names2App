using Microsoft.Extensions.Hosting;
using Names.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Names.Repositories
{
    public class FileRepository : IFileRepository
    {
        public IEnumerable<string> GetFileList(string rootFolder, ArgList argList)
        {
            var dir = new DirectoryInfo(rootFolder);
            var fileList = new List<string>();

            if (argList.SubFolder)
            {
                GetFiles(dir, fileList);
            }
            else
            {
                GetFiles(dir, fileList, false);
            }

            return fileList;
        }

        public IEnumerable<Item> GetItemList(IEnumerable<string> fileList)
        {
            IEnumerable<Item> items = new List<Item>();
            var count = 0;

            foreach (var file in fileList)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var item = new Item
                {
                    ItemId = count++,
                    Name = fileName,
                    ChangeName = fileName,
                    Changed = false,
                    Path = Path.GetDirectoryName(file),
                    Extension = Path.GetExtension(file)
                };

                items = items.Append(item);
            }

            return items;
        }

        private void GetFiles(DirectoryInfo d, ICollection<string> fileList, bool recursive = true)
        {
            var files = d.GetFiles("*.*");
            foreach (var fileName in files.Select(file => file.FullName))
            {
                var ext = Path.GetExtension(fileName.ToLowerInvariant());
                if (ext != ".exe" && ext != ".bak" && ext != ".log")
                {
                    fileList.Add(fileName);
                }
            }

            if (recursive)
            {
                var dirs = d.GetDirectories("*.*");
                foreach (var dir in dirs)
                {
                    GetFiles(dir, fileList, true);
                }
            }
        }
    }
}
