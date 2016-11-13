/*
    Babylon Dimensional Legends Functions
*/
function BDLF(canvasId, callbackScope) {
    var scene;
    _self = this;

    _self.canvas = document.getElementById(canvasId);
    _self.engine = new BABYLON.Engine(_self.canvas, true);
    
    _self.DLTexturesURL = {
        GroundTower1: '/Content/textures/stone-1.jpg',
        SkyTower1: '/Content/textures/sky-2.jpg',
        Pillar1: '/Content/textures/stone-2-b.jpg',
        Teto1: '/Content/textures/teto-1.jpg',
        Plane1: '/Content/textures/plane-1.jpg',
        Ice1: '/Content/textures/ice-1.jpg',
        fase: [{}, {A: '/Imagens/cards/cartas/35.jpg'}]
    };

    _self.AnguloFacil = {
        FRENTE: BABYLON.Quaternion.RotationYawPitchRoll(0, 0, 0),
        DIREITA: BABYLON.Quaternion.RotationYawPitchRoll(Math.PI / 2, 0, 0),
        ESQUERDA: BABYLON.Quaternion.RotationYawPitchRoll((Math.PI / 2) * -1, 0, 0),
        INVERSO: BABYLON.Quaternion.RotationYawPitchRoll(Math.PI, 0, 0)
    };

    _self.textureType = {
        emissive: 'emissiveTexture',
        ambient: 'ambientTexture',
        diffuse: 'diffuseTexture'
    };

    _self.subDivs = {
        grounds: 2,
        spheres: 100,
        cylinder: 2
    };

    _self.addEmptyScene = function () {
        var s = new BABYLON.Scene(_self.engine);
        // (G force like, on Y-axis)
        s.gravity = new BABYLON.Vector3(0, -0.9, 0);
        s.collisionsEnabled = true;

        return s;
    };

    _self.addCamera = function () {
        var camera = new BABYLON.FreeCamera("FreeCamera", new BABYLON.Vector3(0, 1, -120), scene);
        camera.attachControl(_self.canvas, true);
        // Apply collisions and gravity to the active camera
        camera.checkCollisions = true;
        camera.applyGravity = true;
        // Set the ellipsoid around the camera (e.g. your player's size)
        camera.ellipsoid = new BABYLON.Vector3(0.8, 1.4, 0.8);
    };

    _self.addLight = function (name) {
        // name, (possivel parametro) "objeto_de_alvo"
        var light = new BABYLON.HemisphericLight("light-" + name, new BABYLON.Vector3(1, 1, 1), scene);
        light.intensity = 0.2;
    };

    _self.addGround = function (name, width, depth, positions) {
        positions = typeof (positions) !== undefined ? positions : false;
        // Params: name, width, depth, subdivs, scene
        var ground = new BABYLON.Mesh.CreateGround("ground-" + name, width, depth, _self.subDivs.grounds, scene);
        ground.checkCollisions = true;
        
        if (positions !== false) {
            for (var i in positions) {
                ground.position[i] = positions[i];
            }
        };

        return ground;
    };

    _self.addPlane = function (name, size, positions, angulo, options) {
        positions = typeof (positions) !== undefined ? positions : false;
        angulo = typeof (angulo) !== undefined ? angulo : _self.AnguloFacil.FRENTE;
        options = typeof (options) !== undefined ? options : false;

        // Params: name, size, scene
        var plane = new BABYLON.Mesh.CreatePlane("plane-" + name, size, scene);
        plane.checkCollisions = true;
        plane.rotationQuaternion = angulo;
        
        if (positions !== false) {
            for (var i in positions) {
                plane.position[i] = positions[i];
            }
        };

        if (options !== false) {
            for (var i in options) {
                plane[i] = options[i];
            }
        };

        return plane;
    };

    _self.addBox = function (name, size, positions) {
        positions = typeof (positions) !== undefined ? positions : false;
        // Params: name, subdivs, size, scene
        var box = BABYLON.Mesh.CreateBox("box-" + name, size, scene);
        box.position = new BABYLON.Vector3(0, 0, 0);
        box.checkCollisions = true;
        
        if (positions !== false) {
            for (var i in positions) {
                box.position[i] = positions[i];
            }
        };

        return box;
    };

    _self.addSphere = function (name, size, positions) {
        positions = typeof (positions) !== undefined ? positions : false;
        // Params: name, subdivs, size, scene
        var sphere = BABYLON.Mesh.CreateSphere("sphere-" + name, _self.subDivs.spheres, size, scene);
        sphere.position = new BABYLON.Vector3(0, 0, 0);
        sphere.rotate(new BABYLON.Vector3(1.0, 0.0, 0.0), Math.PI / 2.0, BABYLON.Space.Local);

        if (positions !== false) {
            for (var i in positions) {
                sphere.position[i] = positions[i];
            }
        };

        return sphere;
    };

    _self.addCylinder = function (name, height, diameterTop, diameterBottom, tessellation, positions, updatable) {
        positions = typeof (positions) !== undefined ? positions : false;
        updatable = typeof (updatable) !== undefined ? updatable : false;

        // name, height, diameterTop, diameterBottom, tessellation, subdivisions, scene, updatable
        var cylinder = BABYLON.Mesh.CreateCylinder("cylinder-" + name, height, diameterTop, diameterBottom, tessellation, _self.subDivs.cylinder, scene);
        cylinder.position = new BABYLON.Vector3(0, 0, 0);
        cylinder.checkCollisions = true;

        if (positions !== false) {
            for (var i in positions) {
                cylinder.position[i] = positions[i];
            }
        };

        return cylinder;
    };

    _self.addPedestalA1 = function (numeroNome, material, x, z, callback) {
        var spot = new BABYLON.SpotLight("spot" + numeroNome, 10, new BABYLON.Vector3(0, -1, 0), 4, 1, scene);
//        spot.diffuse = new BABYLON.Color3(1, 1, 1);
//        spot.specular = new BABYLON.Color3(0, 0, 0);
        spot.emissive = new BABYLON.Color3(0, 0, 0);
        spot.intensity = 0.25;

        // pedestal
        var pedestal1 = _self.addCylinder('ped' + numeroNome, 1.8, 5, 5, 30, { x: x, y: 1, z: z });
        var pedestal1box = _self.addBox('ped' + numeroNome + 'b', 2, { x: x, y: 3.6, z: z });

        pedestal1.material = material;
        pedestal1box.material = material;
        pedestal1box.actionManager = new BABYLON.ActionManager(scene);

        if (typeof (callback) != 'undefined') {
            callback(pedestal1, pedestal1box);
        }
        spot.position = pedestal1box.position;
    };

    _self.addMaterial = function (materialName, textureUrl, textureTypeParam, textureParams) {
        textureParams = typeof (textureParams) !== undefined ? textureParams : false;

        var material = new BABYLON.StandardMaterial("material-" + materialName, scene);
        material.backFaceCulling = false;
        material[textureTypeParam] = new BABYLON.Texture(
            textureUrl,
            scene
        );

        if (textureParams !== false) {
            for (var i in textureParams) {
                material[textureTypeParam][i] = textureParams[i];
            }
        };

        return material;
    };

    _self.clickCallback = function (mesh, callback) {
//        mesh.actionManager.registerAction(new BABYLON.InterpolateValueAction(BABYLON.ActionManager.OnPickTrigger, mesh, "visibility", 0.2, 1000))
//            .then(new BABYLON.InterpolateValueAction(BABYLON.ActionManager.OnPickTrigger, mesh, "visibility", 1, 1000));

        mesh.actionManager.registerAction(new BABYLON.ExecuteCodeAction(BABYLON.ActionManager.OnPickTrigger, callback));
    }

    _self.overCallback = function (mesh, callback) {
        mesh.actionManager.registerAction(new BABYLON.ExecuteCodeAction(BABYLON.ActionManager.OnPointerOverTrigger, callback));
    }

    _self.outCallback = function (mesh, callback) {
        mesh.actionManager.registerAction(new BABYLON.ExecuteCodeAction(BABYLON.ActionManager.OnPointerOutTrigger, callback));
    }

    _self.rotate = function (mesh) {
        scene.actionManager.registerAction(new BABYLON.IncrementValueAction(BABYLON.ActionManager.OnEveryFrameTrigger, mesh, "rotation.y", 0.01));
        scene.actionManager.registerAction(new BABYLON.IncrementValueAction(BABYLON.ActionManager.OnEveryFrameTrigger, mesh, "rotation.x", 0.01));
        scene.actionManager.registerAction(new BABYLON.IncrementValueAction(BABYLON.ActionManager.OnEveryFrameTrigger, mesh, "rotation.z", 0.01));
    }

    _self.render = function () {
        //callback = typeof (callback) !== undefined ? callback : false;
        _self.engine.runRenderLoop(function () {
            scene.render();
        });

        // Resize
        window.addEventListener("resize", function () {
            _self.engine.resize();
        });
    };

    _self.construct = function () {
        scene = _self.addEmptyScene();
        scene.actionManager = new BABYLON.ActionManager(scene);
        callbackScope(_self);
        _self.render();
    };

    _self.construct();

}