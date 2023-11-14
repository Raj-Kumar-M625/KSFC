// For PaymentDate element
const paymentDateElement = document.getElementById('PaymentDate');
if (paymentDateElement) {
    paymentDateElement.max = new Date().toISOString().split('T')[0];
}

// For TenderDate element
const tenderDateElement = document.getElementById('TenderDate');
if (tenderDateElement) {
    tenderDateElement.max = new Date().toISOString().split('T')[0];
}

$("#utrno").on("change", function () {
    const UTRNO = $(this).val();
    if (utrAvailable.includes(UTRNO)) {
        swal.fire({
            title: 'UTR number already exists',
            icon: 'warning',
            confirmButtonText: 'Ok',
        }).then(() => {
            $(this).val("");
        });
    }
});

$("#challanno").on("change", function () {
    const ChallanNo = $(this).val();
    if (challanNoAvailable.includes(ChallanNo)) {
        swal.fire({
            title: 'Challan Number already exists',
            icon: 'warning',
            confirmButtonText: 'Ok',
        }).then(() => {
            $(this).val("");
        });
    }
});

$("#bsrCode").on("change", function () {
    const BSRCode = $(this).val();
    if (bsravailable.includes(BSRCode)) {
        swal.fire({
            title: 'BSR Code already exists',
            icon: 'warning',
            confirmButtonText: 'Ok',
        }).then(() => {
            $(this).val("");
        });
    }
});

$("#File").on("change", function () {
    const maxFileSize = 2097152;
    const fileData = document.getElementById('File').files[0];
    const fileName = document.querySelector('#File').value;
    const fileSize = fileData ? fileData.size : 0;
    const extension = fileName.split('.').pop();

    if (fileSize > maxFileSize) {
        swal.fire({
            title: "The file size is too large",
            icon: "warning",
            button: "Ok!",
        }).then(() => {
            $("#File").val(null);
        });
    } else if (extension !== "pdf") {
        swal.fire({
            title: "Please select a PDF file only",
            icon: "warning",
            button: "Ok!",
        }).then(() => {
            $("#File").val(null);
        });
    }
});
