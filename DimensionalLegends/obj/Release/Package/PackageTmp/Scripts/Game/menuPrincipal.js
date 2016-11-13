mainApp.controller('MenuPrincipal', function ($scope, main) {
    var _self = this;
    var bloqueio = false;

    //window["window"]["open"]
    $scope.listaMenu = [
                //{ nome: "Multiplayer", evt: main.appRoot + '/Versus', descr: "Jogue contra o mundo todo e se torne um dos melhores jogadores!" },
                { nome: "Arcade", evt: main.appRoot + '/Arcade', descr: "Jogue contra a máquina para liberar novos desafios e cartas enquanto aumenta seu nível!" },
                //{ nome: "Shopping", evt: '', descr: "Compre, venda ou veja as cartas que estão a venda." },
                { nome: "Minhas Cartas", evt: main.appRoot + '/MinhasCartas', descr: "Monte seu deck com suas cartas disponíveis." },
                { nome: "Meu Perfil", evt: main.appRoot + '/Perfil/Index/', descr: "Ver o seu perfil de usuário." },
                //{ nome: "Ranking", evt: '', descr: "O ranking dos jogadores é mostrado aqui." },
	            { nome: "Como Jogar", evt: main.appRoot + '/Tutorial', descr: "Veja um tutorial." },
                //{ nome: "Sala de música", evt: '', descr: "Gerencie ou escute uma música do jogo!" },
                //{nome: "Medalhas", evt: '', descr: "Cada medalha é um novo desafio conquistado." },
	            { nome: "Opções", evt: main.appRoot + '/Opcoes', descr: "Deixe o jogo conforme suas preferências." },
	            { nome: "Sair", evt: main.appRoot + '/Login/Logout', descr: "Encerra sua sessão no jogo" }
	        ];

    var construct = function () {

        main.lerPlayer(function () {
            $scope.$apply(function () {
                $scope.main = main;
                $scope.listaMenu[3].evt = $scope.listaMenu[3].evt + $scope.main.user.Nick;
                console.log($scope.main.user);
            });

            main.dicas();
            main.evtTriggers('ul#menuPrincipal', 'click');
        });

        main.lerOpcoes(function () {
            $scope.$apply(function () {
                $scope.main = main;
            });
        });

    };

    $(function () {
        construct();
    });
});

