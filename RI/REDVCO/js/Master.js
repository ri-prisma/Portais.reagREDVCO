$(document).ready(function () {

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

    $('a[href$="Mailling"]').each(function () {
        $(this).attr("data-toggle", "modal");
        $(this).attr("data-target", "#alertasModal");
        $(this).attr('href', '');

    });

    // Busca
    $(".inputBusca").keypress(function(event) {
        event = event || window.event;

        if (event.keyCode == '13') {
            Buscar();

            event.preventDefault();
        }
    });

    $(".inputBuscaMobile").keypress(function(event) {
        event = event || window.event;

        if (event.keyCode == '13') {
            BuscarMobile();

            event.preventDefault();
        }
    });

    $(".inputOk").click(function() {
        Buscar();
        event.preventDefault();
    });

    $(".inputOkMobile").click(function() {
        BuscarMobile();
        event.preventDefault();
    });

    $('.btnX').click(function() {
        mostrarTextoSemFavoritos();
    });

    $('.btn_incluir').click(function() {
        mostrarTextoSemFavoritos();
    });

    $("li").each(function () {
        if ($.trim($(this).html()) == "")
            $(this).remove();
    });

    //remover marcadores alertas
    $('div[class*=checkbox-modal-form]').attr('style', 'display:none;');
    //marcar marcador português
    $('input[id*=controlMailingPort]').prop('checked', true);

    function accessApplyFont(size) {
        localStorage.setItem('access_font_size', size)
        var size_px = 16 + Number(size);
        $('body').css({ 'font-size': size_px });
    }

    var access_font_size = 0;

    if (localStorage.getItem('access_font_size')) {
        access_font_size = Number(localStorage.getItem('access_font_size'));
        accessApplyFont(access_font_size);
    }

    $('.icon-Fonte-maior').on('click', function (e) {
        e.preventDefault();
        if (access_font_size < 4) {
            access_font_size += 1;
            accessApplyFont(access_font_size);
        }
    });
    $('.icon-Fonte-menor').on('click', function (e) {
        e.preventDefault();
        if (access_font_size > 0) {
            access_font_size -= 1;
            accessApplyFont(access_font_size);
        }
    });


});

