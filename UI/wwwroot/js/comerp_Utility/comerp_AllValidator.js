// Fatch duplucate file 

const createDuplicateCheckValidator = (endpoint, key, errorMessage) => {
    return function (value, element) {
        let isValid = false;
        $.ajax({
            type: "GET",
            url: endpoint,
            data: { key: key, val: value },
            async: false,
            success: function (response) {
                isValid = !response;
            },
            error: function () {
                isValid = false;
            }
        });
        return isValid;
    };
}

// Helper.js
export const checkValidation = (formName) => {
    $(document).on('keyup change', `${formName} input, ${formName} select`, function (e) {
        e.preventDefault(); // Prevent default behavior
        const $element = $(this);

        // Check if the element is valid
        if ($element.valid()) {
            // Remove existing feedback to avoid duplicates
            $element.closest('.form-group, .col-md-4').find('.valid-feedback').remove();

            // Append professional feedback with a loading spinner
            $element.after(`
                <div class="valid-feedback d-flex align-items-center">
                    <i class="spinner-border spinner-border-sm text-success me-2" role="status"></i>
                    <span>Validating...</span>
                </div>
            `);

            // Replace spinner with a checkmark after validation
            setTimeout(() => {
                const feedback = $element.closest('.form-group, .col-md-4').find('.valid-feedback');
                feedback.html(`
                    <i class="bi bi-check-circle-fill text-success me-2"></i> <!-- Bootstrap Icon -->
                    <span>Ok</span>
                `);
            }, 200); // Short delay for better UX
        } else {
            // Remove feedback when invalid
            $element.closest('.form-group, .col-md-4').find('.valid-feedback').remove();
        }
    });
};







