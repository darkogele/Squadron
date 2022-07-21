using Microsoft.EntityFrameworkCore;
using SquadronApi.Data;
using SquadronApi.Dto_s;
using SquadronApi.Entities;
using SquadronApi.Services.Contracts;

namespace SquadronApi.Services;

public class FileService : IFileService
{
    private readonly DataContext _context;

    public FileService(DataContext context)
    {
        _context = context;
    }

    public async Task<int> SaveFile(IFormFile file)
    {
        var fileName = Path.GetFileName(file.FileName);
        var listOfFiles = new List<UploadedFileLine>();

        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();
                if (line is null) break;

                var values = line.Split(separator: ",").Select(x => x.Trim()).ToArray();

                listOfFiles.Add(new UploadedFileLine()
                {
                    Color = values[0],
                    Number = int.Parse(values[1]),
                    Label = values[2]
                });
            }
        }

        // You can use this as json in sql or in monogo/comsmos db to make things more easy
        var forSql = System.Text.Json.JsonSerializer.Serialize(listOfFiles);

        var fileForDb = new UploadedFile { Lines = listOfFiles, Name = fileName };

        await _context.FileLines.AddRangeAsync(listOfFiles);
        await _context.File.AddAsync(fileForDb);
        await _context.SaveChangesAsync();

        return fileForDb.Id;
    }

    public async Task<List<string>> GetListOfAllFiles()
    {
        var data = await _context.File.OrderByDescending(x => x.Id)
            .Select(x => x.Name).ToListAsync();
        return data;
    }

    public async Task<List<UploadedFileDto>> GetLastSavedFile()
    {
        var finalId = await _context.File.MaxAsync(x => x.Id);

        var data = await _context.FileLines.Where(x => x.UploadedFileId == finalId)
            .Select(x => new UploadedFileDto
            {
                Color = x.Color,
                Label = x.Label,
                Number = x.Number
            })
            .ToListAsync();

        var data2 = await _context.File.Include(x => x.Lines)
            .OrderByDescending(x => x.Id)
            .FirstAsync();

        var dto = data2.Lines.ConvertAll(line => new UploadedFileDto
        {
            Label = line.Label,
            Number = line.Number,
            Color = line.Color
        });

        return dto;
    }
}