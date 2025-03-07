using Contabsv_core.Models;
using Contabsv_core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Contabsv_core.Pages.Clientes
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
        public ClientesModel clt { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }
            clt = await _apiService.GetClienteInterno(Id);
            if (clt == null)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>
        /// EL METODO PUT AL TERMINAR DE GUARDAR LOS CAMBIOS EL PROVEEDOR LO TIENE QUE MANDAR A LA LISTA
        /// </summary>
        /// <returns>RETORNA UN 200/201</returns>
        public async Task<IActionResult> OnPostModAsync()
        {

            Console.WriteLine($"Datos de Actualizar: {JsonSerializer.Serialize(clt)}");
            try
            {
                bool success = await _apiService.ActualizarClienteClt(clt);
                if (success)
                {
                    TempData["Mensaje"] = "Cliente actualizado con éxito.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToPage("/Clientes/Lista");
                }
                else
                {
                    TempData["Mensaje"] = "Error al actualizar el Cliente.";
                    TempData["TipoMensaje"] = "danger";
                    return Page(); // Permanece en la misma página para mostrar el error
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
