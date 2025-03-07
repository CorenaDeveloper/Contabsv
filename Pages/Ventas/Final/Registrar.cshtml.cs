using Contabsv_core.Models;
using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Contabsv_core.Pages.Ventas.Final
{
    [Authorize]
    public class RegistrarModel : PageModel
    {
        private readonly ApiService _apiService;

        public RegistrarModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        [BindProperty]
        public VentasConsumidorModel ventasConsumidor { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }
            await CargarViewDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var idCliente = int.Parse(User.FindFirst("IdCliente")?.Value ?? "0");
            ventasConsumidor.idCliente = idCliente;

            bool success = await _apiService.RegistrarConsumidor(ventasConsumidor);
            if (success)
            {
                TempData["Mensaje"] = "Venta registrada con éxito.";
                TempData["TipoMensaje"] = "success";
                return RedirectToPage();
            }
            await CargarViewDataAsync();
            ModelState.AddModelError("", "Error al registrar la Venta.");
            return Page();
        }

        private async Task CargarViewDataAsync()
        {
            var idCliente = User.FindFirst("IdCliente")?.Value ?? "0";

            ViewData["ClaseDocumentos"] = await _apiService.GetClaseDocumentos();
            ViewData["TipoDocumentos"] = (await _apiService.GetTipoDocumento()).Where(t => t.sectorP == "Ventas").ToList();
            ViewData["Operaciones"] = (await _apiService.GetOperaciones()).Where(t => t.sectorP == "Ventas").ToList();
            ViewData["Clientes"] = await _apiService.GetClientesInternos(idCliente);
            ViewData["TipoOperacionIG"] = (await _apiService.GetTipoOperacionIG()).Where(t => t.sectorP == "Ventas").ToList();
        }
    }
}
