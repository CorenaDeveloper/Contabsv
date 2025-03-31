using Contabsv_core.Models;
using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Contabsv_core.Pages.Proveedores
{
    [Authorize]
    public class RegistrarModel : PageModel
    {
        private readonly ApiService _apiService;

        public RegistrarModel(ApiService apiService)
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

        private async Task CargarViewDataAsync()
        {
            ViewData["Sector"] = await _apiService.GetSector();
        }

        [BindProperty]
        public ProveedoresModel NewProd { get; set; }

        public async Task<IActionResult> OnPostNewAsync()
        {
            var idCliente = int.Parse(User.FindFirst("IdCliente")?.Value ?? "0");
            NewProd.idCliente = idCliente;

            var resultado = await _apiService.RegistrarProveedor(NewProd);

            if (resultado.Success)
            {
                TempData["Mensaje"] = resultado.Message;  // "Proveedor registrado con éxito."
                TempData["TipoMensaje"] = "success";
                return RedirectToPage();
            }

            // Si falla, cargamos los datos y mostramos el mensaje de error
            await CargarViewDataAsync();
            ModelState.AddModelError(string.Empty, resultado.Message);
            TempData["Mensaje"] = resultado.Message;
            TempData["TipoMensaje"] = "danger";  // o "error", según tu lógica de estilos

            return Page();
        }
    }
}
