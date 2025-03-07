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

            ///Console.WriteLine(JsonSerializer.Serialize(NewProd, new JsonSerializerOptions { WriteIndented = true }));

            bool success = await _apiService.RegistrarProveedor(NewProd);
            if (success)
            {
                TempData["Mensaje"] = "Proveedor registrada con éxito.";
                TempData["TipoMensaje"] = "success";
                return RedirectToPage();
            }
            await CargarViewDataAsync();
            ModelState.AddModelError("", "Error al registrar el proveedor.");
            return Page();
        }
    }
}
