using SquadronApi.Dto_s;

namespace SquadronApi.Services.Contracts;

public interface IFileService
{
    Task<int> SaveFile(IFormFile file);
    Task<List<UploadedFileDto>> GetLastSavedFile();
    Task<List<string>> GetListOfAllFiles();
}