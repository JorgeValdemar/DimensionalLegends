mainApp.controller('SelecaoCartas', function ($scope, main) {
    var _self = this;
    var bloqueio = false;
    var dicas = {};

    $scope.listaDeck = [];
    $scope.listaCartas = []; // cartas selecionadas


    function removerCarta(deckIndice) {
        var r = false;
        $scope.$apply(function () {
            for (var i = 0; i < $scope.listaDeck.length; i++) {
                if ($scope.listaDeck[i].Numero == $scope.listaCartas[deckIndice].Numero) {
                    $scope.listaDeck[i].EmUso = false;
                }
            }

            $scope.listaCartas.splice(deckIndice, 1);
            r = true;
        });

        return r;
    }

    function adicionarCarta(cardsIndice) {
        var r = false;
        $scope.$apply(function () {
            if ($scope.listaCartas.length >= 5) {
                main.modal.abrir("Atenção", "o limite de cartas é 5");
                return r;
            }
            $scope.listaDeck[cardsIndice].EmUso = true;

            for (var i = 0; i < $scope.listaCartas.length; i++) {
                if ($scope.listaDeck[cardsIndice].Numero == $scope.listaCartas[i].Numero) {
                    main.modal.abrir("Atenção", "Esta carta já está adicionada.");
                    return r;
                }
            }

            $scope.listaCartas.push($scope.listaDeck[cardsIndice]);
            r = true;
        });

        return r;
    };

    function confirmarPartida() {
        $('#salvarDeck').unbind();
        $('#salvarDeck').bind('click', function () {
            main.modal.abrir('Iniciar partida',
                'Você está a iniciar uma partida.'
                + '<br />'
                + 'Em caso de dúvidas veja o tutorial no menu principal.'
                + '<br />'
                + '<br />'
                + 'Quer iniciar a partida?',
                function () {
                    $('#faseListaCartas').val( JSON.stringify($scope.listaCartas) );
                    $('#formSlcCartas').submit();
                }
            );
        });
    };

    $scope.atualizarCartaPopup = function (objThis, opcionalInfo) {
        var num = $(objThis).parents("li").index();
        if (opcionalInfo == 'deck') {
            $scope.$apply(function () {
                $scope.main.carta = main.getCardByIndex(num, $scope.listaDeck);
            });
        } else if (opcionalInfo == 'listaCartas') {
            $scope.$apply(function () {
                $scope.main.carta = main.getCardByIndex(num, $scope.listaCartas);
            });
        }
    }

    $scope.lerDeck = function (callback) {

        $.ajax({
            type: "POST",
            url: "/aplicacao/Cartas/LerPlayerDeck.ashx",
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

    $scope.refreshDeck = function () {
        $('#colunaDeck .deletar-card-deck').bind("click", function () {
            var num = $(this).parents("li").index();

            if (adicionarCarta(num)) {
                $('#colunaDeck .deletar-card-deck').unbind();
                $('#colunaCartas .adicionar-card').unbind();

                $scope.refreshDeck();
                $scope.refreshCards();
                confirmarPartida();
            }
        });

        main.dicas();
        main.cartaPopup(function (objThis, opcionalInfo) { $scope.atualizarCartaPopup(objThis, opcionalInfo) }, true);
        
    };

    $scope.refreshCards = function () {
        $('#colunaCartas .adicionar-card').bind("click", function () {
            var num = $(this).parents("li").index();

            if (removerCarta(num)) {
                $('#colunaDeck .deletar-card-deck').unbind();
                $('#colunaCartas .adicionar-card').unbind();

                $scope.refreshDeck();
                $scope.refreshCards();
                confirmarPartida();
            };
        });

        main.dicas();
        main.cartaPopup(function (objThis, opcionalInfo) { $scope.atualizarCartaPopup(objThis, opcionalInfo) }, true);

    };


    var construct = function () {

        dicas = new main.dicas();

        main.lerPlayer(function () {
            $scope.$apply(function () {
                $scope.main = main;
            });
        });

        main.lerOpcoes(function () {
            $scope.$apply(function () {
                $scope.main = main;
            });
        });

        $scope.lerDeck(function (resp) {

            $scope.$apply(function () {
                $scope.listaDeck = resp.ListaCards;
            });

            $scope.refreshDeck();
            $scope.refreshCards();
        });

        $('.enscrollbox').enscroll({
            verticalTrackClass: 'track',
            verticalHandleClass: 'handle',
            minScrollbarLength: 28,
            drawScrollButtons: false
        });

        $('.voltar').click(function () {
            window.open(main.appRoot + '/MenuPrincipal', '_self');
        });


    };



    $(function () {
        construct();
    });
});


