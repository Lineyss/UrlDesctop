// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.querySelectorAll(".Files").forEach(element => {
    element.addEventListener("click", (e) => {
        e.preventDefault();
        let a = 'https://localhost:7126/' + element.textContent;
        window.location.href = a;
    });
})