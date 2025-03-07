using Contabsv_core.Models;
using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Contabsv_core.Pages.Clientes
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
            return Page();
        }

        [BindProperty]
        public ClientesModel clt { get; set; }
        public async Task<IActionResult> OnPostNewAsync()
        {

            var idCliente = int.Parse(User.FindFirst("IdCliente")?.Value ?? "0");
            clt.idCliente = idCliente;

            ///Console.WriteLine(JsonSerializer.Serialize(clt, new JsonSerializerOptions { WriteIndented = true }));

            bool success = await _apiService.RegistrarClienteClt(clt);
            if (success)
            {
                TempData["Mensaje"] = "Cliente registrada con éxito.";
                TempData["TipoMensaje"] = "success";
                return RedirectToPage();
            }
            ModelState.AddModelError("", "Error al registrar el proveedor.");
            return Page();
        }
    }
}
