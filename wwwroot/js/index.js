// ready does two things
// it runs the function arg only once the document is done loading
// it also means that all the code in that function is not sharing the browsers global scope but has its own scope
$(document).ready(function () {
    let form = $("#myForm");
    form.hide();

    let buyButton = $("#buyButton");
    // buyButton.onclick = (e) => alert("clicked");
    // buyButton.addEventListener("click", (e) => alert("buying item"));
    buyButton.on("click", (e) => console.log("buying item"));

    let productInfo = $(".product-props li");
    productInfo.on("click", function () {
        // remember this in a function() and this in a fat arrow are different
        console.log("You clicked on " + $(this).text());
    });

    $("#loginToggle").on("click", () => {
        $(".popup-form").fadeToggle(200);
    });
});