using Contabsv_core.Models;
using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Contabsv_core.Pages.Compras
{
    [Authorize]
    public class EditarModel : PageModel
    {
        private readonly ApiService _apiService;
        public EditarModel(ApiService apiService)
        {
            _apiService = apiService;
        }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public CompraModel Compra { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            Compra = await _apiService.GetCompra(Id);
            if (Compra == null)
            {
                return NotFound();
            }

            await CargarViewDataAsync();
            return Page();
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

        public async Task<IActionResult> OnPostModAsync()
        {

            Console.WriteLine($"Datos de Actualizar: {JsonSerializer.Serialize(Compra)}");
            try
            {
                bool success = await _apiService.ActualizarCompras(Compra);
                if (success)
                {
                    TempData["Mensaje"] = "Compra actualizado con éxito.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToPage("/Compras/Listar");
                }
                else
                {
                    TempData["Mensaje"] = "Error al actualizar Compra.";
                    TempData["TipoMensaje"] = "danger";
                    await CargarViewDataAsync();
                    return Page(); // Permanece en la misma página para mostrar el error
                }
            }
            catch (HttpRequestException ex)
            {
                TempData["Mensaje"] = $"No se pudo conectar con el servicio de Compras, {ex.Message}";
                TempData["TipoMensaje"] = "danger";
                await CargarViewDataAsync();
                return Page();
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Ocurrió un error inesperado, {ex.Message}";
                TempData["TipoMensaje"] = "danger";
                await CargarViewDataAsync();
                return Page();
            }
        }

    }
}
