using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Contabsv_core.Pages.TablasAuxiliares.Resoluciones
{
    [Authorize]
    public class ListaModel : PageModel
    {
        private readonly ApiService _apiService;
        public ListaModel(ApiService apiService)
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

        public async Task<IActionResult> OnGetResolucionesAsync()
        {
            var idCliente = User.FindFirst("IdCliente")?.Value ?? "0";
            var compras = await _apiService.GetResolciones(idCliente);
            return new JsonResult(compras, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int idResolucion)
        {
            try
            {
                var resultado = await _apiService.DeleteResolucion(idResolucion);
                if (resultado.Success)
                {
                    TempData["Mensaje"] = resultado.Message;
                    TempData["TipoMensaje"] = "success";
                    return RedirectToPage();
                }
                else
                {
                    TempData["Mensaje"] = resultado.Message;
                    TempData["TipoMensaje"] = "danger";
                    return RedirectToPage();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ocurrió un error: {ex.Message}");
                return Page();
            }
        }
    }
}
