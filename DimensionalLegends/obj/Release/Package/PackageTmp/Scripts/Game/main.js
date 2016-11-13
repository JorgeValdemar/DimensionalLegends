var mainApp = angular.module('mainApp', []);
mainApp.service('main', function () {
    var scope = {};
    scope.appRoot = '';//'/Game';
    scope.wsRoot = 'ws://localhost:53967/Aplicacao/WS';
//    scope.wsRoot = 'ws://dimensionallegends.com/Aplicacao/WS';
    scope.user = sessionStorage.user;
    scope.opcoes = sessionStorage.opcoes;
    scope.defaultAttrCarta = [
        { Atributo: 'A1' },
        { Atributo: 'A2' },
        { Atributo: 'A3' },
        { Atributo: 'B1' },
        { Atributo: 'B2' },
        { Atributo: 'C1' },
        { Atributo: 'C2' },
        { Atributo: 'C3' },
    ];
    scope.opcaoTempCartaTipo = 'default';
    scope.carta = {
        Numero: 1,
        Rank: 0,
        Nome: '',
        A1: '',
        A2: '',
        A3: '',
        B1: '',
        B2: '',
        C1: '',
        C2: '',
        C3: '',
        Elemento: {
            ElementoId: 0,
            Elemento: ''
        }
    };

    // ATENCAO: apenas icones de elemento em class
    scope.icones = [
        'glyphicon glyphicon-fire', 
        'glyphicon glyphicon-asterisk', 
        'glyphicon glyphicon-tower', 
        'glyphicon glyphicon-flash', 
        'glyphicon glyphicon-certificate', 
        'glyphicon glyphicon-eye-open', 
        'glyphicon glyphicon-tint', 
        'glyphicon glyphicon-cloud'
    ];


    scope.fancyboxVideo = function (titulo) {
        $(".video").click(function () {
            $.fancybox({
                'padding': 0,
                'autoScale': false,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'title': titulo,
                'width': 600,
                'height': 385,
                'href': this.href.replace(new RegExp("watch\\?v=", "i"), 'v/'),
                'type': 'swf',
                'swf': {
                    'wmode': 'transparent',
                    'allowfullscreen': 'true'
                }
            });
        });
    }

    // preciso para ativar as listas de eventos
    scope.evtTriggers = function (obj, bind) {
        $(obj).find("[data-evt!='']").each(function () {
            var event = $(this).attr('data-evt');
            if (typeof (event) != 'undefined') {
                $(this).bind(bind, function () {
                    window.open(event, '_self');
                });
            };
        });
    }

    scope.getCardByIndex = function (index, arrCartas) {
        return arrCartas[index];
    }

    scope.dicas = function () {
        var defaultHeight = 700;
        var _dicaSelf = this;

        _dicaSelf.fechar = function () {
            $('#dica').css('display', 'none');
        };

        _dicaSelf.abrir = function (textoDica, objTarget, useCanvas) {
            objTarget = typeof (objTarget) != 'undefined' ? objTarget : "canvas.usa-dicas";
            useCanvas = typeof (useCanvas) != 'undefined' ? useCanvas : true;


            $('#dica').css('display', 'block');
            $('#dicaTxt').html(textoDica);

            var dicaW = $('#dica').outerWidth(true);
            var dicaH = $('#dica').outerHeight(true);
            var p = $(objTarget).offset();
            var top = p.top;
            var left = p.left;
            var reverseValLeft = $('body').width() / 2;
            var reverseValTop = $('body').height() / 2;

            // as quests da altura ainda n縊 est縊 totalmente definidas, como o body retorna o conteudo total visivel para ele
            // tomamos um valor default como providencia
            if (reverseValTop < defaultHeight / 2) {
                reverseValTop = defaultHeight / 2;
            }

            if (objTarget != "canvas.usa-dicas") {
                // abaixo os ifs servem para que o popup n縊 saia da tela
                if (reverseValLeft < left) {
                    left = left - dicaW;
                } else {
                    left = left + $(objTarget).outerWidth(true);
                }

                if (reverseValTop < top) {
                    top = top - dicaH;
                } else {
                    top = top + $(objTarget).outerHeight(true);
                }
            }
            $('#dica').css('top', top + 'px');
            $('#dica').css('left', left + 'px');
        }

        _dicaSelf.construct = function () {

            $('[data-toggle="dica"]').each(function () {
                $(this).bind('mouseenter', function () {
                    var descr = $(this).attr('data-descr');
                    _dicaSelf.abrir(descr, this, false);
                });
                $(this).bind('mouseleave', function () {
                    $('#dica').css('display', 'none');
                });
            });
        }

        _dicaSelf.construct();

    };



    scope.cartaPopup = function (callback, opcionalInfo) {
        callback = typeof (callback) != 'undefined' ? callback : function (obj) { };
        opcionalInfo = typeof (opcionalInfo) != 'undefined' ? opcionalInfo : false;
        var defaultHeight = 700;

        $('[data-toggle="carta"]').unbind();
        $('[data-toggle="carta"]').each(function () {
            $(this).bind('mouseenter', function () {
                // ATENﾇﾃO: Por alguma raz縊 n縊 pode haver nada antes deste callback, surge um bug de evento
                // tem que retornar objeto carta
                if (opcionalInfo) {
                    callback(this, $(this).attr('data-info'));
                } else {
                    callback(this);
                }

                $('.carta.forma-' + scope.opcaoTempCartaTipo).css('display', 'block');

                var dicaW = $('.carta.forma-' + scope.opcaoTempCartaTipo).outerWidth(true);
                var dicaH = $('.carta.forma-' + scope.opcaoTempCartaTipo).outerHeight(true);
                var p = $(this).offset();
                var top = p.top;
                var left = p.left;
                var reverseValLeft = $('body').width() / 2;
                var reverseValTop = $('body').height() / 2;

                // as quests da altura ainda n縊 est縊 totalmente definidas, como o body retorna o conteudo total visivel para ele
                // tomamos um valor default como providencia
                if (reverseValTop < defaultHeight / 2) {
                    reverseValTop = defaultHeight / 2;
                }

                // abaixo os ifs servem para que o popup n縊 saia da tela
                if (reverseValLeft < left) {
                    left = left - dicaW;
                } else {
                    left = left + $(this).outerWidth(true);
                }

                if (reverseValTop < top) {
                    top = top - dicaH;
                } else {
                    top = top + $(this).outerHeight(true);
                }

                $('.carta.forma-' + scope.opcaoTempCartaTipo).css('top', top + 'px');
                $('.carta.forma-' + scope.opcaoTempCartaTipo).css('left', left + 'px');
            });
            $(this).bind('mouseleave', function () {
                $('.carta.forma-' + scope.opcaoTempCartaTipo).css('display', 'none');
            });
        });
    };

    scope.newCard = function ($target, card, callback) {
        callback = typeof callback === undefined ? function () { } : callback;

        var $cartaClone = $('footer .carta.forma-default').clone();
        $cartaClone.addClass('no-select-text');
        $cartaClone.attr('data-card-number', card.Numero);

        for (var i = 0; i < scope.defaultAttrCarta.length; i++) {
            var $div = $('<div>');
            var $cardAttrDiv = $('<div>');
            var $valorSpan = $('<span>');

            $div.addClass(scope.defaultAttrCarta[i].Atributo);

            var classesAttrDiv = card[scope.defaultAttrCarta[i].Atributo] != 'E' ?
                card[scope.defaultAttrCarta[i].Atributo] > 6 ?
                    'attr-ico attr-forte' : card[scope.defaultAttrCarta[i].Atributo] < 3
                        ? 'attr-ico attr-fraco' : card[scope.defaultAttrCarta[i].Atributo] == 'S'
                            ? 'attr-ico cor-especial'
                : 'attr-ico'
            : 'attr-ico cor-elemental';

            $cardAttrDiv.addClass(classesAttrDiv);

            if (card[scope.defaultAttrCarta[i].Atributo] == 'S') {
                $valorSpan.addClass('cor-txt-especial');
            }

            $valorSpan.addClass('ng-binding ng-scope');

            if (card[scope.defaultAttrCarta[i].Atributo] != 'E') {
                $valorSpan.text(card[scope.defaultAttrCarta[i].Atributo]);
            } else {
                $valorSpan.addClass(scope.icones[card.Elemento.ElementoId - 1]);
                $valorSpan.text('6');
            }

            $cardAttrDiv.append($valorSpan);

            if (card[scope.defaultAttrCarta[i].Atributo] == '0') {
                $div.addClass('hidden');
            }

            $div.append($cardAttrDiv);
            $cartaClone.append($div);
        }

        // abusar do recurso, trara lentidão.
        $cartaClone.find('.compass .circle-g').attr('class', 'circle-g rank-' + card.Rank);
        $cartaClone.find('.compass .rankBandeira span').attr('class', 'glyphicon glyphicon-bookmark rank-' + card.Rank);
        $cartaClone.css('background-image', 'url(/Imagens/cards/cartas/' + card.Numero + '.jpg)');

        callback($cartaClone);

        $target.addClass('has-card');
        $target.append($cartaClone);
    };


    scope.lerPlayer = function (callback) {

        $.ajax({
            type: "POST",
            url: "/aplicacao/Utils/PlayerStatus.ashx",
            success: function (resp) {
                bloqueio = false;
                resp = JSON.parse(resp);
                if (resp.Erro) {
                    scope.modal.abrir('Erro', resp.ErroDescricao);
                } else {

                    scope.user = {
                        Coins: resp.PlayerStatus.Coins,
                        Imagem: resp.PlayerStatus.Imagem,
                        Level: resp.PlayerStatus.Level,
                        MaxHp: resp.PlayerStatus.MaxHp,
                        MaxMp: resp.PlayerStatus.MaxMp,
                        MaxSp: resp.PlayerStatus.MaxSp,
                        Nick: resp.PlayerStatus.Nick
                    };

                    sessionStorage.user = scope.user;

                    callback();
                };

            },
            error: function () {
                bloqueio = false;
                scope.modal.abrir('Erro', 'houve um problema com a resposta do servidor');
            },
            statusCode: {
                404: function () {
                    bloqueio = false;
                    scope.modal.abrir('Erro status', '404');
                }
            }
        });
    };


    scope.lerOpcoes = function (callback) {

        $.ajax({
            type: "POST",
            url: "/aplicacao/Utils/Opcoes.ashx",
            success: function (resp) {
                bloqueio = false;
                resp = JSON.parse(resp);
                if (resp.Erro) {
                    scope.modal.abrir('Erro', resp.ErroDescricao);
                } else {

                    scope.opcoes = {
                        Idioma: resp.Opcoes.Idioma,
                        Som: resp.Opcoes.Som,
                        ChibiAssistente: resp.Opcoes.ChibiAssistente,
                        Assistente: {
                            ChibiId: resp.Opcoes.Assistente.ChibiId,
                            Nome: resp.Opcoes.Assistente.Nome,
                            Imagem: resp.Opcoes.Assistente.Imagem
                        }
                    };

                    sessionStorage.opcoes = scope.opcoes;

                    callback();
                };

            },
            error: function () {
                bloqueio = false;
                scope.modal.abrir('Erro', 'houve um problema com a resposta do servidor');
            },
            statusCode: {
                404: function () {
                    bloqueio = false;
                    scope.modal.abrir('Erro status', '404');
                }
            }
        });
    };


    var construct = function () {

        scope.modal = new Modal();

    };

    construct();

    return scope;
});
