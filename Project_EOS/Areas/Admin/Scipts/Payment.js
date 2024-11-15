$(document).ready(function () {
    $('.edit-button').click(function () {
        var row = $(this).closest('tr');
        row.find('span').hide();
        row.find('input').show();
        row.find('.edit-button').hide();
        row.find('.update-button').show();
        row.find('.cancel-button').show();
    });

    $('.cancel-button').click(function () {
        var row = $(this).closest('tr');
        row.find('span').show();
        row.find('input').hide();
        row.find('.edit-button').show();
        row.find('.update-button').hide();
        row.find('.cancel-button').hide();
    });

    $('.update-button').click(function () {
        var row = $(this).closest('tr');
        var paymentID = $(this).data('id');
        var paymentDate = row.find('.edit-payment-date').val();
        var paymentMethod = row.find('.edit-payment-method').val();

        $.ajax({
            url: '/Admin/PaymentManage/UpdatePayment',
            type: 'POST',
            data: {
                paymentID: paymentID,
                paymentDate: paymentDate,
                paymentMethod: paymentMethod
            },
            success: function (response) {
                if (response.success) {
                    row.find('.payment-date').text(paymentDate);
                    row.find('.payment-method').text(paymentMethod);
                    row.find('span').show();
                    row.find('input').hide();
                    row.find('.edit-button').show();
                    row.find('.update-button').hide();
                    row.find('.cancel-button').hide();
                } else {
                    alert('Failed to update payment');
                }
            }
        });
    });
});
