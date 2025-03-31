using Contabsv_core.Models;
using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Contabsv_core.Pages.Compras
{
    [Authorize]
    public class RegistrarCprInternaModel : PageModel
    {
        private readonly ApiService _apiService;

        public RegistrarCprInternaModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }
            await CargarViewDataAsync();
            return Page();
        }

        [BindProperty]
        public CompraModel NuevaCompra { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            var idCliente = int.Parse(User.FindFirst("IdCliente")?.Value ?? "0");
            NuevaCompra.idCliente = idCliente;

            var success = await _apiService.RegistrarCompra(NuevaCompra);
            if (success.Success)
            {
                TempData["Mensaje"] = success.Message;
                TempData["TipoMensaje"] = "success";
                return RedirectToPage();
            }
            else
            {
                TempData["Mensaje"] = success.Message;
                TempData["TipoMensaje"] = "danger";
                await CargarViewDataAsync();
                return RedirectToPage();
            }
        }

        private async Task CargarViewDataAsync()
        {
            var idCliente = User.FindFirst("IdCliente")?.Value ?? "0";

            ViewData["ClaseDocumentos"] = await _apiService.GetClaseDocumentos();
            ViewData["TipoDocumentos"] = (await _apiService.GetTipoDocumento()).Where(t => t.sectorP == "Compras").ToList();
            ViewData["Operaciones"] = (await _apiService.GetOperaciones()).Where(t => t.sectorP == "Compras").ToList();
            ViewData["Proveedores"] = await _apiService.GetProveedores(idCliente);
            ViewData["Clasificaciones"] = (await _apiService.GetClasificacion()).Where(t => t.sectorP == "Compras").ToList();
            ViewData["TipoOperacionIG"] = (await _apiService.GetTipoOperacionIG()).Where(t => t.sectorP == "Compras").ToList();
        }
    }
}
