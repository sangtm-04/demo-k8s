$(document).ready(function () {
    new Customer();
});

class Customer {
    constructor() {
        this.initEvents();
    }

    initEvents() {
        $(document).on('click', '.btn-submit', function () {
            const customer = {
                CustomerName: $('input[name="CustomerName"]').val()
            };
            $.ajax({
                method: 'POST',
                url: '/api/customers',
                data: JSON.stringify(customer),
                async: true,
                contentType = 'application/json'
            }).done(function (res) {
                alert(res.message);
            }).fail(function (res) {
                console.log(res.message);
            });
        });
    }
}