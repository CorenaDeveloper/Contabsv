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


        public async Task<IActionResult> OnPostDeleteAsync(int idCompra)
        {
            try
            {
                var resultado = await _apiService.DeleteCompras(idCompra);
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
                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                TempData["TipoMensaje"] = "danger";
                return Page();
            }
        }
    }
}
