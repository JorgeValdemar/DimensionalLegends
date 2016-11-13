/*
DataInfoJS v1.0
Author: Jorge Valdemar da Silva
*/

function DataInfo() {
    var _self = this;

    _self.addBtnFiltros = function (objId, lista) {
        //(function ($) {

        $(objId).each(function () {
            $(this).click(function () {
                var attrFiltro = $(this).attr('data-filtro-attr'); // atributo a filtrar
                var srcFiltro = $(this).attr('data-filtro'); // conteudo a filtrar
                var rex = new RegExp(srcFiltro, 'i');

                $(lista).hide();
                $(lista).not($(lista + '.nenhum-resultado')).filter(function () {
                    // ao invés de testar $(this).text() que seria todo o texto da lista, testo o atributo.
                    return rex.test($(this).attr("data-" + attrFiltro));
                }).show();

                if ($(lista + ':visible').length == 0) {
                    $(lista + '.nenhum-resultado').show();
                };

            });
        });

        //} (jQuery));
    };

    _self.addCampoBusca = function(inputId, lista) {
        //(function ($) {

            $(inputId).keyup(function () {

                var rex = new RegExp($(this).val(), 'i');
                $(lista).hide();
                $(lista).not($(lista + '.nenhum-resultado')).filter(function () {
                    return rex.test($(this).text());
                }).show();

                if ($(lista + ':visible').length == 0) {
                    $(lista + '.nenhum-resultado').show();
                };
            })

        //} (jQuery));
    };

    _self.limparBusca = function (lista) {
        $(lista+'.nenhum-resultado').hide();
        $(lista).not($(lista + '.nenhum-resultado')).show();
    };

    //personalizar para os icones do #deck ul li e do #listaCartas ul li
    _self.construtor = function () {
        
    }

    _self.construtor();

};
