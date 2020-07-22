// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    new Customer();
});

class Customer {
    constructor() {
        $('input.form-control').focus();
        this.initEvents();
    }

    initEvents() {
        $(document).on('click', '.btn-submit', function () {
            const customer = {
                CustomerName: $('input.form-control').val()
            };
            $.ajax({
                method: 'POST',
                url: '/api/customers',
                data: JSON.stringify(customer),
                contentType: 'application/json'
            }).done(function (res) {
                alert(res.message);
                var customerName = $('input.form-control').val();
                $('input.form-control').val('')
                $('input.form-control').focus();
                $('.list-customer').append(`<div>${customerName}</div>`)
            }).fail(function (res) {
                console.log(res.message);
            });
        });
    }
}