function Buscar() {
    var buscada = $(".inputBusca").val().replace(/"/g, "");
    window.location = "ListaBusca.aspx?busca=" + buscada;
}

function BuscarMobile() {
    var buscada = $(".inputBuscaMobile").val().replace(/"/g, "");
    window.location = "ListaBusca.aspx?busca=" + buscada;
}

function Trim(str) { return str.replace(/^\s+|\s+$/g, ""); }

function irParaTopo() { $('html, body').animate({ scrollTop: 0 }, 'slow'); }


//Alertas
function abreBoxMailingMenu() {
    $('#fadeModal').show();
    $('#alertasModal').addClass('in').attr('style', 'display:block');
}

function abreBoxMailing() {
    var nome = $("input[id*=alertanome]").val();
    var email = $("input[id*=alertaemail]").val();
    var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;

    if (filter.test(email) && Trim(nome) != "") {
        document.getElementById('nome_modal').value = nome;
        document.getElementById('email_modal').value = email;
        $('#fadeModal').show();
        $('#alertasModal').addClass('in').attr('style', 'display:block');
    }
    else if (Trim(nome) == "") {
        if ($(".hidLinguagem").val() == "ptg") {
            alert('Digite um nome!');
        }
        else {
            alert('Enter a name!');
        }

    }
    else if (!filter.test(email)) {
        if ($(".hidLinguagem").val() == "ptg") {
            alert('E-mail Inválido!');
        }
        else {
            alert('Invalid E-mail!');
        }
    }
}


function enviaAlerta() {
    Validar();
}

function Validar() {

    if ((document.getElementById('nome_modal').value == "") && ($(".hidLinguagem").val() == "ptg")) {
        alert("É necessário um nome.");
        return false;
    }
    else if (document.getElementById('nome_modal').value == "") {
        alert("Insert a name.");
        return false;
    }

    if ((document.getElementById('email_modal').value == "") && ($(".hidLinguagem").val() == "ptg")) {
        alert("É necessário um email.");
        return false;
    }
    else if (document.getElementById('email_modal').value == "") {
        alert("Insert a email.");
        return false;
    }
    if (document.getElementById('controlMailingPort').checked != true && document.getElementById('controlMailingIng').checked != true) {
        if ($(".hidLinguagem").val() == "ptg")
            alert("Selecione pelo menos um marcador.");
        else
            alert("Select at least one marker.");
        return false;
    }

    if ((document.getElementById("idCaptcha").value) == "0") {
        alert("Marque a caixa de seleção!");
        return false;
    }

    cadastraContato();
}

function fechaBoxAlerta() {
    $('#fadeModal').hide();
    $('div[class*=modal-backdrop]').hide();
    $('#alertasModal').hide();
}

function limpaModal() {
    $('.form-contato').find('input:text').val('');
    $('.form-contato').find('input:checkbox').prop('checked', false);
    $('.form-contato').find('select').val('0');
    $('.form-alertas').find('input:text').val('');
}


function cadastraContato() {
   
        var mailingPort = "";
        var mailingIng = "";
        var mailingEs = "";

        var nomeAlerta = $('#nome_modal').val();
        var emailAlerta = $('#email_modal').val();
        var telefone = $('#controlTelefone').val();
        var empresa = $('#controlEmpresa').val();
        var cargo = $('#controlCargo').val();
        var perfil = $('#controlPerfil').val();
        //var txtCaptcha = $('input[id*=txtCaptcha]').val();

        var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;

        if (!filter.test(emailAlerta)) {
            if ($(".hidLinguagem").val() == "ptg") {
                alert('E-mail Inválido!');
            }
            else if ($(".hidLinguagem").val() == "eng") {
                alert("Invalid E-mail!");
            }
            return false;
        }

        if (document.getElementById('controlMailingPort').checked == true) {
            mailingPort = $('#controlMailingPort').val();
        }
        if (document.getElementById('controlMailingIng').checked == true) {
            mailingIng = $('#controlMailingIng').val();
        }

        CallServer("alerta;" + nomeAlerta + ";" + emailAlerta + ";" + telefone + ";" + empresa + ";" + cargo + ";" + perfil + ";" + mailingPort + ";" + mailingIng + ";" + mailingEs);
   

}
// FIM Alertas

function ExportarOutlookHome(titulo, descricao, cidade, datainicio, datafim) {
    var calSingle = new ics();
    calSingle.addEvent(titulo, descricao, cidade, datainicio, datafim);
    calSingle.download(titulo);
}

function retornoCallback(arg) {
    var args = arg.split(';');

    switch (args[0]) {
        case "impressao":
            {
                executaImpressao(args[1]);
                break;
            }
        case "buscarShow":
            {
                alert(args[1]);
                break;
            }
        case "email":
            {
                if (args[1] == "success") {
                    alert(args[2]);
                    fechaBoxEmail();
                }
                else
                    alert(args[2]);
                break;
            }
        case "novaDescricaoTriResponse":
            exibirNovaDescricao(args[1], args[2]);
            break;
        case "lembreteAgenda":
            var alertagenda = $('input[id$=MsgLembreteAgenda]').val();
            limparCamposAgenda();
            alert(alertagenda);
            break;
        case "paginarResponse":
            efetuarPaginacaoResponse(args[1], args[2]);
            break;
        case "alerta":
            {
                var alertari = $('input[id$=MsgSucessoRi]').val();
                alert(alertari);
                fechaBoxAlerta();
                limpaModal();
                break;
            }
        case "alertaContatoExiste":
            {
                var mensagem = unescape(args[1]);
                eval(mensagem);
                fechaBoxAlerta();
                limpaModal();
                break;
            }
        case "EventosAnteriores":
            {
                carregarEventosAnteriores(args);
                break;
            }
        case "EventosProximos":
            {
                carregarEventosProximos(args);
                break;
            }
        case "paginarcalendarioresponsive":
            {
                montaEventosCalendario(args[1]);
                mostraEventosDoDiaSelecionadoPosMudancaMes();
                break;
            }
        case "captchaIvalido":
            {
                var textoAlerta = $('input[id$=MsgErroCaptcha]').val();
                alert(textoAlerta);
                break;
            }
        default:
            break;
    }
}

function erroCallback(err) {
    alert("erro:" + err);
}

var hdfIdConteudosKitDownloads = [];
function setIdConteudoDownHidden(sender) {
    var idCript = sender.id;
    if ($.inArray(idCript, hdfIdConteudosKitDownloads) == -1) // Se id NÃO estiver no array
        hdfIdConteudosKitDownloads.push(idCript);
    else
        hdfIdConteudosKitDownloads = $.grep(hdfIdConteudosKitDownloads, function (value) {
            return value !== idCript;
        });

    $("input[id*=hdfIdConteudosKitDownloads]").val(hdfIdConteudosKitDownloads);
}

function baixarTodosArquivosHome(event) {
    var elementId = event.currentTarget.getAttribute("resultado");
    $("input[id*=hdfIdBaixarTodos]").val(elementId);

    __doPostBack(uniqueIdBaixarTodos);
}

function clearInputHiden() {
    $("input[id*=hdfIdConteudosKitDownloads]").val('');
}

function ValidaKit() {

    if ($('input[type=checkbox][class*=inputResul]:checked').val() == null) {
        if ($(".hidLinguagem").val() == "ptg")
            alert("Selecione pelo menos um arquivo!");
        else
            alert("Select at least one file!");
        $('input[type=checkbox][class*=inputResul]').focus();
        return false;
    }
}