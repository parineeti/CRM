/*

- Thank you for purchasing the multi-purpose Rocket - Coming Soon Template from hBeThemes. 
- You do not need to edit any of the Javascript below.

*/


"use strict";


// Popup Javascript
var modal = document.getElementById('notifyPopup');
var btn = document.getElementById('notify-me-btn-margin');
var span = document.getElementById('notifyclose');

btn.onclick = function() {
    modal.setAttribute( 'style', 'display:block' );
}

span.onclick = function() {
    modal.setAttribute( 'style', 'display:none' );
}

window.onclick = function(event) {
    if (event.target == modal) {
        modal.setAttribute( 'style', 'display:none' );
    }
}



// Smooth Scroll Javascript
$(function() {
    $('a[href*="#"]:not([href="#"])').on('click', function() {
        if (location.pathname.replace(/^\//,'') == this.pathname.replace(/^\//,'') && location.hostname == this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) +']');
            if (target.length) {
                $('html, body').animate({
                scrollTop: target.offset().top
                }, 1000);
                return false;
            }
        }
    });
});



// Close error / success notification
function close_success(id) {
    if(id === 1) {
        jQuery("#form-success").hide();
    }
}
function close_error(id) {
    if(id === 1) {
        jQuery("#form-error").hide();
    }
}