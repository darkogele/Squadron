using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquadronApi.Services.Contracts;

namespace SquadronApi.Controllers;

[ApiController, Authorize]
public class FilesController : BaseController
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("Add-File")]
    public async Task<IActionResult> AddPhoto(IFormFile file)
    {
        if (file.Length <= 0)
            return BadRequest("Empty file");

        if (file.ContentType != "text/plain")
            return BadRequest("Wrong file format");

        return Ok(await _fileService.SaveFile(file));
    }

    [HttpGet("Grid-View")]
    public async Task<IActionResult> FilesGridView()
    {
        return Ok(await _fileService.GetListOfAllFiles());
    }

    [HttpGet("latest-File")]
    public async Task<IActionResult> LatestFile()
    {
        return Ok(await _fileService.GetLastSavedFile());
    }
}