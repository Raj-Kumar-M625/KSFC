document.addEventListener('DOMContentLoaded', function () {
    const firstInput = document.querySelector("#first");
    const secondInput = document.querySelector("#second");
    const firstError = document.querySelector("#first-error");
    const secondError = document.querySelector("#second-error");

    firstInput.addEventListener("input", function () {
        if (firstInput.value === secondInput.value) {
            secondInput.value = "";
            firstError.textContent = "UTR Numbers cannot be the same";
            secondError.textContent = "";
        } else {
            firstError.textContent = "";
        }
    });

    secondInput.addEventListener("input", function () {
        if (firstInput.value === secondInput.value) {
            firstInput.value = "";
            secondError.textContent = "UTR Numbers cannot be the same";
            firstError.textContent = "";
        } else {
            secondError.textContent = "";
        }
    });
});
