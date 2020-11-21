
function dropMaterias()
{
    var carrera = document.getElementById('CarreraId').value;

    var materia = document.getElementById('MateriaId');

    var dropAño = document.getElementById('Año');

    dropAño.selectedIndex = 1;

    $(document).ready(function () {
        $('select').formSelect();
    });

    var año = document.getElementById('Año').value;

     $.ajax(
        {
            url: "/Materia/GetMateriasId",
            data: {
                idCarrera: carrera,
                año: año
            },
            type: "POST",
            success: function (data) {
                var parseJson = JSON.parse(JSON.stringify(data));

                for (var i = materia.childElementCount; i > 0; i--) {
                    materia.remove(i);

                    $(document).ready(function () {
                        $('select').formSelect();
                    });
                }
                for (var i in parseJson) {
                    var option = document.createElement('option');

                    option.value = parseJson[i].materiaId;

                    option.text = parseJson[i].nombre;

                    materia.add(option);

                    $(document).ready(function () {
                        $('select').formSelect();
                    });

                    
                }


            }


        });
}


function dropAño() {

    var carrera = document.getElementById('CarreraId').value;

    var materia = document.getElementById('MateriaId');

    var año = document.getElementById('Año').value;

    $.ajax(
        {
            url: "/Materia/GetMateriasId",
            data: {
                idCarrera: carrera,
                año: año
            },
            type: "POST",
            success: function (data) {
                var parseJson = JSON.parse(JSON.stringify(data));

                for (var i = materia.childElementCount; i > 0; i--) {
                    materia.remove(i);

                    $(document).ready(function () {
                        $('select').formSelect();
                    });
                }
                for (var i in parseJson) {
                    var option = document.createElement('option');

                    option.value = parseJson[i].materiaId;

                    option.text = parseJson[i].nombre;

                    materia.add(option);

                    $(document).ready(function () {
                        $('select').formSelect();
                    });


                }


            }


        });

}






function dropSegundo()
{
    var materia = document.getElementById('MateriaId').value;

  //  var año = document.getElementById('Año').value;

    var temaid = document.getElementById('TemaId');

    $.ajax(
        {
            url: "/Tema/GetTemasId",
            data: {
                idMateria : materia
            },
            type: "POST",
            success: function (data) {
                var parseJson = JSON.parse(JSON.stringify(data));

                for (var i = temaid.childElementCount; i > 0; i--) {
                    temaid.remove(i);

                    $(document).ready(function () {
                        $('select').formSelect();
                    });
                }
                for (var i in parseJson) {
                    var option = document.createElement('option');

                    option.value = parseJson[i].temaId;

                    option.text = parseJson[i].nombre;

                    temaid.add(option);

                    $(document).ready(function () {
                        $('select').formSelect();
                    });


                }


            }


        });
}