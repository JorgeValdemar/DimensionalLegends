mainApp.controller('Login', function ($scope, main) {
    var bloqueio = false;
    $scope.main = main;

    $scope.Logar = function (email, senha) {
        if (bloqueio) return;
        bloqueio = true;
        var dados = {
            Email: email,
            Senha: senha
        };

        $.ajax({
            type: "POST",
            url: "/aplicacao/login.ashx",
            data: "data=" + JSON.stringify(dados),
            success: function (resp) {
                bloqueio = false;
                resp = JSON.parse(resp);
                if (resp.Erro) {
                    main.modal.abrir('Erro ao logar', resp.ErroDescricao);
                } else {
                    if (typeof (resp.Login) != 'undefined' && resp.Login != null) {
                        if (typeof (resp.Login.PlayerStatus) != 'undefined' && resp.Login.PlayerStatus != null) {
                            window.open(main.appRoot + "/MenuPrincipal", "_self"); // Tela inicial
                        } else {
                            window.open(main.appRoot + "/Cadastro", "_self"); // Tela cadastro
                        }
                    } else {
                        main.modal.abrir('Erro ao logar', 'Login e senha não conferem');
                    };
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

    var configLogin = function () {

        $('#formLogin input').keypress(function (e) {
            if (e.which == 13) {
                $('#formLogin .btn').click();
            }
        });

        $('#formLogin .btn').click(function () {
            if ($(this).parents("#formLogin").parsley().validate()) {

                var email = $('#email').val();
                var senha = $('#senha').val();

                $scope.Logar(email, senha);

            };
        });

    };


    var construct = function () {
        configLogin();

        $('header').css('display', 'none');
    };

    construct();

});
