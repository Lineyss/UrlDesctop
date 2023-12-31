﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Ссылки

let domainName = 'https://localhost:7126/';
let action = 'FileManager/';
let mainUrl = domainName + action;

function Href(url) {
    let customUrl = mainUrl + url;
    window.location.href = customUrl;
}
function HrefAction(url, action) {
    let customUrl = domainName + action + url;
    window.location.href = customUrl;
}

let Files = document.querySelectorAll(".Files");

Files.forEach(element => {
    element.addEventListener("click", (e) => {
        console.log(navigator.platform.indexOf("Win"));
        if (navigator.platform.indexOf("Win") != -1)
        {
            e.preventDefault();
            Href(element.getAttribute("href")); 
        }
    });
})

// Поиск

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

// Нажатие на элементы

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

let disabledButtons = document.querySelectorAll(".disabledButton");

function checkSelectedItem() {
    disabledButtons.forEach(element => {
        if (listSelectedItem.length > 0) {
            element.disabled = false;
        }
        else {
            element.disabled = true;
        }
    })
}

// Создать папку и Загрузить файл
function Post(url, value) {
    const Http = new XMLHttpRequest();
    console.log(url);
    Http.open("POST", url, false)
    if (value != null) {
        Http.setRequestHeader("Content-Type", "application/json");
        Http.send(JSON.stringify(value));
    }
    else {
        Http.send()
    }

    if (Http.status == 200) {
        return true
    }

    return false;
}


let popupConteiner = document.querySelector(".popupConteiner");
let inputCreateFolder = document.querySelector(".inputCreateFolder");
let inputUploadFiles = document.querySelector(".inputUploadFiles");

popupConteiner.addEventListener("click", (e) => {
    if (inputCreateFolder != document.activeElement && e.target.tagName != 'BUTTON') {
        popupConteiner.classList.add("Hide");
    }
});

document.querySelector(".buttonCreateFolder").addEventListener("click", () => {
    popupConteiner.classList.remove("Hide");
});

document.querySelector(".buttonPopCreateFolder").addEventListener("click", () => {
    let url = input.value + inputCreateFolder.value;
    if (inputCreateFolder.value.length > 0) {
        url = domainName + "CreateFolder/" + input.value + "/" + inputCreateFolder.value;

        if (Post(url)) {
            window.location.reload(true);
        }
    }
});

document.querySelector(".buttonUploadFiles").addEventListener("click", () => {
    console.log(1)
    inputUploadFiles.click();
})


// Удаление и Скачивание

let poputLoad = document.querySelector(".load");


disabledButtons.forEach(element => {
    element.addEventListener("click", () => {
        if (element.innerText == "Скачать") {
            poputLoad.classList.remove("Hide");
            downloadZip(domainName + "Download")
        }
        else {
            window.location.reload(
                Post(domainName + "Delete", convertArr())
            );
        }
    })
})

function downloadZip(url) {
    data = convertArr()

    var xhr = new XMLHttpRequest();
    xhr.open('POST', url);
    xhr.responseType = "blob";
    xhr.setRequestHeader('Content-Type', 'application/json');

    // Конвертируем массив в JSON формат
    var listSelectedItem = JSON.stringify(data);

    console.log(listSelectedItem)
    console.log(url)

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var blob = new Blob([xhr.response], { type: 'application/zip' });
            var url = window.URL.createObjectURL(blob);

            // Создаем ссылку на скачивание архива
            var link = document.createElement('a');
            link.href = url;
            link.download = 'archive.zip';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            window.URL.revokeObjectURL(url);

            poputLoad.classList.add("Hide");
        }
    };



    xhr.send(listSelectedItem);
}

function convertArr()
{
    let listItem = []

    listSelectedItem.forEach(element => {
        listItem.push(element.children[4].children[0].getAttribute("href"));
    })

    return listItem;
}