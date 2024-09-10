$(document).ready(function () {

    sleep(1000);

    $("h2").each(function () {
        if ($.trim($(this).html()) == "")
            $(this).remove();
    });

    var cont = 0;
    $('a[id*=tituloList]').each(function () {
        $(this).attr('data-target', '#collapse-' + cont);
        $(this).attr('aria-controls', 'collapse-' + cont);
        cont++;
    });

    var cont2 = 0;
    $('div[id*=collapse-]').each(function () {
        var id = $(this).attr('id');
        $(this).attr('id', id + cont2);
        $(this).attr('aria-labelledby', 'heading-' + cont2);
        cont2++;
    });

    var cont3 = 0;
    $('div[id*=heading-]').each(function () {
        var id = $(this).attr('id');
        $(this).attr('id', id + cont3);
        cont3++;
    });


    $('a[class*=idLink]').each(function () {
        if ($.trim($(this).html()) == "") {
            $(this).parent().parent().parent().remove();
        }
    });

  /*  $('span[class*=pegaData]').each(function () {
        ano = $(this).text().split('/')[2];
        $(this).text(ano);
    });
*/


    $('div[class*=loader]').attr('style', 'display:none;');
    $('div[id*=totalContent]').attr('style', 'display:block;');
});

function efetuarFiltroPorAno(ano) {

    var idCanal = $('input[id*=hdCanal]').val();
    var linguagem = $('input[class*=hidLinguagem]').val();
    $('div[id*=totalContent]').attr('style', 'display:none;');
    $('div[class*=loader]').attr('style', 'display:block;');

    $.ajax({
        type: "POST",
        url: "filtroListGroup.asmx/RefreshContent",
        data: JSON.stringify({
            "ano": ano,
            "idCanal": idCanal,
            "linguagem": linguagem
        }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: onSuccess,
        error: function (result) {
        }
    });

}

function limpaFiltroPorAno() {
    if (window.location.href.indexOf("ListGroup2.aspx") > 0) {
        window.location = "ListGroup2.aspx?idCanal=" + getIdCanal();
    }
    else {
        window.location = "ListGroup.aspx?idCanal=" + getIdCanal();
    }

}

function getIdCanal() {
    var strHref = window.location.href;
    var strQueryString = strHref.substr(strHref.indexOf("=") + 1);
    var aQueryString = strQueryString.split("&");
    return aQueryString[0];
}

function sleep(delay) {
    var start = new Date().getTime();
    while (new Date().getTime() < start + delay);
}

function onError(result) {
    alert(result._message);
}


function onSuccess(result) {
    //alert(result);
    $('div#accordion').empty();


    var i;
    var c;
    var text = "";
    var conteudos = "";
    for (i = 0; i < result.d.length; i++) {
        if (!(typeof result.d[i].Titulo === "undefined")) {
            var corpoHtmlBase = '<div class="card"> <div class="card-header" id="heading-"> <h4 class="mb-0"> <a href="#" id="tituloList" class="btn collapsed  btn-secondary idLink" runat="server" data-toggle="collapse" data-target="#collapse-" aria-expanded="false" aria-controls="collapse-"> #TituloCanal </a> </h4> </div> <div id="collapse-" class="collapse" aria-labelledby="heading-" data-parent="#accordion"> <div class="card-body"> <ul class="list-group">#RecebeConteudos </ul>  </div> </div> </div>'
            corpoHtmlBase = corpoHtmlBase.replaceAll('#TituloCanal', result.d[i].Titulo);
            var corpoConteudos = "";

            for (c = 0; c < result.d[i].Materia.length; c++) {
                if (!(typeof result.d[i].Materia[c].Titulo === "undefined")) {
                    corpoConteudos = '<li class="list-group-item d-flex justify-content-between align-items-center"> <div> <span class="float-left"> #trocaData </span> <a href="#trocaLink">#trocaTitulo</a> </div> </li>';
                    corpoConteudos = corpoConteudos.replaceAll('#trocaData', result.d[i].Materia[c].Data);
                    corpoConteudos = corpoConteudos.replaceAll('#trocaTitulo', result.d[i].Materia[c].Titulo);
                    corpoConteudos = corpoConteudos.replaceAll('#trocaLink', result.d[i].Materia[c].Link);
                    conteudos += corpoConteudos;
                }
            }
            corpoHtmlBase = corpoHtmlBase.replaceAll('#RecebeConteudos', conteudos);
            conteudos = "";



        }
        text += corpoHtmlBase;
    }

    $('div#accordion').append(text);


    $('a').each(function () {
        var link = $(this);
        var urlLink = $(this).attr('href');
        if (typeof link.attr('href') != 'undefined') {
            if ((link.attr('href').indexOf('/Download/') > -1) || (link.attr('href').indexOf('download.aspx') > -1) || (link.attr('href').indexOf('Download.aspx') > -1)) {
                var descricao = link.text().trim();
                link.attr('target', '_blank');

                if (descricao == '') {
                    descricao = urlLink.split('download.aspx?')[1];
                }

                var url = window.location.href;

                if ((url.toLowerCase().indexOf('/central') > -1) || (url.toLowerCase().indexOf('/center') > -1)) {
                    var ano = $(this).parents('div[id*=divResultados]').attr('ano');
                    if (ano != undefined) {
                        var idLink = $(this).attr('id');
                        descricao = idLink.split('_')[4];

                        if ($(".hidLinguagem").val() == "ptg") {
                            link.attr("onClick", "gtag('event', 'file_download', {'link_text' : '" + descricao + "_PT_" + ano + "','file_name' : '" + descricao + "_PT_" + ano + "'});");

                        } else {
                            link.attr("onClick", "gtag('event', 'file_download', {'link_text': '" + descricao + "_EN_" + ano + "','file_name' : '" + descricao + "_EN_" + ano + "'});");
                        }
                    }


                } else {
                    link.attr("onClick", "gtag('event', 'file_download', {'link_text' : '" + descricao + "','file_name' : '" + descricao + "'});");
                }
            }
        }
    });

    $("h2").each(function () {
        if ($.trim($(this).html()) == "")
            $(this).remove();
    });

    var cont = 0;
    $('a[id*=tituloList]').each(function () {
        $(this).attr('data-target', '#collapse-' + cont);
        $(this).attr('aria-controls', 'collapse-' + cont);
        cont++;
    });

    var cont2 = 0;
    $('div[id*=collapse-]').each(function () {
        var id = $(this).attr('id');
        $(this).attr('id', id + cont2);
        $(this).attr('aria-labelledby', 'heading-' + cont2);
        cont2++;
    });

    var cont3 = 0;
    $('div[id*=heading-]').each(function () {
        var id = $(this).attr('id');
        $(this).attr('id', id + cont3);
        cont3++;
    });


    $('a[class*=idLink]').each(function () {
        if ($.trim($(this).html()) == "") {
            $(this).parent().parent().parent().remove();
        }
    });

    $('div[class*=loader]').attr('style', 'display:none;');
    $('div[id*=totalContent]').attr('style', 'display:block;');

}



