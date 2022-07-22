using System.Drawing;
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
        var colors = Enum.GetNames(typeof(KnownColor)).Select(x => x.ToLower());

        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();
                if (line is null) break;

                var values = line.Split(separator: ",").Select(x => x.Trim()).ToArray();

                var validateColor = colors.Contains(values[0].ToLower().Trim());
                var validateNumber = int.TryParse(values[1], out var number);
                var validateLabel = values[2].Length >= 3;

                if (validateColor && validateNumber && validateLabel)
                {
                    listOfFiles.Add(new UploadedFileLine()
                    {
                        Color = values[0],
                        Number = number,
                        Label = values[2]
                    });
                }
            }
        }

        // You can use this as json in sql or in monogo/comsmos to make things more easy
        var jsonData = System.Text.Json.JsonSerializer.Serialize(listOfFiles);

        if (listOfFiles.Count > 0)
        {
            var fileForDb = new UploadedFile { Lines = listOfFiles, Name = fileName };

            await _context.FileLines.AddRangeAsync(listOfFiles);
            await _context.File.AddAsync(fileForDb);
            await _context.SaveChangesAsync();

            return fileForDb.Id;
        }

        return 0;
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