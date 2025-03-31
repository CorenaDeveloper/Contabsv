using Contabsv_core.Models;
using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Contabsv_core.Pages.TablasAuxiliares.Resoluciones
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

        [BindProperty]
        public CrudResolucionesModel ResolucionModel { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {

            var idCliente = int.Parse(User.FindFirst("IdCliente")?.Value ?? "0");
            ResolucionModel.idCliente = idCliente;

            var success = await _apiService.RegistrarResolucion(ResolucionModel);
            if (success.Success)
            {
                TempData["Mensaje"] = success.Message;
                TempData["TipoMensaje"] = "success";
                return RedirectToPage("/TablasAuxiliares/Resoluciones/Lista");
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
            ViewData["TipoDocumentos"] = (await _apiService.GetTipoDocumento()).Where(t => t.sectorP == "Ventas").ToList();
        }
    }
}
