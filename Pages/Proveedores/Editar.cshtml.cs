using Contabsv_core.Models;
using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Threading.Tasks;

namespace Contabsv_core.Pages.Proveedores
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
        public ProveedoresModel Proveedor { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            // Inicializa la propiedad Proveedor
            Proveedor = await _apiService.GetProveedor(Id);
            if (Proveedor == null)
            {
                return NotFound();
            }

            await CargarViewDataAsync();
            return Page();
        }


        private async Task CargarViewDataAsync()
        {
            ViewData["Sector"] = await _apiService.GetSector();
        }


        /// <summary>
        /// EL METODO PUT AL TERMINAR DE GUARDAR LOS CAMBIOS EL PROVEEDOR LO TIENE QUE MANDAR A LA LISTA
        /// </summary>
        /// <returns>RETORNA UN 200/201</returns>
        public async Task<IActionResult> OnPostModAsync()
        {

            //Console.WriteLine($"Datos de Actualizar: {JsonSerializer.Serialize(Proveedor)}");
            try
            {
                var response = await _apiService.ActualizarProveedor(Proveedor); 
                if (response.Success)
                {
                    TempData["Mensaje"] = response.Message ?? "Proveedor actualizado con éxito.";
                    TempData["TipoMensaje"] = "success";

                    // Redirige a la lista de proveedores si todo salió bien
                    return RedirectToPage("/Proveedores/Lista");
                }
                else
                {
                    TempData["Mensaje"] = response.Message ?? "Error al actualizar el proveedor.";
                    TempData["TipoMensaje"] = "danger";

                    // Permanece en la misma página para mostrar el mensaje de error
                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                TempData["Mensaje"] = $"No se pudo conectar con el servicio de proveedores, {ex.Message}";
                TempData["TipoMensaje"] = "danger";

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Ocurrió un error inesperado, {ex.Message}";
                TempData["TipoMensaje"] = "danger";

                return Page();
            }
        }
    }
}
