using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;

namespace Contabsv_core.Pages.Proveedores
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
        public async Task<IActionResult> OnGetProveedoresAsync()
        {
            var idCliente = User.FindFirst("IdCliente")?.Value ?? "0";
            var proveedores = await _apiService.GetProveedores(idCliente);
            return new JsonResult(proveedores, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        /// <summary>
        /// PROPIEDAD ENLAZADA AL METODO
        /// </summary>
        public async Task<IActionResult> OnPostDeleteAsync(int idProveedor)
        {
            try
            {
                var resultado = await _apiService.DeleteProveedor(idProveedor);
                if (resultado)
                {
                    TempData["Mensaje"] = "Proveedor eliminado con éxito.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToPage();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al eliminar el proveedor.");
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