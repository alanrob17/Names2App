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
            string rootFolder = Directory.GetCurrentDirectory();

            IEnumerable<string> fileList = _repository.GetFileList(rootFolder, argList);

            foreach (var file in fileList)
            {
                await _outputService.WriteLineAsync($"Found file: {file}");
            }

            if (argList.ChangeFileName)
            {
                foreach (var file in fileList)
                {
                    await _outputService.WriteLineAsync($"Would rename file: {file}");
                }
            }
        }
    }
}
