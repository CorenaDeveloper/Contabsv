using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Contabsv_core.Pages.Clientes
{
    [Authorize]
    public class ListaModel : PageModel
    {
        private readonly IConfiguration _configuration;

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

        /// <summary>
        ///  METODO CREADO PARA COMSUMIR DIRECTAMENTE DESDE DATATABLER.NET
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetClientesIntAsync()
        {
            var idCliente = User.FindFirst("IdCliente")?.Value ?? "0";
            var proveedores = await _apiService.GetClientesInternos(idCliente);
            return new JsonResult(proveedores, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        /// <summary>
        /// ELIMINA EL CLIENTE
        /// </summary>
        /// <param name="idClienteClt"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteAsync(int idClienteClt)
        {
            try
            {
                var resultado = await _apiService.DeleteClienteClt(idClienteClt);
                if (resultado)
                {
                    TempData["Mensaje"] = "Cliente eliminado con éxito.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToPage();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al eliminar el Cliente.");
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
