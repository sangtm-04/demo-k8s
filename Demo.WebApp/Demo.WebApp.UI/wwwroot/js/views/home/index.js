$(document).ready(function () {
    $(document).on('click', '.btn-submit', function () {
        const customer = {
            CustomerName: $('input[name="CustomerName"]').val()
        };
        $.ajax({
            method: 'POST',
            url: 'https://localhost:44326/api/customers',
            data: JSON.stringify(customer),
            async: true,
            contentType = 'application/json'
        }).done(function (res) {
            alert(res.message);
        }).fail(function (res) {
            console.log(res.message);
        });
    });
});