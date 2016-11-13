mainApp.controller('MinhasCartas', function ($scope, main) {
    var _self = this;
    var bloqueio = false;
    var datainfo;
    var listaCartasFiltro = '#listaCartas ul li';
    var listaDeckFiltro = '#deck ul li';

    $scope.listaDeck = [];
    $scope.listaCartas = [];


    function removerTodasAsCartas() {
        var r = false;
        $scope.$apply(function () {
            for (var i = 0; i < $scope.listaCartas.length; i++) {
                $scope.listaCartas[i].EmUso = false;
            }

            $scope.listaDeck = [];
            r = true;
        });

        return r;
    }

    function removerCarta(deckIndice) {
        var r = false;
        $scope.$apply(function () {
            for (var i = 0; i < $scope.listaCartas.length; i++) {
                if ($scope.listaCartas[i].Numero == $scope.listaDeck[deckIndice].Numero) {
                    $scope.listaCartas[i].EmUso = false;
                }
            }

            $scope.listaDeck.splice(deckIndice, 1);
            r = true;
        });

        return r;
    }


    $scope.salvarDeck = function () {
        if (bloqueio) return;
        bloqueio = true;
        var ListaCards = $scope.listaDeck;

        $.ajax({
            type: "POST",
            url: "/aplicacao/Cartas/AtualizarPlayerDeck.ashx",
            data: "data=" + JSON.stringify(ListaCards),
            success: function (resp) {
                bloqueio = false;
                resp = JSON.parse(resp);
                if (resp.Erro) {
                    main.modal.abrir('Erro ao salvar', resp.ErroDescricao);
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



    function adicionarCarta(cardsIndice) {
        var r = false;
        $scope.$apply(function () {
            if ($scope.listaDeck.length >= 10) {
                main.modal.abrir("Atenção", "o limite de cartas é 10");
                return r;
            }

            $scope.listaCartas[cardsIndice].EmUso = true;

            for (var i = 0; i < $scope.listaDeck.length; i++) {
                if ($scope.listaDeck[i].Numero == $scope.listaCartas[cardsIndice].Numero) {
                    main.modal.abrir("Atenção", "Esta carta já está adicionada.");
                    return r;
                }
            }

            $scope.listaDeck.push($scope.listaCartas[cardsIndice]);
            r = true;
        });

        return r;
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


    $scope.lerCartas = function (callback) {

        $.ajax({
            type: "POST",
            url: "/aplicacao/Cartas/LerPlayerCards.ashx",
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

    $scope.refreshDeck = function () {
        $('#colunaDeck .deletar-card-deck').bind("click", function () {
            var num = $(this).parents("li").index();

            if (removerCarta(num)) {
                $('#colunaDeck .deletar-card-deck').unbind();
                $('#colunaCartas .adicionar-card').unbind();

                $scope.refreshDeck();
                $scope.refreshCards();
            }
        });


        $('#removerDeck').bind("click", function () {
            if (removerTodasAsCartas()) {
                $('#colunaDeck .deletar-card-deck').unbind();
                $('#colunaCartas .adicionar-card').unbind();

                $scope.refreshDeck();
                $scope.refreshCards();
            }
        });

        main.dicas();
        main.cartaPopup(function (objThis, opcionalInfo) { $scope.atualizarCartaPopup(objThis, opcionalInfo) }, true);

        datainfo.addBtnFiltros('#colunaDeck .elemento-ico', listaDeckFiltro);
        datainfo.addBtnFiltros('#colunaDeck .rank-ico', listaDeckFiltro);
    };

    $scope.refreshCards = function () {
        $('#colunaCartas .adicionar-card').bind("click", function () {
            var num = $(this).parents("li").index();

            if (adicionarCarta(num)) {
                $('#colunaDeck .deletar-card-deck').unbind();
                $('#colunaCartas .adicionar-card').unbind();

                $scope.refreshDeck();
                $scope.refreshCards();

                $('#deck.enscrollbox').animate({
                    scrollTop: $('#deck ul').outerHeight(true)
                }, 600);
            };
        });

        main.dicas();
        main.cartaPopup(function (objThis, opcionalInfo) { $scope.atualizarCartaPopup(objThis, opcionalInfo) }, true);

        datainfo.addCampoBusca('#filter', listaCartasFiltro);
        datainfo.addBtnFiltros('#colunaCartas .elemento-ico', listaCartasFiltro);
        datainfo.addBtnFiltros('#colunaCartas .rank-ico', listaCartasFiltro);
    };

    var construct = function () {

        $scope.main = main;
        datainfo = new DataInfo();

        main.lerPlayer(function () {
            $scope.$apply(function () {
                $scope.main.user = main.user;
            });
        });

        main.lerOpcoes(function () {
            $scope.$apply(function () {
                $scope.main.opcoes = main.opcoes;
            });
        });



        $scope.lerDeck(function (resp) {

            $scope.$apply(function () {
                $scope.listaDeck = resp.ListaCards;
            });

            $scope.refreshDeck();
        });



        $scope.lerCartas(function (resp) {

            $scope.$apply(function () {
                $scope.listaCartas = resp.ListaCards;
            });

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

        $('#limparCartas').click(function () {
            datainfo.limparBusca(listaCartasFiltro);
        });

        $('#limparDeck').click(function () {
            datainfo.limparBusca(listaDeckFiltro);
        });

        $('#salvarDeck').click(function () {
            if ($scope.listaDeck.length != 10) {
                main.modal.abrir("Atenção", "O número de cartas deve ser exatamente 10");
                return;
            }
            $scope.salvarDeck();
        });

    };



    $(function () {
        construct();
    });
});

