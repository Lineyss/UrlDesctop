// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function Href(url) {
    let a = 'https://localhost:7126/' + url;
    window.location.href = a;
}

document.querySelectorAll(".Files").forEach(element => {
    element.addEventListener("click", (e) => {
        if (navigator.platform.indexOf("Win") != -1)
        {
            e.preventDefault();
            Href(element.getAttribute("href")); 
        }
    });
})

let button = document.querySelector(".buttonSearch");
let input = document.querySelector(".inputSearch");

button.addEventListener("click", () => {
    let text = input.value;
    if (navigator.platform.indexOf("Win") != -1) {
        Href(text);
    }
    else {
        window.location.href = text;
    }
});

input.addEventListener("keyup", (e) => {
    if (e.keyCode == 13) {
        button.click();
    }
});



function ChangeDisableButton(Disable)
{
    document.querySelectorAll(".disabledButton").forEach(element => {
        element.disabled = Disable;
    })
}