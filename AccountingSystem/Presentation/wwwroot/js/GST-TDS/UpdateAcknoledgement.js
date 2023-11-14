//on page ready hide all branch option
$("#selBranchName").find('option').hide();
// set task as empty
$("#selBranchName").val('');
$("#selIfsc").val('');
$("#selAccountNo").val('');


$("#selBankName").on('change', function () {
    var selectedBank = $("#selBankName").val();
    if (selectedBank != '') {
        $("#selBranchName").find('option').hide();
        $("#selBranchName option[value='']").show();
        $('*[data="' + selectedBank + '"]').show();
        $("#selIfsc").val('');
        $("#selAccountNo").val('');
        $("#selBankMasterId").val('');
        $('input:checkbox').removeAttr('checked');

    }
    else {
        // if BankName not selected then hide all tasks
        $("#selBranchName").find('option').hide();
        $("#selBranchName").val('');
        $('input:checkbox').removeAttr('checked');

    }

});

// onchange of BranchName Drop down
$("#selBranchName").on('change', function () {
    var selectedBranchDetails = $("#selBranchName").val().split(',');
    var selectedBranch = selectedBranchDetails[0];
    var BankMasterId = selectedBranchDetails[1];
    var selectedBranchAcnt = selectedBranchDetails[2];
    var selectedTdsLevel = selectedBranchDetails[3];
    if (selectedBranch != '') {
        $("#selIfsc").val(selectedBranch);
        $("#selAccountNo").val(selectedBranchAcnt);
        $("#selBankMasterId").val(BankMasterId);
        var id = $("#selBankMasterId").val(BankMasterId);
        $("#branchid").val(BankMasterId);
        const myCheckbox = document.getElementById("selectedbankGSTTDSLevel");
        myCheckbox.checked = (selectedTdsLevel === "True");

    }
    else {
        // if BranchName not selected then hide all tasks
        $("#selIfsc").val('');
        $("#selAccountNo").val('');
        $("#selBankMasterId").val('');
        $('input:checkbox').removeAttr('checked');

    }

});
$("#acknowledgementRefNo").on("change", function () {

    var ACNNO = $(this).val();

   
for (i = 0; i < acnAvailable.length; i++) {
    if (ACNNO == acnAvailable[i]) {
        swal.fire({
            title: 'AcknowledgementRef number already exists',
            icon: 'warning',
            confirmButtonText: 'Ok',
        }).then((result) => {
            $(this).val("");
        })
    }
}

    });