(function (window, $) {
    'use strict';

    var Tareas = window.Tareas;

    function cargarTareas() {
        return Tareas.api.listar()
            .done(function (respuesta) {
                if (!respuesta.Exito) {
                    Tareas.ui.mostrarMensaje(respuesta.Mensaje, 'error');
                    return;
                }

                Tareas.ui.renderizarLista(respuesta.Datos.Tareas);
            })
            .fail(mostrarErrorDeConexion);
    }

    function mostrarErrorDeConexion() {
        Tareas.ui.mostrarMensaje('No se pudo completar la operación. Intente de nuevo.', 'error');
    }

    $(function () {
        var $app = $('#app-tareas');

        if (!$app.length) {
            return;
        }

        Tareas.api.configurar($app);
        Tareas.ui.inicializar($app);

        cargarTareas();
    });
})(window, jQuery);
