$(document).ready(function () {
    let buyButton = $("#buyButton");
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