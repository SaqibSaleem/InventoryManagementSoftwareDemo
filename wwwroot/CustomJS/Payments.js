$(document).ready(function () {
   
})
var stripe = Stripe("pk_test_51SkBC0QryqqrRais5B1HtCx9pLHG54DPREKyLRx0UjAG1kDRUGmAGmAR8a43otJFWVqlAkbJVTirn7ArnunddXIr00LhifTSh4");
var elements = stripe.elements();
var card = elements.create("card");

card.mount("#card-element");

// Call to the stripe
function btnBuy(id) {
    $.ajax({
        url: "/payment/create",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ id: id }),
        success: function (response) {

            // Stripe card confirmation
            stripe.confirmCardPayment(response.clientSecret, {
                payment_method: {
                    card: card,
                    billing_details: {
                        email: "test@email.com"
                    }
                }
            }).then(function (result) {

                if (result.error) {
                    $("#card-errors").text(result.error.message);
                } else {
                    if (result.paymentIntent.status === "succeeded") {
                        alert("Payment successful!");
                    }
                }
            });
        },
        error: function () {
            alert("Error creating payment");
        }
    });

};
