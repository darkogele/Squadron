using Microsoft.AspNetCore.Mvc;
using SquadronApi.Services.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace SquadronApi.Controllers;

public class FilesController : BaseController
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("Add-File")]
    [SwaggerOperation(Summary = "Add file.txt")]
    public async Task<IActionResult> AddPhoto(IFormFile file)
    {
        if (file.Length <= 0)
            return BadRequest("Empty file");

        if (file.ContentType != "text/plain")
            return BadRequest("Wrong file format");

        return HandleResult(await _fileService.SaveFile(file));
    }

    [HttpGet("Grid-View")]
    [SwaggerOperation(Summary = "Grid View of all files")]
    public async Task<IActionResult> FilesGridView()
    {
        return Ok(await _fileService.GetListOfAllFiles());
    }

    [HttpGet("latest-File")]
    [SwaggerOperation(Summary = "Get data for latest uploaded file")]
    public async Task<IActionResult> LatestFile()
    {
        return Ok(await _fileService.GetLastSavedFile());
    }
}