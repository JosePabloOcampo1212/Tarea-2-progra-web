(function (window, $) {
    'use strict';

    var Tareas = window.Tareas = window.Tareas || {};

    var urls = {};

    function configurar($app) {
        urls = {
            listar: $app.attr('data-url-listar')
        };
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
        }
    };
})(window, jQuery);
