(function (window, $) {
    'use strict';

    var Tareas = window.Tareas;

    var filtroActual = 'todas';

    function cargarTareas() {
        return Tareas.api.listar(filtroActual)
            .done(function (respuesta) {
                if (!respuesta.Exito) {
                    Tareas.ui.mostrarMensaje(respuesta.Mensaje, 'error');
                    return;
                }

                Tareas.ui.renderizarLista(respuesta.Datos.Tareas, filtroActual);
                Tareas.ui.renderizarResumen(respuesta.Datos.Resumen);
            })
            .fail(mostrarErrorDeConexion);
    }

    function procesarOperacion(peticion, $boton) {
        var resultado = $.Deferred();

        if ($boton) {
            Tareas.ui.marcarOcupado($boton, true);
        }

        peticion
            .done(function (respuesta) {
                Tareas.ui.mostrarMensaje(respuesta.Mensaje, respuesta.Exito ? 'exito' : 'error');
                cargarTareas().always(function () {
                    resultado.resolve(respuesta);
                });
            })
            .fail(function () {
                mostrarErrorDeConexion();
                resultado.reject();
            })
            .always(function () {
                if ($boton) {
                    Tareas.ui.marcarOcupado($boton, false);
                }
            });

        return resultado.promise();
    }

    function mostrarErrorDeConexion() {
        Tareas.ui.mostrarMensaje('No se pudo completar la operacion. Intente de nuevo.', 'error');
    }

    function idDeTarea(elemento) {
        return $(elemento).closest('.gt-tarea').attr('data-id');
    }

    $(function () {
        var $app = $('#app-tareas');
        var $formCrear = $('#form-crear');

        if (!$app.length) {
            return;
        }

        Tareas.api.configurar($app);
        Tareas.ui.inicializar($app);

        filtroActual = Tareas.estadoCliente.obtenerFiltro();
        Tareas.ui.marcarFiltroActivo(filtroActual);

        $formCrear.find('[maxlength]').each(function () {
            Tareas.ui.actualizarConteo(this);
        });
        $app.on('input', '[maxlength]', function () {
            Tareas.ui.actualizarConteo(this);
        });

        $formCrear.on('submit', function (evento) {
            evento.preventDefault();

            var formulario = this;
            var titulo = $(formulario).find('[name="titulo"]').val();
            var descripcion = $(formulario).find('[name="descripcion"]').val();

            procesarOperacion(Tareas.api.crear(titulo, descripcion), $(formulario).find('[type="submit"]'))
                .done(function (respuesta) {
                    if (respuesta.Exito) {
                        formulario.reset();
                        $(formulario).find('[maxlength]').each(function () {
                            Tareas.ui.actualizarConteo(this);
                        });
                        $(formulario).find('[name="titulo"]').trigger('focus');
                    }
                });
        });

        $app.on('change', '.js-estado', function () {
            var id = idDeTarea(this);

            Tareas.ui.marcarCompletada($(this).closest('.gt-tarea'), this.checked);

            procesarOperacion(Tareas.api.cambiarEstado(id))
                .done(function () {
                    Tareas.ui.enfocarTarea(id, '.js-estado');
                });
        });

        $app.on('click', '.js-editar', function () {
            Tareas.ui.mostrarEdicion($(this).closest('.gt-tarea'), true);
        });

        $app.on('click', '.js-cancelar', function () {
            Tareas.ui.mostrarEdicion($(this).closest('.gt-tarea'), false);
        });

        $app.on('submit', '.js-form-editar', function (evento) {
            evento.preventDefault();

            var $item = $(this).closest('.gt-tarea');

            procesarOperacion(Tareas.api.editar(
                idDeTarea(this),
                $item.find('.js-edit-titulo').val(),
                $item.find('.js-edit-descripcion').val()
            ), $(this).find('[type="submit"]'));
        });

        $app.on('click', '.js-eliminar', function () {
            Tareas.ui.mostrarConfirmacion($(this).closest('.gt-tarea'), true);
        });

        $app.on('click', '.js-confirmar-no', function () {
            Tareas.ui.mostrarConfirmacion($(this).closest('.gt-tarea'), false);
        });

        $app.on('click', '.js-confirmar-si', function () {
            procesarOperacion(Tareas.api.eliminar(idDeTarea(this)), $(this));
        });

        $app.on('keydown', '.gt-tarea', function (evento) {
            if (evento.key !== 'Escape') {
                return;
            }

            var $item = $(this);

            if (!$item.find('.js-form-editar').prop('hidden')) {
                Tareas.ui.mostrarEdicion($item, false);
            } else if (!$item.find('.js-confirmacion').prop('hidden')) {
                Tareas.ui.mostrarConfirmacion($item, false);
            }
        });

        $app.on('click', '.js-filtro', function () {
            filtroActual = $(this).attr('data-filtro');

            Tareas.estadoCliente.guardarFiltro(filtroActual);
            Tareas.ui.marcarFiltroActivo(filtroActual);
            cargarTareas();
        });

        cargarTareas();
    });
})(window, jQuery);
