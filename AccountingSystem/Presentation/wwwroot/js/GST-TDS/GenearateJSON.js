$("#penalty").on('change', function () {
    calculateTotalGSTTDSAmount();
});

$('#interest').on('change', function () {
    calculateTotalGSTTDSAmount();
});

function calculateTotalGSTTDSAmount() {
   
    var penalty = parseFloat($('#penalty').val() == '' ? 0 : $('#penalty').val());
    var interest = parseFloat($('#interest').val() == '' ? 0 : $('#interest').val());
    var totalGstTdsPayment = gstTdsAmount + penalty + interest;
    $('#totalGstTdsPayment').val(totalGstTdsPayment);
}