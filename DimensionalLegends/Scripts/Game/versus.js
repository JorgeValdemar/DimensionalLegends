mainApp.controller('Versus', function ($scope, main) {
    var _self = this;
    // We return this object to anything injecting our service
    //var Service = {};
    // Keep all pending requests here until they get responses
    //var callbacks = {};
    // Create a unique callback ID to map requests to responses
    //var currentCallbackId = 0;
    var IdTemp; // ID específico aceito apenas pelo socket
    
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


    function doSend(message) {
        writeToScreen("SENT: " + message);
        ws.send(message);
    }

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

        $('.voltar').click(function () {
            window.open(main.appRoot + '/MenuPrincipal', '_self');
        });

        var ws = new WebSocket(main.wsRoot + "/Chat.ashx");
        ws.onopen = function (evt) {
            console.log("Socket has been opened!");
            console.log(evt);
        };

        ws.onmessage = function (evt) {
            console.log(evt);
            //listener(JSON.parse(message.data));
        };

        ws.onclose = function (evt) {
            console.log(evt);
            alert(evt);
            //onClose(evt)
        };

        ws.onerror = function (evt) {
            console.log(evt);
            //onError(evt)
        };

        //ws.close();
        

    };



    $(function () {
        construct();
    });
});


