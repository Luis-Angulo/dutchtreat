let form = document.getElementById("myForm");
form.hidden = true;

let buyButton = document.getElementById("buyButton");
// buyButton.onclick = (e) => alert("clicked");
buyButton.addEventListener("click", (e) => alert("buying item"));