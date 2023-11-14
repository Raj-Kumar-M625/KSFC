
    //on page ready hide all branch option
    $("#selBranchName").find('option').hide();
    // set task as empty
    $("#selBranchName").val('');
    $("#selIfsc").val('');
    $("#selAccountNo").val('');
    $("#penalty").on('change', function () {
        calculateTotalTDSAmount();
    });

    $('#interest').on('change', function () {
        calculateTotalTDSAmount();
    });

    function calculateTotalTDSAmount() {
        
        var penalty = parseFloat($('#penalty').val() == '' ? 0 : $('#penalty').val());
        var interest = parseFloat($('#interest').val() == '' ? 0 : $('#interest').val());
        var totalTdsPayment = tdsAmount + penalty + interest;
        $('#totalTdsPayment').val(totalTdsPayment);
    }

    $("#selBankName").on('change', function () {
        debugger;
        var selectedBank = $("#selBankName").val();
        if (selectedBank != '') {
            $("#selBranchName").find('option').hide();
            $("#selBranchName option[value='']").show();
            $('*[data="' + selectedBank + '"]').show();
            $("#selIfsc").val('');
            $("#selAccountNo").val('');
            $("#selBankMasterId").val('');
        }
        else {
            // if BankName not selected then hide all tasks
            $("#selBranchName").find('option').hide();
            $("#selBranchName").val('');
        }

    });

    // onchange of BranchName Drop down
    $("#selBranchName").on('change', function () {
        var selectedBranchDetails = $("#selBranchName").val().split(',');
        var selectedBranch = selectedBranchDetails[0];
        var BankMasterId = selectedBranchDetails[1];
        var selectedBranchAcnt = selectedBranchDetails[2];
        if (selectedBranch != '') {
            $("#selIfsc").val(selectedBranch);
            $("#selAccountNo").val(selectedBranchAcnt);
            $("#selBankMasterId").val(BankMasterId);
            var id = $("#selBankMasterId").val(BankMasterId);
            $("#branchid").val(BankMasterId);
        }
        else {
            // if BranchName not selected then hide all tasks
            $("#selIfsc").val('');
            $("#selAccountNo").val('');
            $("#selBankMasterId").val('');
        }

    });


