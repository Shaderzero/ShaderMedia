using Api.Services;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Models;

namespace Api.Controllers;

[ApiController]
public class InpxController(IZipService zipService) : ControllerBase
{
    [HttpGet]
    [Route(ApiPaths.InpxInit)]
    public async Task<ApiResponse<bool>> InitInpxAsync()
    {
        try
        {
            await zipService.ParseZipAsync();
            // var x = await parser.GetInternalFileNamesAsync();

            return new ApiResponse<bool>
            {
                Value = true
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                Errors = [ex.Message]
            };
        }
    }
}