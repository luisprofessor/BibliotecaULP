function uploadFiles(inputId)
{

    var input = document.getElementById('files');
    var nombre = document.getElementById('Nombre').value;
    var usuario = document.getElementById('UsuarioId').value;
    var tipoId = document.getElementById('TipoId').value;
    var temaId = document.getElementById('TemaId').value;
    var materiaId = document.getElementById('MateriaId').value;
    var fecha = document.getElementById('fecha').value;
    var bar = document.getElementById('determinate');
  
    var files = input.files;
    var formdata = new FormData();
    var bandera = true;
    
   



    for (var i = 0; i != files.length; i++) {
        formdata.append("Archivo", files[i]);
    }

    formdata.append("DocumentoId", 0)
    formdata.append("Nombre", nombre)
    formdata.append("UsuarioId", usuario)
    formdata.append("TipoId", tipoId)
    formdata.append("TemaId", temaId)
    formdata.append("MateriaId", materiaId)
    formdata.append("FechaSubida", fecha)



   startUpdatingProgressIndicator();

    $.ajax(
        {
            url: "/Documento/Create",
            data: formdata,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (data) {
              
                bar.style.width = "100%"

               
            }
        }
    );


    var intervalId;

    function startUpdatingProgressIndicator() {
       

        intervalId = setInterval(
            function ()
            {

                $.post(
                    "/documento/progress",
                    function (progress)
                    {
                        console.log(progress);

                       bar.style.width = progress + "%";
                        if (progress == 100) {
                            bar.style.width = "100%"

                        }
                        if (bar.style.width == "100%") {

                            stopUpdatingProgressIndicator();

                            bar.style.width = "100%"

                            if (bandera) {
                                alert("Files Uploaded!");
                                bandera = false;

                            }



                        }
                        
                    }
                );
            },
            25
        );
    }

    function stopUpdatingProgressIndicator() {

        clearInterval(intervalId);
    }





}

