

using Contabsv_core.Services;

var builder = WebApplication.CreateBuilder(args);


// Configurar servicios de antifalsificación
builder.Services.AddSingleton<ApiService>();
builder.Services.AddHttpContextAccessor();

// Agregar servicios de autenticación con Cookies
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Login"; // Redirige a Login si no está autenticado
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2); // Tiempo de sesión
        options.SlidingExpiration = true; // Renueva el tiempo de sesión si el usuario sigue activo
    });


builder.Services.AddAuthorization();
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();