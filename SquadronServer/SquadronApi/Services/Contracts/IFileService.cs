using SquadronApi.Core;
using SquadronApi.Dto_s;

namespace SquadronApi.Services.Contracts;

public interface IFileService
{
    Task<ServerResponse<int>> SaveFile(IFormFile file);
    
    Task<List<UploadedFileDto>> GetLastSavedFile();

    Task<List<string>> GetListOfAllFiles();
}