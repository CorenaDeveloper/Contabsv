// Función para mostrar alertas
function showAlert(type, message) {
    var alertHtml = `
            <div class="alert alert-${type} d-flex align-items-center fade show" role="alert">
                <svg class="bi flex-shrink-0 me-2" width="24" height="24">
                    <use xlink:href="#${type === 'success' ? 'check-circle-fill' : 'exclamation-triangle-fill'}"></use>
                </svg>
                <div>${message}</div>
            </div>
        `;

    $("#alertContainer").html(alertHtml);

    // Si es éxito, ocultar después de 2 segundos
    if (type === "success") {
        setTimeout(function () {
            $("#alertContainer").html(""); // Limpia la alerta
        }, 2000);
    }
}