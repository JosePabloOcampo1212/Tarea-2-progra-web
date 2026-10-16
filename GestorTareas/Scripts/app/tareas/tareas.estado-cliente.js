(function (window) {
    'use strict';

    var Tareas = window.Tareas = window.Tareas || {};

    var CLAVE_FILTRO = 'gestorTareas.filtro';

    var FILTROS_VALIDOS = ['todas', 'pendientes', 'completadas'];

    var FILTRO_POR_DEFECTO = 'todas';

    function obtenerFiltro() {
        try {
            var valor = window.localStorage.getItem(CLAVE_FILTRO);

            return FILTROS_VALIDOS.indexOf(valor) >= 0 ? valor : FILTRO_POR_DEFECTO;
        } catch (e) {
            return FILTRO_POR_DEFECTO;
        }
    }

    function guardarFiltro(filtro) {
        if (FILTROS_VALIDOS.indexOf(filtro) < 0) {
            return;
        }

        try {
            window.localStorage.setItem(CLAVE_FILTRO, filtro);
        } catch (e) {
        }
    }

    Tareas.estadoCliente = {
        obtenerFiltro: obtenerFiltro,
        guardarFiltro: guardarFiltro
    };
})(window);
