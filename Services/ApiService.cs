

using Contabsv_core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Contabsv_core.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// CONFIGURACION DE MI APIS TANTO LA URL COMO EL X-AUTH-TOKEN QUE ALMACENAMOS EN EL ARCHIVO APPSETTING.JSON
        /// </summary>
        /// <param name="configuration"></param>
        public ApiService(IConfiguration configuration)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration["ApiSettings:apisUrl"])
            };
            _httpClient.DefaultRequestHeaders.Add("X-AUTH-TOKEN", configuration["ApiSettings:AuthToken"]);
        }

        /// <summary>
        /// LISTA TODO LAS CLASES DE DOCUMENTOS QUE TENEMOS EN LA DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<ClaseDocumentoModel>> GetClaseDocumentos()
        {
            var response = await _httpClient.GetAsync("DBContabilidad_Clase_Documento/Clase_Documentos");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ClaseDocumentoModel>>(json);
        }

        /// <summary>
        /// LISTA SECTORES DE LA DB
        /// </summary>
        /// <returns></returns>
        public async Task<List<SectorModel>> GetSector()
        {
            var response = await _httpClient.GetAsync("DBContabilidad_Sector/Sector");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<SectorModel>>(json);
        }

        /// <summary>
        /// LISTA TODO LOS TIPOS DE DOCUMENTOS QUE TENEMOS SE DEBE FILTRAR EL SECTOR ENC ASO SEA NECESARIO
        /// </summary>
        /// <returns></returns>
        public async Task<List<TipoDocumentoModel>> GetTipoDocumento()
        {
            var response = await _httpClient.GetAsync("DBContabilidad_Tpo_Documento/TipoDocumento");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TipoDocumentoModel>>(json);
        }

        /// <summary>
        /// LISTA TODO LOS TIPOS DE OPERACIONES QUIE TENEMOS EN NUESTRA DB , ESTO INCLUYE TANTO INGRESOS COMO GASTOS, COSTOS
        /// </summary>
        /// <returns></returns>
        public async Task<List<OperacionesModel>> GetOperaciones()
        {
            var response = await _httpClient.GetAsync("DBContabilidad_Operaciones/Operaciones");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<OperacionesModel>>(json);
        }

        /// <summary>
        /// MUESTRA TODAS LAS CLASIFICACIONES LOS DOCUMENTOS POR TIPO DE MOCIMIENTO
        /// </summary>
        /// <returns></returns>
        public async Task<List<ClasificacionesModel>> GetClasificacion()
        {
            var response = await _httpClient.GetAsync("DBContabilidad_Clasificacion/Clasificacion");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ClasificacionesModel>>(json);
        }

        /// <summary>
        /// CLASIFICA LOS TIPOS DE OPERACION QUE SE ESTA REALIZANDO EN BASE A EL TIPO DE MOVIMIENTO
        /// </summary>
        /// <returns></returns>
        public async Task<List<TipoOperacionIGModel>> GetTipoOperacionIG()
        {
            var response = await _httpClient.GetAsync("DBContabilidad_TipoOprecionCG/TipoOperacionIngoGast");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TipoOperacionIGModel>>(json);
        }


        //=================================================================
        //====================  APIS PROVEEDORES ==========================
        //=================================================================

        /// <summary>
        /// LISTA LOS PRPOVEEDORES DE CADA CLIENTE REGISTRADO EN EL SISTEMA
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public async Task<List<ProveedoresModel>> GetProveedores(string idCliente)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DBContabilidad_Proveedores/Proveedores?idCliente={idCliente}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ProveedoresModel>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                throw;
            }

        }

        /// <summary>
        /// CONSULTA PROVEEDOR PERO POR ID
        /// </summary>
        /// <param name="idProveedor"></param>
        /// <returns></returns>
        public async Task<ProveedoresModel> GetProveedor(int idProveedor)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DBContabilidad_Proveedores/Proveedores/{idProveedor}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProveedoresModel>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                throw;
            }

        }

        /// <summary>
        /// CREANDO PROVEEDOR DE CLIENTE
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> RegistrarProveedor(ProveedoresModel add)
        {
            var apiResponse = new ApiResponse();

            try
            {
                var json = JsonSerializer.Serialize(add, new JsonSerializerOptions { WriteIndented = true });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine(json);

                var response = await _httpClient.PostAsync("DBContabilidad_Proveedores/Proveedores", content);

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.Success = true;
                    apiResponse.Message = "Proveedor registrado con éxito.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    apiResponse.Success = false;
                    apiResponse.Message = $"Error al registrar: {response.StatusCode} - {errorContent}";
                }
            }
            catch (HttpRequestException ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = $"Error en la solicitud HTTP: {ex.Message}";
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = $"Ocurrió un error inesperado: {ex.Message}";
            }

            return apiResponse;
        }

        /// <summary>
        /// ACTUALIZA 
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResponse> ActualizarProveedor(ProveedoresModel pro)
        {
            var apiResponse = new ApiResponse();
            try
            {
                var json = JsonSerializer.Serialize(pro, new JsonSerializerOptions { WriteIndented = true });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("DBContabilidad_Proveedores/Proveedores", content);

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.Success = true;
                    apiResponse.Message = "Proveedor actualizado con éxito.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    apiResponse.Success = false;
                    apiResponse.Message = $"Error al actualizar: {response.StatusCode} - {errorContent}";
                }
            }
            catch (HttpRequestException ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = $"Error en la solicitud HTTP: {ex.Message}";
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = $"Ocurrió un error inesperado: {ex.Message}";
            }

            return apiResponse;
        }

        /// <summary>
        /// ELIMINA UN PROVEEDOR
        /// </summary>
        /// <param name="idProveedor"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ApiResponse> DeleteProveedor(int idProveedor)
        {
            var response = await _httpClient.DeleteAsync($"DBContabilidad_Proveedores/Proveedores/{idProveedor}");

            var apiResponse = new ApiResponse
            {
                Success = response.IsSuccessStatusCode
            };

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                apiResponse.Message = $"Error al eliminar: {response.StatusCode} - {errorContent}";
            }

            return apiResponse;
        }

        //====================  FIN PROVEEDORES ===========================

        //=================================================================
        //====================  APIS CLIENTES DEL USUARIO==================
        //=================================================================

        /// <summary>
        /// LISTA LOS PRPOVEEDORES DE CADA CLIENTE REGISTRADO EN EL SISTEMA
        /// </summary>
        /// <param name="idCliente"></param>
        /// <returns></returns>
        public async Task<List<ClientesModel>> GetClientesInternos(string idCliente)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DBContabilidad_ClientexClts/ClientexClts?idCliente={idCliente}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ClientesModel>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                throw;
            }

        }

        /// <summary>
        /// CONSULTA PROVEEDOR PERO POR ID
        /// </summary>
        /// <param name="idProveedor"></param>
        /// <returns></returns>
        public async Task<ClientesModel> GetClienteInterno(int idClienteClt)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DBContabilidad_ClientexClts/ClientexClts/{idClienteClt}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ClientesModel>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                throw;
            }

        }

        /// <summary>
        /// CREA CLIENTE DEL USUARIO CONTABLE
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> RegistrarClienteClt(ClientesModel add)
        {
            try
            {
                var json = JsonSerializer.Serialize(add, new JsonSerializerOptions { WriteIndented = true });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("DBContabilidad_ClientexClts/ClientexClts", content);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error en la solicitud HTTPS: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// ACTUALIZA EL CLIENTE INTERNO DEL USUARIO CONTABLE
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> ActualizarClienteClt(ClientesModel pro)
        {
            try
            {
                var json = JsonSerializer.Serialize(pro, new JsonSerializerOptions { WriteIndented = true });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("DBContabilidad_ClientexClts/ClientexClts/", content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Cliente Interno actualizado con éxito.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error al actualizar el Cliente Interno. Código de estado: {response.StatusCode}");
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error en la solicitud HTTPS: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Manejar cualquier otra excepción no prevista
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// ELIMINA EL CLIENTE INTERNO DE CADA USUARIO
        /// </summary>
        /// <param name="idClienteClt"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteClienteClt(int idClienteClt)
        {
            var response = await _httpClient.DeleteAsync($"DBContabilidad_ClientexClts/ClientexClts/{idClienteClt}");
            return response.IsSuccessStatusCode;
        }
        //=================================================================
        //====================  RESOLUCIONES  =============================
        //=================================================================       

        public async Task<List<ResolucionesModel>> GetResolciones(string idCliente)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DBContabilidad_Resolucion/Resolucion?idCliente={idCliente}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ResolucionesModel>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                throw;
            }

        }

        [HttpGet]
        public async Task<ResolucionesModel> GetResolucion(int idResolucion)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DBContabilidad_Resolucion/Resolucion/{idResolucion}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResolucionesModel>(json);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                throw;
            }

        }

        [HttpPost]
        public async Task<ApiResModel> RegistrarResolucion(CrudResolucionesModel a)
        {
            try
            {
                var json = JsonSerializer.Serialize(a, new JsonSerializerOptions { WriteIndented = true });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("DBContabilidad_Resolucion/Resolucion", content);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResModel
                    {
                        Success = true,
                        Message = $"Registro con éxito.",
                        StatusCode = response.StatusCode
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResModel
                    {
                        Success = false,
                        Message = $"Error: {errorContent}",
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error de solicitud HTTP: {httpEx.Message}",
                    StatusCode = HttpStatusCode.ServiceUnavailable
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpPut]
        public async Task<ApiResModel> ActualizarResolucion(CrudResolucionesModel a)
        {
            try
            {
                var json = JsonSerializer.Serialize(a, new JsonSerializerOptions { WriteIndented = true });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("DBContabilidad_Resolucion/Resolucion/", content);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResModel
                    {
                        Success = true,
                        Message = $"Actualizacion con éxito.",
                        StatusCode = response.StatusCode
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResModel
                    {
                        Success = false,
                        Message = $"Error: {errorContent}",
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error de solicitud HTTP: {httpEx.Message}",
                    StatusCode = HttpStatusCode.ServiceUnavailable
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpDelete]
        public async Task<ApiResModel> DeleteResolucion(int idResolucion)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"DBContabilidad_Resolucion/Resolucion?id={idResolucion}");
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResModel
                    {
                        Success = true,
                        Message = $"Resolución Eliminado con exito.",
                        StatusCode = response.StatusCode
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResModel
                    {
                        Success = false,
                        Message = $"Error al eliminar registro: {errorContent}",
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error de solicitud HTTP: {httpEx.Message}",
                    StatusCode = HttpStatusCode.ServiceUnavailable
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

        }

        //=================================================================
        //====================  APIS CONSUMIDOR FINAL =====================
        //=================================================================        

        [HttpPost]
        public async Task<bool> RegistrarConsumidor(VentasConsumidorModel add)
        {
            var json = JsonSerializer.Serialize(add, new JsonSerializerOptions { WriteIndented = true });
            try
            {          
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("DBContabilidad_Consumidor/Consumidor", content);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error en la solicitud HTTP: {ex.Message} {json}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ListVentasConsumidorModel>> GetConsumidoresFinal(string idCliente, string fechaInicio, string fechaFin, int tipoFecha)
        {
            var response = await _httpClient.GetAsync($"DBContabilidad_Consumidor/Consumidor?fechaInicio={fechaInicio}&fechaFin={fechaFin}&tipoFecha={tipoFecha}&idCliente={idCliente}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ListVentasConsumidorModel>>(json);
        }



        //=================================================================
        //====================  APIS CONTRIBUYENTES  ======================
        //=================================================================        

        [HttpPost]
        public async Task<bool> RegistrarContribuyentes(VentasContribuyentes add)
        {
            var json = JsonSerializer.Serialize(add, new JsonSerializerOptions { WriteIndented = true });
            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("DBContabilidad_VentaContribuyente/Contribuyente", content);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error en la solicitud HTTP: {ex.Message} {json}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                return false;
            }
        }

       

        //=================================================================
        //====================  APIS COMPRAS ==============================
        //=================================================================

        /// <summary>
        /// LISTA TODAS LAS COMPRAS MEDIANTE PARAMETROS
        /// </summary>
        /// <param name="idCliente">ID DEL CLIENTE CONTABLE</param>
        /// <param name="fechaInicio">FECHA DE INICIO DE LA BUSQUEDA</param>
        /// <param name="fechaFin">FECHA DE FIN DE LA BUSQUEDA</param>
        /// <param name="tipoFecha">TIPO DE FECHA SI ES 1 FECHA EMISION, SI ES 2 FECHA DECLARACION/PRESENTACION</param>
        /// <returns></returns>
        public async Task<List<ListarCompras>> GetCompras(string idCliente, string fechaInicio, string fechaFin, int tipoFecha)
        {
            var response = await _httpClient.GetAsync($"DBContabilidad_Compras/Compras?idCliente={idCliente}&fechaInicio={fechaInicio}&fechaFin={fechaFin}&tipoFecha={tipoFecha}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ListarCompras>>(json);
        }


        [HttpGet]
        public async Task<CompraModel> GetCompra(int idCompra)
        {
            try
            {
                var response = await _httpClient.GetAsync($"DBContabilidad_Compras/Compras/{idCompra}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CompraModel>(json);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                throw;
            }

        }

        [HttpPost]
        public async Task<ApiResModel> RegistrarCompra(CompraModel compra)
        {
            try
            {
                var json = JsonSerializer.Serialize(compra, new JsonSerializerOptions { WriteIndented = true });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("DBContabilidad_Compras/Compras", content);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResModel
                    {
                        Success = true,
                        Message = $"Registro con éxito.",
                        StatusCode = response.StatusCode
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResModel
                    {
                        Success = false,
                        Message = $"Error: {errorContent}",
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error de solicitud HTTP: {httpEx.Message}",
                    StatusCode = HttpStatusCode.ServiceUnavailable
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }


        [HttpPut]
        public async Task<ApiResModel> ActualizarCompras(CompraModel pro)
        {
            try
            {
                var json = JsonSerializer.Serialize(pro, new JsonSerializerOptions { WriteIndented = true });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("DBContabilidad_Compras/Compras/", content);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResModel
                    {
                        Success = true,
                        Message = $"Actualizacion con éxito.",
                        StatusCode = response.StatusCode
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResModel
                    {
                        Success = false,
                        Message = $"Error: {errorContent}",
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error de solicitud HTTP: {httpEx.Message}",
                    StatusCode = HttpStatusCode.ServiceUnavailable
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }


        [HttpDelete]
        public async Task<ApiResModel> DeleteCompras(int idCompras)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"DBContabilidad_Compras/Compras/{idCompras}");
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResModel
                    {
                        Success = true,
                        Message = $"Compra Eliminado con exito.",
                        StatusCode = response.StatusCode
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResModel
                    {
                        Success = false,
                        Message = $"Error al eliminar el Compra: {errorContent}",
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error de solicitud HTTP: {httpEx.Message}",
                    StatusCode = HttpStatusCode.ServiceUnavailable
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel
                {
                    Success = false,
                    Message = $"Error inesperado: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
