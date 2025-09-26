using Names.Models;
using Names.Services.Output;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Names.Services
{
    public class FileService : IFileService
    {
        private readonly Repositories.IFileRepository _repository;
        private readonly IOutputService _outputService;

        public FileService(Repositories.IFileRepository fileRepository, IOutputService outputService)
        {
            _repository = fileRepository;
            _outputService = outputService;
        }

        public async Task RunFileOperations(ArgList argList)
        {
            int count = 0;
            string rootFolder = Directory.GetCurrentDirectory();
            ICollection<string> phrases = new List<string>();
            phrases = CreatePhraseList();

            IEnumerable<string> fileList = _repository.GetFileList(rootFolder, argList);

            IEnumerable<Item> items = _repository.GetItemList(fileList);

            // This is where I change the files.
            foreach (var item in items)
            {
                item.ChangeName = _repository.RemoveNames(item.ChangeName);
                item.ChangeName = _repository.RemoveEmojis(item.ChangeName);
                item.ChangeName = _repository.RemoveDiacritics(item.ChangeName);
                item.ChangeName = _repository.RemovePhrase(item.ChangeName, phrases);
                item.ChangeName = _repository.ModifyName(item.ChangeName, "\\.");
                item.ChangeName = _repository.ModifyName(item.ChangeName, "_");
                item.ChangeName = _repository.ModifyName(item.ChangeName, "\\s+");
                item.ChangeName = _repository.AddSpaces(item.ChangeName);

                await _outputService.WriteLineAsync($"{argList.ProperCase}");

                if (item.ChangeName == item.ChangeName.ToUpperInvariant() || argList.ProperCase == true)
                {
                    item.ChangeName = item.ChangeName.ToLowerInvariant(); // .ToTitleCase() won't change uppercase filenames
                    item.ChangeName = _repository.FixCase(item.ChangeName);
                }

                item.ChangeName = _repository.FixTerms(item.ChangeName);
                //item.ChangeName = _repository.FixCase(item.ChangeName);
                //item.ChangeName = _repository.FixTerms(item.ChangeName);
                item.ChangeName = _repository.CleanFileName(item.ChangeName);

            }
                
            _repository.ModifyStatus(items);
            
            if (argList.ChangeFileName)
            { 
                _repository.ChangeFileNames(items);
            }

            _repository.WriteReport(items);
        }

        private static List<string> CreatePhraseList()
        {
            return new List<string>
            {
                "www.sanet.st",
                "softarchive.net",
                "softarchive.la",
                "sanet.st",
                "sanet..st",
                "sanet.cd",
                "sanet..cd",
                "sanet.me",
                "sanet..me",
                "snorgared",
                "avaxhome",
                "avxhom",
                "ebook",
                "e-book",
                "a novel"
            };
        }
    }
}
