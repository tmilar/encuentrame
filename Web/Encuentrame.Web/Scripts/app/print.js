$(document).ready(function () {
    if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {   // Chrome Browser Detected?
        console.log('Chrome');
        window.PPClose = false;                                     // Clear Close Flag
        window.onbeforeunload = function () { // Before Window Close Event
            if (window.PPClose === false) { // Close not OK?
                return 'Leaving this page will block the parent window!\nPlease select "Stay on this Page option" and use the\nCancel button instead to close the Print Preview Window.\n';
            }
        };
        window.print();                                             // Print preview
        window.PPClose = true;                                      // Set Close Flag to OK.
    } else {
        console.log('noChrome');
        window.print();
        window.close();
    }
});