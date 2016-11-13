mainApp.controller('Tutorial', function ($scope, main) {
    var _self = this;

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

    };



    $(function () {
        construct();
    });
});


