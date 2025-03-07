using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Contabsv_core.Pages.Ventas.Final
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly ApiService _apiService;
        public ListarModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnGetConsumidorAsync(string? fechaInicio, string? fechaFin, int tipoFecha)
        {
            var idCliente = User.FindFirst("IdCliente")?.Value ?? "0";
            var compras = await _apiService.GetConsumidoresFinal(idCliente, fechaInicio, fechaFin, tipoFecha);
            return new JsonResult(compras, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
