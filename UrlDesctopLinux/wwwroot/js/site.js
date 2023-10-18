// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


document.querySelectorAll(".Files").forEach(element => {
    element.addEventListener("click", (e) => {
        if (navigator.platform.indexOf("Win") != -1)
        {
            let a = 'https://localhost:7126/' + element.getAttribute("href");
            window.location.href = a;
        }
    });
})