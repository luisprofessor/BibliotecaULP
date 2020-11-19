

function drop1() {

    var dropInstituto = document.getElementById('InstitutoId').value;
    
    var dropCarrera = document.getElementById('CarreraId');

    var dropAño = document.getElementById('Año');

    dropAño.selectedIndex = 1;

  

    $(document).ready(function () {
        $('select').formSelect();
    });

    $.ajax(
        {
            url: "/Carrera/GetCarrerasId",
            data: { id:dropInstituto},
            type: "POST",
            success: function (data) {


                for (var i = dropCarrera.childElementCount; i > 0  ; i--) {
                    dropCarrera.remove(i);

                    $(document).ready(function () {
                        $('select').formSelect();
                    });
                }
                var asd = JSON.parse(JSON.stringify(data));
                //alert(data[0].nombre);
                for (var i in asd) {
                    //var option = document.createElement('option');

                   /* option.innerHTML = data[i].nombre;

                    option.value = data[i].carreraId;

                    dropCarrera.appendChild(option);*/

                    // dropCarrera.add(new Option(data[i].nombre,data[i].carreraId));
                    dropCarrera.innerHTML = dropCarrera.innerHTML + 
                        '<option value="' + asd[i].carreraId + '">' + asd[i].nombre + '</option>';


                    $(document).ready(function () {
                        $('select').formSelect();
                    });
                }
                

            }
        });
}

function drop2() {

    var dropCarrera = document.getElementById('CarreraId').value;

    var dropAño = document.getElementById('Año').value;

    var dropMateria = document.getElementById('Materia');

    $.ajax(
        {
            url: "/Materia/GetMateriasId",
            data: {
                idCarrera: dropCarrera,
                 año : dropAño},
            type: "POST",
            success: function (data)
            {
                var parseJson = JSON.parse(JSON.stringify(data));

                for (var i = dropMateria.childElementCount; i > 0; i--) {
                    dropMateria.remove(i);

                    $(document).ready(function () {
                        $('select').formSelect();
                    });
                }
                for (var i in parseJson)
                {
                    var option = document.createElement('option');

                    option.value = parseJson[i].materiaId;

                    option.text = parseJson[i].nombre;

                    dropMateria.add(option);

                    $(document).ready(function () {
                        $('select').formSelect();
                    });

                    console.log(dropMateria);
                }

               
            }


        });
}

