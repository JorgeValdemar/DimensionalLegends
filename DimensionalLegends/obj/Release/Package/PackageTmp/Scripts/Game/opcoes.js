mainApp.controller('Opcoes', function ($scope, main) {
    var _self = this;
    var bloqueio = false;

    $scope.listaAssistentes = [];

    $scope.lerAssistentes = function (callback) {

        $.ajax({
            type: "POST",
            url: "/aplicacao/Assistentes/Listar.ashx",
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

    $scope.atualizarAssistente = function () {
        if (bloqueio) return;
        bloqueio = true;
        var dados = {
            ChibiAssistente: main.opcoes.ChibiAssistente
        };

        $.ajax({
            type: "POST",
            url: "/aplicacao/Assistentes/Atualizar.ashx",
            data: "data=" + JSON.stringify(dados),
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

    $scope.selecionarAssistente = function (objThis) {
        var id = $(objThis).attr('id').replace('assistente', '');
        var nome = $(objThis).attr('data-descr');
        var imagem = $(objThis).attr('data-img');

        main.opcoes.ChibiAssistente = id;
        main.opcoes.Assistente.ChibiId = id;
        main.opcoes.Assistente.Nome = nome;
        main.opcoes.Assistente.Imagem = imagem;

        sessionStorage.opcoes = main.opcoes;

        $scope.$apply(function () {
            $scope.main.opcoes = main.opcoes;
        });
    };

    var construct = function () {

        $scope.main = main;

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

        if ($('#listaAssistentes').length > 0) {

            $scope.lerAssistentes(function (resp) {

                $scope.$apply(function () {
                    $scope.listaAssistentes = resp.ListaAssistentes;
                });

                main.dicas();

                $('ul#listaAssistentes li .assistente[data-existe="true"]').click(function () {
                    $scope.selecionarAssistente(this);
                });
            });

            $('#salvarAssistente').click(function () {
                $scope.atualizarAssistente();
            });

        }

    };

    $(function () {
        construct();
    });
});

