mainApp.controller('ArcadeFases2d', function ($scope, main) {
    var _self = this;
    var bloqueio = false;
    var dicas = {};

    $scope.listaFases = [];

    $scope.lerFases = function (callback) {

        $.ajax({
            type: "POST",
            url: "/Aplicacao/Arcade/ListarFases.ashx",
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

    $scope.abrirFaseInfo = function (indice) {
        var txtCartas = '';
        for (var i in $scope.listaFases[indice].ListaDrop) {
            txtCartas +=
                ' => Carta nº:'
                + $scope.listaFases[indice].ListaDrop[i].Numero
                + ' - '
                + $scope.listaFases[indice].ListaDrop[i].Nome
                + ' | Rank: ' + $scope.listaFases[indice].ListaDrop[i].Rank
                + '<br />';
        }

        main.modal.abrir('Fase ' + $scope.listaFases[indice].FaseNome,
            'Lider: ' + $scope.listaFases[indice].ArcadeLiderStatus.Nick
            + '<br />'
            + 'Drop: <br />' + txtCartas,
            function () {
                // TODO: Um dia fazer isso de forma transparente, não permitir ninguém visualizar ID assim
                window.open(main.appRoot + '/Arcade/SelecaoCartas/' + $scope.listaFases[indice].Id, '_self');
            }
        );
    }

    $scope.refresh = function () {
        $('#deck .fase-info').bind("click", function () {
            var num = $(this).parents("li").index();
            $scope.abrirFaseInfo(num);
            //$(this).unbind();
        });

        main.dicas();
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

        $scope.lerFases(function (resp) {
            $scope.$apply(function () {
                $scope.listaFases = resp.ListaFases;
            });

            $scope.refresh();
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


