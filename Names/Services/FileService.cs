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

            IEnumerable<string> fileList = _repository.GetFileList(rootFolder, argList);


            IEnumerable<Item> items = _repository.GetItemList(fileList);
                
            if (argList.ChangeFileName)
            {
                foreach (var file in items)
                {
                    await _outputService.WriteLineAsync($"file: {file.ItemId} - {file.Name}");
                }
            }
        }
    }
}
