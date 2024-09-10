
$(document).ready(function() {

    // Fix input element click problem
    $('.dropdown input, .dropdown label, .dropdown-menu a[data-toggle="tab"]').click(function(e) {
        e.stopPropagation()
        $(this).tab('show')
    });

    $('.btn-search, .btn-fechar').click(function () {
        $(".darkLayer").slideToggle();
    });


    function accessApplyTheme(theme) {
        localStorage.setItem('access_theme', theme)

        if (theme == 'dark') {
            $('body').addClass('contraste');
        } else {
            $('body').removeClass('contraste');
        }
    }

    var access_theme = 'light';

    if (localStorage.getItem('access_theme')) {
        access_theme = localStorage.getItem('access_theme');
        accessApplyTheme(access_theme);
    }

    $('.btn-contraste').on('click', function (e) {
        e.preventDefault();

        if (access_theme == 'light') {
            access_theme = 'dark';
        } else {
            access_theme = 'light';
        }
        accessApplyTheme(access_theme);
    });


    $('.carousel').carousel({
        pause: true,
        interval: false
    });


    $('[data-toggle="tooltip"]').tooltip({
        html: true,
        container: 'body'
    });

    $('[data-toggle="popover"]').popover({
        html: true
    });




});

