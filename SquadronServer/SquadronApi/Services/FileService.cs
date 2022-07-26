﻿using Microsoft.EntityFrameworkCore;
using SquadronApi.Core;
using SquadronApi.Data;
using SquadronApi.Dto_s;
using SquadronApi.Entities;
using SquadronApi.Services.Contracts;
using System.Drawing;

namespace SquadronApi.Services;

public class FileService : IFileService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FileService(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServerResponse<int>> SaveFile(IFormFile file)
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

                if (values.Length == 3)
                {
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
        }

        // You can use this as json in sql or in monogo/comsmos to make things more easy
        var jsonData = System.Text.Json.JsonSerializer.Serialize(listOfFiles);
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();

        if (listOfFiles.Count > 0 && userId is not null)
        {
            var fileForDb = new UploadedFile { Lines = listOfFiles, Name = fileName, UserId = (int)userId };

            await _context.FileLines.AddRangeAsync(listOfFiles);
            await _context.File.AddAsync(fileForDb);
            await _context.SaveChangesAsync();

            return ServerResponse<int>.Success(fileForDb.Id);
        }

        return ServerResponse<int>.Failure("Bad file, invalid Values in file");
    }

    public async Task<List<string>> GetListOfAllFiles()
    {
        return await _context.File.OrderByDescending(x => x.Id)
            .Select(x => x.Name).ToListAsync();
    }

    public async Task<List<UploadedFileDto>> GetLastSavedFile()
    {
        if (await _context.File.AnyAsync())
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

            return data2.Lines.ConvertAll(line => new UploadedFileDto
            {
                Label = line.Label,
                Number = line.Number,
                Color = line.Color
            });
        }

        return new List<UploadedFileDto>();
    }
}