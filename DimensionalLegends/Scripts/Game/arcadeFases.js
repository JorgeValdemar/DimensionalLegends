mainApp.controller('ArcadeFases', function ($scope, main) {
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

    var sky = function (bclass) {
        var sphere = bclass.addSphere('blue-sky', 400);
        sphere.material = bclass.addMaterial('sky1', bclass.DLTexturesURL.SkyTower1, bclass.textureType.ambient);
        return sphere;
    };

    var paredesPrincipais = function (bclass, material) {
        // parede 1 - frente
        var parede = bclass.addPlane('parede-1', 360, { x: 0, y: 10, z: 135 });
        // parede 2 - direita
        var parede2 = bclass.addPlane('parede-2', 360, { x: 135, y: 10, z: 0 }, bclass.AnguloFacil.DIREITA);
        // parede 3 - esquerda
        var parede3 = bclass.addPlane('parede-3', 360, { x: -135, y: 10, z: 0 }, bclass.AnguloFacil.ESQUERDA);
        // parede 4 - tras
        var parede4 = bclass.addPlane('parede-4', 360, { x: 0, y: 10, z: -135 }, bclass.AnguloFacil.INVERSO);

        parede.material = material;
        parede2.material = material;
        parede3.material = material;
        parede4.material = material;

        return { frente: parede, direita: parede2, esquerda: parede3, tras: parede4 };
    };

    var estagio = {
        skyTower: function (bclass) {
            var alturaParedes = 160;
            bclass.addCamera();
            bclass.addLight();

            //sky(bclass);

            // materiais
            var materialPisoT1 = bclass.addMaterial('piso-padrao-t1', bclass.DLTexturesURL.GroundTower1, bclass.textureType.diffuse,
                { uScale: 20.0, vScale: 20.0 }
            );
            var materialTetoT1 = bclass.addMaterial('teto-padrao-t1', bclass.DLTexturesURL.Teto1, bclass.textureType.diffuse,
                { uScale: 14.0, vScale: 14.0 }
            );
            var materialPlaneT1 = bclass.addMaterial('plane-padrao-t1', bclass.DLTexturesURL.Plane1, bclass.textureType.diffuse,
                { uScale: 10.0, vScale: 10.0 }
            );
            var materialPlaneP1 = bclass.addMaterial('plane-parede-t1', bclass.DLTexturesURL.Plane1, bclass.textureType.diffuse,
                { uScale: 24.0, vScale: 10.0 }
            );
            var materialIceP1 = bclass.addMaterial('ice-t1', bclass.DLTexturesURL.Ice1, bclass.textureType.emissive,
                { uScale: 0.8, vScale: 0.8 }
            );

            var materialF1 = bclass.addMaterial('a-f1', bclass.DLTexturesURL.GroundTower1, bclass.textureType.emissive,
                { uScale: 0.8, vScale: 0.8 }
            );

            var materialA1 = bclass.addMaterial('fase-A1', bclass.DLTexturesURL.fase[1].A, bclass.textureType.emissive,
                { uScale: 0.8, vScale: 0.8 }
            );

            paredesPrincipais(bclass, materialPlaneT1);

            // pilar principal
            var cylinder = bclass.addCylinder('centro', 80, 40, 40, 12, { x: 0, y: 40, z: 0 });
            cylinder.material = bclass.addMaterial('pillar-centro', bclass.DLTexturesURL.Pillar1, bclass.textureType.ambient,
                { uOffset: 1, vOffset: 1, uScale: 5.0, vScale: 5.0 }
            );
            cylinder.parent = groundF1;

            // chão 1
            var groundF1 = bclass.addGround('f1', 280, 280);
            groundF1.material = materialPisoT1;

            // teto 1
            var tetoF1 = bclass.addGround('f2', 280, 280, { x: 0, y: 24, z: 0 });
            tetoF1.material = materialTetoT1;

            // paredes labrinto
            var c1 = bclass.addBox('c1', 2.8, { x: -100.3, y: 1, z: 17.1 });
            c1.material = materialPlaneP1;
            c1.scaling = new BABYLON.Vector3(0.1, alturaParedes, 75);

            // parede do corredor da entrada
            var c2 = bclass.addBox('c2', 2.8, { x: -5, y: 1, z: -160 });
            c2.material = materialPlaneP1;
            c2.scaling = new BABYLON.Vector3(0.1, alturaParedes, 40);
            
            // parede do corredor da entrada
            var c3 = bclass.addBox('c3', 2.8, { x: 5, y: 1, z: -160 });
            c3.material = materialPlaneP1;
            c3.scaling = new BABYLON.Vector3(0.1, alturaParedes, 40);

            // parede esquerda do corredor da rampa
            var c4 = bclass.addBox('c4', 2.8, { x: 25, y: 1, z: 122 });
            c4.material = materialPlaneT1;
            c4.scaling = new BABYLON.Vector3(90, alturaParedes, 0.1);

            // parede da entrada
            var c5 = bclass.addBox('c5', 2.8, { x: 88, y: 1, z: -105 });
            c5.material = materialPlaneT1;
            c5.scaling = new BABYLON.Vector3(60, alturaParedes, 0.1);

            // parede da entrada
            var c6 = bclass.addBox('c6', 2.8, { x: -88, y: 1, z: -105 });
            c6.material = materialPlaneT1;
            c6.scaling = new BABYLON.Vector3(60, alturaParedes, 0.1);

            // parede que leva ao primiro corredor
            var c7 = bclass.addBox('c1', 2.8, { x: -74, y: 1, z: -45 });
            c7.material = materialPlaneP1;
//            c7.scaling = new BABYLON.Vector3(0.1, alturaParedes, 45);
            c7.scaling = new BABYLON.Vector3(0.1, alturaParedes, 150);

            bclass.addPedestalA1('1', materialIceP1, 0, -35, function (pedestal, pedestalBox) {
                pedestal.material = materialF1;
                pedestalBox.material = materialA1;
                bclass.rotate(pedestalBox);
                bclass.clickCallback(pedestalBox, function () {
                    console.log($scope.listaFases);
                    
                    var txtCartas = '';
                    for(var i in $scope.listaFases[0].ListaDrop) {
                        txtCartas += 
                            ' => Carta nº:' 
                            + $scope.listaFases[0].ListaDrop[i].Numero 
                            + ' - '
                            + $scope.listaFases[0].ListaDrop[i].Nome
                            + ' | Rank: ' + $scope.listaFases[0].ListaDrop[i].Rank
                            + '<br />';
                    }

                    main.modal.abrir('Fase ' + $scope.listaFases[0].FaseNome,
                        'Lider: ' + $scope.listaFases[0].ArcadeLiderStatus.Nick
                        + '<br />'
                        + 'Drop: <br />' + txtCartas
                    );
                    dicas.fechar();
                });

                bclass.overCallback(pedestalBox, function () {
                    dicas.abrir('Fase: A1<br />Dificuldade: Fácil<br /><br />Clique para saber mais.');
                });
                bclass.outCallback(pedestalBox, dicas.fechar);
            });

            bclass.addPedestalA1('2', materialIceP1, 35, -15);
            bclass.addPedestalA1('3', materialIceP1, -35, -15);
            bclass.addPedestalA1('4', materialIceP1, 35, 15);
            bclass.addPedestalA1('5', materialIceP1, -35, 15);
            bclass.addPedestalA1('6', materialIceP1, 35, 0);
            bclass.addPedestalA1('7', materialIceP1, -35, 0);
            bclass.addPedestalA1('8', materialIceP1, 0, 35);
            bclass.addPedestalA1('9', materialIceP1, 0, 35);
            

            // FAZER RAMPAS PARA SUBIR ANDAR
            var rampaF1F2 = bclass.addGround('r1', 20, 172);
            rampaF1F2.material = materialPisoT1;
            rampaF1F2.rotationQuaternion = BABYLON.Quaternion.RotationYawPitchRoll(Math.PI / 2, -0.28, 0);
            rampaF1F2.position = new BABYLON.Vector3(-50, 0, 136.4);
        }
    };

    var construct = function () {

        dicas = new main.dicas();

        $scope.lerFases(function (resp) {
            $scope.$apply(function () {
                $scope.listaFases = resp.ListaFases;
            });
        });

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

        var estagio3D = new BDLF('renderCanvasArcadeFases', estagio.skyTower);

    };



    $(function () {
        construct();
    });
});


