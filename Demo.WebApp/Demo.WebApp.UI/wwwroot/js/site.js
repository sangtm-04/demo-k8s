$(document).ready(function () {
    new Customer();
});

class Customer {
    constructor() {
        $('input.form-control').focus();
        this.getCustomers();
        this.initEvents();
    }

    /**
     * Xử lý sự kiện trên màn hình quản lý khách hàng
     * @param
     * */
    initEvents() {
        $(document).on('click', '.btn-submit', this.insertCustomer.bind(this));
        $(document).on('keydown', 'input.form-control', this.onEnterKeyWhenInputCustomerName);
    }

    /**
     * Thêm mới 1 khách hàng
     * @param
     * */
    insertCustomer() {
        const customer = {
            CustomerName: $('input.form-control').val()
        };
        const customerObj = this;
        $.ajax({
            method: 'POST',
            url: '/api/customers',
            data: JSON.stringify(customer),
            contentType: 'application/json'
        }).done(function (res) {
            if (res.data > 0) {
                alert(res.message);
                $('input.form-control').val('')
                $('input.form-control').focus();
                customerObj.getCustomers();
            }
        }).fail(function (res) {
            console.log(res.message);
        });
    }

    /**
     * Lấy danh sách khách hàng
     * @param
     * */
    getCustomers() {
        $.ajax({
            method: 'GET',
            url: '/api/customers'
        }).done(function (res) {
            $('.list-customer').empty();
            if (res.data === -1) {
                $('.list-customer').append(`<div>${res.message}</div>`)
            }
            else {
                $.each(res.data, function (index, customer) {
                    $('.list-customer').append(`<div>${index+1}. ${customer.customerName}</div>`)
                });
            }
            
        }).fail(function (res) {
            console.log(res.message);
        });
    }

    /**
     * Bắt sự kiện nhấn phím enter ở ô nhập tên khách hàng
     * @param {any} e
     */
    onEnterKeyWhenInputCustomerName(e) {
        var keycode = e.charCode || e.keyCode;
        if (keycode === 13) {
            $(this).siblings('.btn-submit').trigger('click');
        }
    }
}
