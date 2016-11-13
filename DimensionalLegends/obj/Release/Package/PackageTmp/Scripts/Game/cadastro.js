mainApp.controller('Cadastro', function ($scope, main) {
    var _self = this;
    var bloqueio = false;

    // contem os 4 decks iniciais
    $scope.deck = {
        shounen: [],
        shoujo: [],
        gamer: [],
        expert: []
    };

    $scope.listaCartas = [];

    $scope.Avatar = '';
    $scope.DeckNumero = 1;
    $scope.NumeroImagem = '';


    $scope.salvarDeck = function () {
        if (bloqueio) return;
        bloqueio = true;

        var data = {
            nomeChar: $scope.Avatar,
            imagemChar: $scope.NumeroImagem,
            deckChar: $scope.DeckNumero
        };

        $.ajax({
            type: "POST",
            url: "/aplicacao/Charcreate/Cadastro.ashx",
            data: "data=" + JSON.stringify(data),
            success: function (resp) {
                bloqueio = false;

                resp = JSON.parse(resp);
                if (resp.Erro) {
                    if (resp.NomeExistente) {
                        main.modal.abrir('Não foi possível salvar, verifique abaixo:', 'Nome selecionado já existe.');
                    } else {
                        main.modal.abrir('Não foi possível salvar, verifique abaixo:', resp.ErroDescricao);
                    };
                } else {
                    window.open(main.appRoot + "/MenuPrincipal", "_self");
                };

            },
            error: function () {
                bloqueio = false;
                main.modal.abrir('Erro', 'houve um problema com a resposta do servidor');
            },
            statusCode: {
                404: function () {
                    bloqueio = false;
                    main.modal.abrir('Erro status', '404');
                }
            }
        });
    };


    $scope.lerDeck = function (num, callback) {

        var data = {
            Deck: num
        }

        $.ajax({
            type: "POST",
            url: "/aplicacao/Charcreate/DecksIniciais.ashx",
            data: "data=" + JSON.stringify(data),
            success: function (resp) {
                bloqueio = false;
                resp = JSON.parse(resp);
                if (resp.Erro) {
                    main.modal.abrir('Erro', resp.ErroDescricao);
                } else {
                    callback(resp);
                };
            },
            error: function () {
                bloqueio = false;
                main.modal.abrir('Erro', 'houve um problema com a resposta do servidor');
            },
            statusCode: {
                404: function () {
                    bloqueio = false;
                    main.modal.abrir('Erro status', '404');
                }
            }
        });

    };


    $scope.atualizarCartaPopup = function (objThis, opcionalInfo) {
        var num = $(objThis).parents("li").index();
        $scope.$apply(function () {
            $scope.main.carta = main.getCardByIndex(num, $scope.listaCartas);
        });
    }


    $scope.atualizarEventos = function (objThis) {
        objThis = typeof (objThis) != 'undefined' ? objThis : 1;

        $scope.$apply(function () {
            $scope.DeckNumero = objThis;
        });

        switch (objThis) {
            case '1':
                $scope.$apply(function () {
                    $scope.listaCartas = $scope.deck.shounen;
                });
                break;
            case '2':
                $scope.$apply(function () {
                    $scope.listaCartas = $scope.deck.shoujo;
                });
                break;
            case '3':
                $scope.$apply(function () {
                    $scope.listaCartas = $scope.deck.gamer;
                });
                break;
            case '4':
                $scope.$apply(function () {
                    $scope.listaCartas = $scope.deck.expert;
                });
                break;
        }

        main.dicas();
        main.cartaPopup(function (objThis, opcionalInfo) { $scope.atualizarCartaPopup(objThis, opcionalInfo) }, true);

        $('#cadastroImagensLista li').unbind();
        $('#cadastroImagensLista li').bind("click", function () {
            var num = $(this).attr('data-numero');
            $scope.$apply(function () {
                $scope.NumeroImagem = num;
            });
        });
    }

    var construct = function () {

        $scope.main = main;

        $('header').css('display', 'none');

        $scope.lerDeck(1, function (resp) {
            $scope.$apply(function () {
                $scope.deck.shounen = resp.ListaCards;
                $scope.listaCartas = $scope.deck.shounen;
            });
            $scope.atualizarEventos();
        });


        $scope.lerDeck(2, function (resp) {
            $scope.$apply(function () {
                $scope.deck.shoujo = resp.ListaCards;
            });
        });


        $scope.lerDeck(3, function (resp) {
            $scope.$apply(function () {
                $scope.deck.gamer = resp.ListaCards;
            });
        });


        $scope.lerDeck(4, function (resp) {
            $scope.$apply(function () {
                $scope.deck.expert = resp.ListaCards;
            });
        });

        $('.enscrollbox').enscroll({
            verticalTrackClass: 'track',
            verticalHandleClass: 'handle',
            minScrollbarLength: 28,
            drawScrollButtons: false
        });

        $('#colunaCadImgs').enscroll({
            horizontalScrolling: true,
            horizontalTrackClass: 'horizontal-track',
            horizontalHandleClass: 'horizontal-handle',
            minScrollbarLength: 28,
            drawScrollButtons: false
        });

        $('.deck-option input').click(function () {
            $scope.atualizarEventos($(this).attr('data-id'));
        });

        $('.voltar').click(function () {
            window.open(main.appRoot + '/MenuPrincipal', '_self');
        });

        $('#salvarCadastro').click(function () {
            $scope.salvarDeck();
        });

    };



    $(function () {
        construct();
    });
});

