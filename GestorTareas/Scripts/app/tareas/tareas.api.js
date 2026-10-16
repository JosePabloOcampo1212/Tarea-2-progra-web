(function (window, $) {
    'use strict';

    var Tareas = window.Tareas = window.Tareas || {};

    var urls = {};

    var token = '';

    function configurar($app) {
        urls = {
            listar: $app.attr('data-url-listar'),
            crear: $app.attr('data-url-crear'),
            cambiarEstado: $app.attr('data-url-cambiar-estado')
        };

        token = $app.find('input[name="__RequestVerificationToken"]').val();
    }

    function enviarPost(url, datos) {
        return $.ajax({
            url: url,
            type: 'POST',
            data: $.extend({ __RequestVerificationToken: token }, datos),
            dataType: 'json'
        });
    }

    Tareas.api = {
        configurar: configurar,

        listar: function () {
            return $.ajax({
                url: urls.listar,
                type: 'GET',
                dataType: 'json',
                cache: false
            });
        },

        crear: function (titulo, descripcion) {
            return enviarPost(urls.crear, { titulo: titulo, descripcion: descripcion });
        },

        cambiarEstado: function (id) {
            return enviarPost(urls.cambiarEstado, { id: id });
        }
    };
})(window, jQuery);
