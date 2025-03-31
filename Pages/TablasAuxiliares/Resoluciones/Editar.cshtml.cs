using Contabsv_core.Models;
using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Contabsv_core.Pages.TablasAuxiliares.Resoluciones
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
        public ResolucionesModel ResolucionModel { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            ResolucionModel = await _apiService.GetResolucion(Id);

            if (ResolucionModel == null)
            {
                return NotFound();
            }
            await CargarViewDataAsync();
            return Page();
        }

        private async Task CargarViewDataAsync()
        {
            ViewData["TipoDocumentos"] = (await _apiService.GetTipoDocumento()).Where(t => t.sectorP == "Ventas").ToList();
        }
    }
}
