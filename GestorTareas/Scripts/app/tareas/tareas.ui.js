(function (window, $) {
    'use strict';

    var Tareas = window.Tareas = window.Tareas || {};

    var $app;
    var $lista;
    var $vacio;
    var $mensaje;
    var plantilla;
    var temporizadorMensaje;

    var DURACION_MENSAJE = 3000;
    var DURACION_ERROR = 5000;

    function inicializar($contenedor) {
        $app = $contenedor;
        $lista = $app.find('#lista-tareas');
        $vacio = $app.find('#lista-vacia');
        $mensaje = $app.find('#mensaje');
        plantilla = document.getElementById('plantilla-tarea');
    }

    function crearElementoTarea(tarea) {
        var $item = $(plantilla.content.firstElementChild.cloneNode(true));

        $item.attr('data-id', tarea.Id);

        $item.find('.js-titulo').text(tarea.Titulo);
        $item.find('.js-descripcion').text(tarea.Descripcion);
        $item.find('.js-fecha').text(tarea.FechaCreacion);

        return $item;
    }

    function renderizarLista(tareas) {
        $lista.empty().attr('aria-busy', 'false');

        $.each(tareas, function (_, tarea) {
            $lista.append(crearElementoTarea(tarea));
        });

        $vacio.prop('hidden', tareas.length > 0);
    }

    function actualizarConteo(campo) {
        var $campo = $(campo);
        var maximo = parseInt($campo.attr('maxlength'), 10);
        var usados = ($campo.val() || '').length;

        $campo.closest('.gt-campo').find('.js-conteo')
            .text(usados + '/' + maximo)
            .toggleClass('gt-conteo-limite', usados >= maximo * 0.9);
    }

    function marcarOcupado($boton, ocupado) {
        $boton.prop('disabled', ocupado).attr('aria-busy', ocupado ? 'true' : 'false');
    }

    function mostrarMensaje(texto, tipo) {
        if (!texto) {
            return;
        }

        window.clearTimeout(temporizadorMensaje);

        $mensaje
            .prop('hidden', true)
            .removeClass('gt-mensaje-exito gt-mensaje-error')
            .addClass(tipo === 'error' ? 'gt-mensaje-error' : 'gt-mensaje-exito')
            .text(texto);
        void $mensaje[0].offsetWidth;
        $mensaje.prop('hidden', false);

        temporizadorMensaje = window.setTimeout(function () {
            $mensaje.prop('hidden', true);
        }, tipo === 'error' ? DURACION_ERROR : DURACION_MENSAJE);
    }

    Tareas.ui = {
        inicializar: inicializar,
        renderizarLista: renderizarLista,
        actualizarConteo: actualizarConteo,
        marcarOcupado: marcarOcupado,
        mostrarMensaje: mostrarMensaje
    };
})(window, jQuery);
