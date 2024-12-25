
export const loger = (message) => {
    console.log(message);
}
///**
// * Initializes a DataTable with the given data and configuration.
// * @param {Array} data - The data to be displayed in the table.
// * @param {Array} columnsConfig - Configuration for the table columns.
// * @param {string} tableId - The ID of the table element.
// */
//export const initializeDataTable = async (data, schema, tableId) => {
//    try {
//        debugger
//        if (!Array.isArray(data) || !Array.isArray(schema) || typeof tableId !== 'string') {
//            throw new Error('Invalid arguments passed to initializeGenericDataTable');
//        }
//        // Handle null values in schema
//        //schema.forEach(x => {
//        //    if (x.value === null) {
//        //        x.value = "No data";
//        //    }
//        //});
//        $(window).on('resize', function () {
//            if ($.fn.DataTable.isDataTable(`#${tableId}`)) {
//                $(`#${tableId}`).DataTable().columns.adjust().responsive.recalc();
//            }
//        });

//        const tableElement = $(`#${tableId}`);
//        if (!tableElement.length) {
//            throw new Error(`Table with ID ${tableId} not found`);
//        }

//        // Check if the DataTable is already initialized
//        if ($.fn.DataTable.isDataTable(tableElement)) {
//            // Destroy the existing DataTable
//            tableElement.DataTable().destroy();
//        }
//        // Inside initializeDataTable function
//        tableElement.DataTable({
//            processing: true,
//            lengthChange: true,
//            lengthMenu: [[5, 10, 20, 30, -1], [5, 10, 20, 30, 'All']],
//            searching: true,
//            ordering: true,
//            paging: true,
//            data: data,
//            columns: schema,
//            responsive: true
            
//        });
//    } catch (error) {
//        console.error('Error initializing DataTable:', error);
//    }
//}
export const initializeDataTable = async (data, schema, tableId) => {
    try {
        console.log("Initializing DataTable with data:", data);
        console.log("Using schema:", schema);
        console.log("Table ID:", tableId);

        // Validate inputs
        if (!Array.isArray(data) || !Array.isArray(schema) || typeof tableId !== 'string') {
            throw new Error('Invalid arguments passed to initializeDataTable');
        }

        // Select the table element
        const tableElement = $(`#${tableId}`);
        if (!tableElement.length) {
            throw new Error(`Table with ID ${tableId} not found`);
        }

        // Destroy existing DataTable if initialized
        if ($.fn.DataTable.isDataTable(tableElement)) {
            tableElement.DataTable().destroy(); // Destroy existing DataTable
        }

        // Handle the case where data is null or empty
        if (!data || data.length === 0) {
            // Initialize the DataTable but with an empty dataset
            tableElement.DataTable({
                processing: true,
                lengthChange: true,
                lengthMenu: [[10, 20, 30, -1], [10, 20, 30, 'All']],
                searching: true,
                ordering: true,
                paging: true,
                data: [],  // Pass an empty array for data
                columns: schema,
                responsive: true
            });

            // Insert a row with a "No data available" message
            tableElement.find('tbody').html('<tr><td colspan="' + schema.length + '" class="text-center">No data available</td></tr>');
            return;
        }

        // Initialize the DataTable with actual data
        const dataTable = tableElement.DataTable({
            processing: true,
            lengthChange: true,
            lengthMenu: [[10, 20, 30, -1], [10, 20, 30, 'All']],
            searching: true,
            ordering: true,
            paging: true,
            data: data,  // Set the actual data here
            columns: schema,
            responsive: true
        });

        // Responsive adjustment on window resize
        $(window).off('resize').on('resize', function () {
            if ($.fn.DataTable.isDataTable(tableElement)) {
                dataTable.columns.adjust().responsive.recalc(); // Adjust and recalculate
            }
        });

        // Trigger a resize manually to force recalculation when the page loads
        $(window).trigger('resize');
    } catch (error) {
        console.error('Error initializing DataTable:', error);
    }
};







// Helper function to create action buttons

const createActionButton = (buttonType, btnClass, id, action, disabled = false) => {
    const disabledAttr = disabled ? 'disabled' : '';
    return `<button class="btn ${btnClass} btn-sm ms-1" onclick="${action}('${id}')" ${disabledAttr}>${buttonType}</button>`;
}

export const createActionButtons = (row, actions) => {
    return actions.map(action => createActionButton(action.label, action.btnClass, row.id, action.callback, action.disabled)).join(' ');
}

