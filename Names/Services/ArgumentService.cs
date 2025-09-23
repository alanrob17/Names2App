using Names.Repositories;
using Names.Services.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Names.Services
{
    public class ArgumentService : IArgumentService
    {
        private readonly IArgumentRepository _repository;
        private readonly IOutputService _output;
        private readonly IFileService _fileService;
        private readonly IList<string> _args;

        public ArgumentService(IArgumentRepository repository, IOutputService output, IFileService fileService, IList<string> args)
        {
            _repository = repository;
            _output = output;
            _fileService = fileService;
            _args = args;
        }

        public async Task RunArgumentOperations()
        {
            var argList = _repository.GetArguments(_args);
            // Use argList as needed
            await _fileService.RunFileOperations(argList);
        }
    }
}
