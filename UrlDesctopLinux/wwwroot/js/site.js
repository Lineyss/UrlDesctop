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

let listSelectedItem = [];

document.querySelectorAll(".CheckBoxInput").forEach(element => {
    element.addEventListener("click", () => {

        let parentElement = element.parentElement.parentElement;

        if (element.checked)
        {
            listSelectedItem.push(parentElement)
        }
        else
        {
            for (let i = 0; i < listSelectedItem.length; i++) {
                if (listSelectedItem[i] == parentElement)
                {
                    listSelectedItem.splice(i, 1)[0];
                }
            }
        }

        checkSelectedItem();
    })
})


function checkSelectedItem() {
    document.querySelectorAll(".disabledButton").forEach(element => {
        if (listSelectedItem.length > 0) {
            element.disabled = false;
        }
        else {
            element.disabled = true;
        }
    })
}