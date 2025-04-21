using Microsoft.AspNetCore.Mvc;

namespace Api.Rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlantsController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public PlantsController(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string q)
    {
        var token = "aeVGvDJPk4E9JaW-RtIQuDsGV0xDbkXx9qDCJS2kVzk";
        var url = $"https://trefle.io/api/v1/plants/search?q={q}&token={token}";

        var response = await _httpClient.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        return Content(json, "application/json");
    }
}
