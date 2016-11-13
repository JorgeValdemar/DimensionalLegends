function Modal() {
    var _self = this;

    _self.abrir = function (titulo, conteudo, continuarCallback) {
        titulo = typeof (titulo) != 'undefined' ? titulo : '';
        conteudo = typeof (conteudo) != 'undefined' ? conteudo : '';
        continuarCallback = typeof (continuarCallback) != 'undefined' ? continuarCallback : false;

        $('#popupModal').find('.modal-title').html(titulo);
        $('#popupModal').find('.modal-body').html(conteudo);
        $('#popupModal').modal('show');

        if (!continuarCallback) {
            $('#popupModal').find('.modal-footer .btn.btn-primary').hide();
        } else {
            $('#popupModal').find('.modal-footer .btn.btn-primary').show();
            $('#popupModal').find('.modal-footer .btn.btn-primary').bind('click', function () {
                continuarCallback();
                // Se um dia eu n quiser que feche automatico ÅEsÅEmudar aqui por parametro
                // tambem posso usar um return do callback
                _self.fechar();
            });
        }
    };

    _self.fechar = function () {
        $('#popupModal').find('.modal-title').html('');
        $('#popupModal').find('.modal-body').html('');
        $('#popupModal').modal('hide');
    };

};
