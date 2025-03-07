using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Contabsv_core.Pages.Compras
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

        /// <summary>
        /// METO CREADO ESPECIALMENTE PARA LLENAR UNA TABLA DE DATATABLES.NET 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoFecha"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetComprasAsync(string? fechaInicio, string? fechaFin, int tipoFecha = 1)
        {
            var idCliente = User.FindFirst("IdCliente")?.Value ?? "0";
            var compras = await _apiService.GetCompras(idCliente, fechaInicio, fechaFin, tipoFecha);
            return new JsonResult(compras, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        /// <summary>
        /// ELIMINA LA COMPRA
        /// </summary>
        public async Task<IActionResult> OnPostDeleteAsync(int idCompra)
        {
            try
            {
                var resultado = await _apiService.DeleteCompras(idCompra);
                if (resultado)
                {
                    TempData["Mensaje"] = "Compra eliminado con éxito.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToPage();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al eliminar Compra.");
                    return Page();
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
