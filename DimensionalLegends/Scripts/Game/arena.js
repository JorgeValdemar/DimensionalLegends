mainApp.controller('Arena', function ($scope, main) {
    var _self = this;
    var bloqueio = false;
    var dicas = {};
    var loopTimeOut = [];

    $scope.listaDeck = [];
    $scope.listaCartas = []; // cartas selecionadas
    $scope.musica = {};
    $scope.arenaConfig = {};
    $scope.arena = [];
    $scope.arenaSituacao = [];


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

    function entrarArena(callback) {

        var data = {
            Id: $('#faseId').val(), // fase ID
            ListaCards: $scope.listaCartas // lista de cartas
        };

        $.ajax({
            type: "POST",
            url: "/Aplicacao/Arena/ConfigurarEntrada.ashx",
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

    }


    $scope.turnoMudar = function(numero, x, y, callback) {

        var data = {
            id: $scope.arenaConfig.Id, 
            CardIndexEscolhido: numero,
            PosXEscolhido: x,
            PosYEscolhido: y
        };

        $.ajax({
            type: "POST",
            url: "/Aplicacao/Arena/TurnoAcao.ashx",
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

    }

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

    $scope.pontuacaoAtualizar = function () {
        var p1pt = $scope.arenaConfig.P1_pontos + 5;
        var p2pt = $scope.arenaConfig.P2_pontos + 5;
        var $listaP1 = $('#placarP1');
        var $listaP2 = $('#placarP2');
        $listaP1.find('li').removeClass('active');
        $listaP2.find('li').removeClass('active');

        for (var i = 0; i < p1pt; i++) {
            $listaP1.find('li').eq(i).addClass('active');
        }

        for (var i = 0; i < p2pt; i++) {
            $listaP2.find('li').eq(i).addClass('active');
        }

        if ($scope.arenaConfig.Encerrada) {
            if ($listaP1.find('li.active').length > $listaP2.find('li.active').length) {
                alert('Vitória!!!\nEm breve o game será mais dinamico e cartas poderão ser adquiridas.');
                window.open(main.appRoot + '/MenuPrincipal', '_self');
            } else if ($listaP1.find('li.active').length < $listaP2.find('li.active').length) {
                alert('Perda\nEm breve o game será mais dinamico e cartas poderão ser adquiridas.');
                window.open(main.appRoot + '/MenuPrincipal', '_self');
            } else {
                alert('Empate\nEm breve o game será mais dinamico e cartas poderão ser adquiridas.');
                window.open(main.appRoot + '/MenuPrincipal', '_self');
            }
        }

    };

    $scope.animacaoCard = function (historico, i) {
        var $tabuleiro = $('#tabuleiro');
        var $bloco = $tabuleiro.find('ul').eq(historico[i].PosX).find('li').eq(historico[i].PosY);
        console.log(i)

        if (historico[i].NewPlayer == 0) return;

        $bloco.rotate3Di(360, 700);
        $bloco.find('.carta').removeClass('p1 p2');
        $bloco.find('.carta').addClass('p' + historico[i].NewPlayer);

        console.log('NOME É: ',$scope.arenaSituacao[historico[i].PosX][historico[i].PosY].Card.Nome);        
    };

    $scope.atualizarHistoricoPassos = function (historico, p, callback) {
        callback = typeof (callback) != 'undefined' ? callback : function () { };

        // [{PosX: 2, PosY: 1, ExecutorPosX: 3, ExecutorPosY: 2, NewPlayer: 1}]
        var $tabuleiro = $('#tabuleiro');
        var totalTime = 0;

        for (var i in historico) {
            totalTime = 1000 * i;
            
            loopTimeOut.push(
                setTimeout(
                    $scope.animacaoCard,
                    totalTime,
                    historico, i
                )
            );
            
        }

        setTimeout(callback, totalTime + 1000);
    };

    $scope.atualizarTabuleiro = function () {
        
        $scope.atualizarHistoricoPassos($scope.ListaHistoricoPassosP1, '1', function () {
            var $tabuleiro = $('#tabuleiro');

            console.log($scope.arenaSituacao);
            // apresenta a carta adversária
            for (var i in $scope.arenaSituacao) {
                for (var j in $scope.arenaSituacao[i]) {
                    var $bloco = $tabuleiro.find('ul').eq(i).find('li').eq(j);

                    if ($scope.arenaSituacao[i][j].Usado && $bloco.is(':empty')) {
                        main.newCard($bloco, $scope.arenaSituacao[i][j].Card, function (c) {
                            c.addClass('handCard');
                            c.addClass('stageCard');
                            c.css('opacity', '1');
                        });

                        // se eu sempre sou o p1, então será obvio receber do servidor p2 como novo
                        $bloco.find('.carta').addClass('p2');

                        /*
                        if ($scope.arenaSituacao[i][j].Player == 1) {
                            $bloco.find('.carta').addClass('p1');
                        } else {
                            $bloco.find('.carta').addClass('p2');
                        }*/
                    }
                }
            }

            setTimeout(function () {

                $scope.atualizarHistoricoPassos($scope.ListaHistoricoPassosP2, '2', function () {
                    $scope.pontuacaoAtualizar()
                });
                
            }, 1000);
            
        });

    };

    _self.posicionarHand = function () {
        var $hand = $('#deckHand');
        var o = $(window).outerWidth(true) / 2;
        $hand.css('left', $hand.outerWidth(true) - o);
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

        var arrCards = JSON.parse($('#faseListaCartas').val());
        $scope.listaCartas = arrCards;

        for (var i = 0; i < arrCards.length; i++)
            main.newCard($('#deckHand ul li').eq(i), arrCards[i], function (c) {
                c.addClass('handCard');
            });


        $('#deckHand ul li .handCard').draggable({
            revert: "invalid", // when not dropped, the item will revert back to its initial position
            //containment: "document",
            //helper: "clone",
            cursor: "move"
        });

        $('ul.tab-coluna li.tab-linha').droppable({
            greedy: true,
            accept: '#deckHand ul li .handCard, .card.stageCard',
            hoverClass: "ui-state-hover",
            activeClass: "ui-state-default",
            drop: function (event, ui) {
                var $alvo = $(this);
                var $item = ui.draggable;
                var $li = $item.parents('li');
                var cartaNumero = $li.find('.carta').attr('data-card-number');
                var index = -1;

                for (var i = 0; i < $scope.listaCartas.length; i++) {
                    if ($scope.listaCartas[i].Numero == cartaNumero) {
                        $scope.listaCartas.splice(i, 1);
                    }
                }

                var posX = $alvo.parents('ul').index();
                var posY = $alvo.index();

                $alvo.droppable('disable');
                $item.draggable('disable');
                $item.unbind();

                $item.css('left', '0');
                $item.css('top', '0');
                $item.css('opacity', '0');
                
                $alvo.addClass('usado');
                $('ul.tab-coluna li.tab-linha').removeClass('ui-state-default');

                $item.appendTo($alvo).fadeIn(function () {
                    $item.addClass('stageCard');
                    $item.css('opacity', '1');

                    $li.removeClass('has-card');
                    $li.empty();
                });
                
                $scope.turnoMudar(cartaNumero, posX, posY, function (resp) {
                    $scope.arenaConfig = resp.ArenaConfig;
                    $scope.arenaSituacao = resp.ArenaSituacao;
                    $scope.ListaHistoricoPassosP1 = resp.ListaHistoricoPassosP1;
                    $scope.ListaHistoricoPassosP2 = resp.ListaHistoricoPassosP2;

                    $scope.atualizarTabuleiro();
                });

            }
        });

        /* configura a entrada corretamente */
        entrarArena(function (resp) {
            $scope.musica = resp.Musica;
            $scope.arenaConfig = resp.ArenaConfig;
            $scope.arenaSituacao = resp.ArenaSituacao;
            $scope.arena = [resp.arenasituacaoY1, resp.arenasituacaoY2, resp.arenasituacaoY3, resp.arenasituacaoY4];

            $scope.pontuacaoAtualizar();
            console.log($scope.arenaConfig.InicioTurno);

            if ($scope.arenaConfig.InicioTurno == 2) {
                console.log('segundo player começou');
                $scope.atualizarTabuleiro();
            }
        });


        $('#desistirBtn').click(function () {
            main.modal.abrir('Desistir',
                'Você está abandonando o seu direito de vitória.'
                + '<br />'
                + 'Quer desistir da partida?',
                function () {
                    window.open(main.appRoot + '/MenuPrincipal', '_self');
                }
            );
        });

        $('#minimizar').click(function () {
            var $hand = $('#deckHand');
            if ($hand.hasClass('minificado')) {
                $hand.removeClass('minificado');
            } else {
                $hand.addClass('minificado');
            }
        });

    };



    $(function () {
        construct();
    });
});


