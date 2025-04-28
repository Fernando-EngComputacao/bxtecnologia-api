using System.Net;
using BXTecnologia.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BXTecnologia.API.Controllers;

[ApiController]
public class CustomerImageController : ControllerBase
{
    private readonly ICustomerImageService _customerImageService;
    
    public CustomerImageController(ICustomerImageService customerImageService)
    {
        _customerImageService = customerImageService;
    }
    
    [HttpPost("customers/{id:guid}/image")]
    public async Task<IActionResult> Upload([FromRoute] Guid id, [FromForm(Name = "Data")] IFormFile file)
    {
        var response = await _customerImageService.UploadImageAsync(id, file);

        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            return Ok();
        }
        
        return BadRequest();
    }

    [HttpPatch("customers/{id:guid}/{fileName}/image")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id, 
        [FromRoute] string fileName,
        [FromQuery(Name = "Width")] int width,
        [FromQuery(Name = "Height")] int height)
    {
        var response = await _customerImageService.UpdateImageAsync(id, fileName, width, height);

        return Ok(response);
    }

    [HttpGet("customers/{id:guid}/{fileName}/image")]
    public async Task<IActionResult> Get([FromRoute] Guid id, [FromRoute] string fileName)
    {
        var response = await _customerImageService.GetImageAsync(id, fileName);

        if (response is null)
        {
            return NotFound(); 
        }
        
        return File(response.ResponseStream, response.Headers.ContentType);
    }

    [HttpGet("customers/{id:guid}/image")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _customerImageService.GetAllImagesByCustomerAsync(id);
        return Ok(response);
    }
    
    [HttpDelete("customers/{id:guid}/image")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var response = await _customerImageService.DeleteImageAsync(id);
        return response.HttpStatusCode switch
        {
            HttpStatusCode.NoContent => Ok(),
            HttpStatusCode.NotFound => NotFound(),
            _ => BadRequest()
        };
    }
}