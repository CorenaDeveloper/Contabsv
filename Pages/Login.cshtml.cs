using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace Contabsv_core.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        [BindProperty]
        public string Usuario { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            return Page(); 
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Ingrese usuario y contraseña.";
                return Page();
            }

            try
            {
                string apiUrl = _configuration["ApiSettings:apisUrl"] + "DBSeguridad_Login/Login";
                string authToken = _configuration["ApiSettings:AuthToken"];

                var requestData = new { usuario = Usuario, password = Password };
                var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("X-AUTH-TOKEN", authToken);
                httpClient.Timeout = TimeSpan.FromSeconds(10);

                // Enviar la petición como POST
                var response = await httpClient.PostAsync(apiUrl, jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = "Usuario o contraseña incorrectos.";
                    return Page();
                }

                // Leer la respuesta
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"?? Respuesta de la API: {responseBody}");
                var userData = JsonSerializer.Deserialize<LoginResponse>(responseBody);

                var claims = new List<Claim>
                 {
                    new Claim(ClaimTypes.NameIdentifier, userData.idUsuario.ToString()),
                    new Claim(ClaimTypes.Name, userData.nombre),
                    new Claim(ClaimTypes.Email, userData.email),
                    new Claim("idCliente", userData.idCliente.ToString())
                 };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal);

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error de conexión con el servidor: {ex.Message}";
                return Page();
            }
        }
    }

    // Clase para recibir la respuesta de la API
    public class LoginResponse
    {
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public int idCliente { get; set; }
    }

}