export const showCreateModal = (modalId, saveBtnId, updateBtnId) => {
    // Clear all text inputs within the specified modal
    $(`${modalId} input[type="text"]`).val('');

    // Show the modal
    $(`${modalId}`).modal('show');

    // Show and hide the specified buttons
    $(`${saveBtnId}`).show();
    $(`${updateBtnId}`).hide();
}


export const showExceptionMessage = (successId, message) => {
    // Ensure the successId is a valid selector
    if (typeof successId !== 'string' || !successId.startsWith('#')) {
        console.error('Invalid successId provided. Must be a string starting with "#".');
        return;
    }

    // Ensure the message is a valid string
    if (typeof message !== 'string') {
        console.error('Invalid message provided. Must be a string.');
        return;
    }

    // Find the success message element and update its content
    const $successElement = $(successId);
    if ($successElement.length) {
        $successElement.text(message).show();
    } else {
        console.error(`Element with selector ${successId} not found.`);
    }
};


export const displayNotification = ({
    messageElementId = '#successMessage',
    modalId = '#exampleModal',
    formId = '#myForm',
    message = 'Operation successful',
    onSuccess = () => { },
    onError = () => { }
}) => {
    // Ensure the messageElementId is a valid selector
    if (typeof messageElementId !== 'string' || !messageElementId.startsWith('#')) {
        console.error('Invalid messageElementId provided. Must be a string starting with "#".');
        onError();
        return;
    }

    // Ensure the message is a valid string
    if (typeof message !== 'string') {
        console.error('Invalid message provided. Must be a string.');
        onError();
        return;
    }

    // Find the message element and update its content
    const $messageElement = $(messageElementId);
    if ($messageElement.length) {
        $messageElement.text(message).show();
    } else {
        console.error(`Element with selector ${messageElementId} not found.`);
        onError();
        return;
    }
    debugger
    // Hide the modal
    $(modalId).modal('hide');

    // Reset the form
    $(formId)[0].reset();

    onSuccess();
};



export const initializevalidation = (formselector, rules, messages) => {
    const validator = $(formselector).validate({
        onkeyup: function (element) {
            $(element).valid();
        },
        rules: rules,
        messages: messages,
        errorelement: 'div',
        errorplacement: function (error, element) {
            error.addclass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorclass, validclass) {
            $(element).addclass('is-invalid');
        },
        unhighlight: function (element, errorclass, validclass) {
            $(element).removeclass('is-invalid');
        }
    });
    debugger
    // bind validation on change and focus events
    $(formselector + ' input[type="text"]').on('change focus', function () {
        validator.element($(this));
    });

    return validator;
}

export const resetValidation = (validator, formSelector) => {
    validator.resetForm(); // Reset validation
    $(formSelector + ' .form-group .invalid-feedback').remove(); // Remove error messages
    $(formSelector + ' input').removeClass('is-invalid'); // Remove error styling
};



export const dataToMap = (data, key) => {
    debugger
    return data.reduce((map, item) => {
        map[item[key]] = item;
        return map;
    }, {});
};

export const clearMessage = (...messageIds) => {
    messageIds.forEach(id => {
        $(`#${id}`).hide();
    });
}
export const resetFormValidation = (formName, validator) => {
    if (!validator) {
        // If no validator is passed, get it from the form
        validator = $(formName).validate();
    }
    // Reset error classes
    $(formName).find('.is-invalid').removeClass('is-invalid');
    $(formName).find('.is-valid').removeClass('is-valid');

    // Optionally reset the form fields
    $(formName)[0].reset(); // Reset all inputs to their initial values
};

export const showLoader = () => {
    $('#loader').show();
};

export const hideLoader = () => {
    $('#loader').hide();
};


/**
    * Sets a date field with a formatted date value or clears it if the value is invalid or default.
    *
    * @param {string} fieldId - The selector for the field (e.g., '#StartDate').
    * @param {string|Date} dateValue - The date value to be processed (in string or Date format).
    */
export const setDateField = (fieldId, dateValue) => {
    const parsedDate = new Date(dateValue);
    const defaultDate = new Date('1970-01-01');

    // Check if the parsed date is valid and not the default date
    if (!isNaN(parsedDate.getTime()) && parsedDate.getTime() !== defaultDate.getTime()) {
        // Format the date in local time (YYYY-MM-DD)
        const formattedDate = [
            parsedDate.getFullYear(),
            String(parsedDate.getMonth() + 1).padStart(2, '0'), // Month is 0-based
            String(parsedDate.getDate()).padStart(2, '0')
        ].join('-');
        $(fieldId).val(formattedDate); // Set the formatted date to the field
    } else {
        $(fieldId).val(''); // Clear the field for invalid or default date
    }
};