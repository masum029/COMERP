import { SendRequest, populateDropdown } from '../Utility/SendRequestUtility.js';
import { createActionButtons, hideLoader, initializeDataTable, resetFormValidation, resetValidation, showCreateModal, showLoader } from '../Utility/helpers.js';
import { notification, notificationCatch, notificationErrors } from '../Utility/notification.js';

// Dynamically define controller and form elements
const controllerName = "User";
const createButton = `#COMERP_Create${controllerName}Btn`;
const formName = `#${controllerName}Form`;
const modalCreateId = `#COMERP_${controllerName}ModelCreate`;
const saveButtonId = `#btnSave${controllerName}`;
const updateButtonId = `#btnUpdate${controllerName}`;
const dataTableId = `COMERP_${controllerName}Table`;
const deleteBtn = '#btnDelete';
const deleteModelId = `#COMERP_delete${controllerName}Model`;
const endpointCreate = `/${controllerName}/Create`;
const endpointGetAll = `/${controllerName}/GetAll`;
const endpointGetById = `/${controllerName}/GetById/`;
const endpointUpdate = `/${controllerName}/Update/`;
const endpointDelete = `/${controllerName}/Delete`;
$(document).ready(async function () {
    debugger
    await initGitAllList();
    await CreateUserBtn(createButton);
});
const initGitAllList = async () => {
    await getUserList();
}


// Dynamically fetch data for verbs or other entities
const getUserList = async () => {
    debugger;
    showLoader(); // Show loader at the start
    try {
        const result = await SendRequest({ endpoint: endpointGetAll });
        debugger;
        if (result.success) {
            await onSuccessEntities(result.data);
            debugger;
        } else {
            notificationErrors({ message: result.message, title: `Error: Failed to retrieve ${controllerName} list` });
        }
    } catch (error) {
        notificationErrors({ message: error, title: `Error: Failed to retrieve ${controllerName} list` });
    } finally {
        hideLoader(); // Hide loader after completion
    }
};


// Handle success and render the entity list
const onSuccessEntities = async (entities) => {
    debugger;

    // Transform and decode entities
    const entitiesList = entities.map((entity) => {
        if (entity) {
            return {
                id: entity?.id,
                name: entity.firstName + ' '+ entity.lastName,
                email: entity.email,
                phon: entity.phone,
                user: entity.userName,
                role: entity.roles[0],
            };
        }
        return null;
    }).filter(Boolean);

    debugger;

    // Define the table schema
    const entitySchema = [
        { data: null, title: 'Name', render: (data, type, row) => row.name || "Null" },
        { data: null, title: 'User Name', render: (data, type, row) => row.user || "Null" },
        { data: null, title: 'Email', render: (data, type, row) => row.email || "Null" },
        { data: null, title: 'Phone', render: (data, type, row) => row.phon || "Null" },
        { data: null, title: 'Role', render: (data, type, row) => row.role || "Null" },
        {
            data: null,
            title: 'Action',
            render: (data, type, row) => createActionButtons(row, [
                { label: 'Edit', btnClass: 'btn-success', callback: `update${controllerName}` },
                { label: 'Delete', btnClass: 'btn-danger', callback: `delete${controllerName}` }
            ])
        }
    ];

    debugger;

    // Initialize the DataTable with the transformed data
    await initializeDataTable(entitiesList, entitySchema, dataTableId);
};
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

$.validator.addMethod("checkDuplicateUsername", createDuplicateCheckValidator(
    "/User/CheckDuplicate",
    "UserName",
    "Username already exists."
));


$.validator.addMethod("checkDuplicateEmail", createDuplicateCheckValidator(
    "/User/CheckDuplicate",
    "Email",
    "Email already exists."
));
$.validator.addMethod("pwcheck", function (value) {
    return /[a-z]/.test(value); // At least one lowercase letter
});
// Initialize validation
const InitializegetUservalidation = $(formName).validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        FirstName: {
            required: true,
            minlength: 2,
            maxlength: 50
        },
        LastName: {
            required: true,
            minlength: 2,
            maxlength: 50
        },
        UserName: {
            required: true,
            checkDuplicateUsername: true
        },
        Email: {
            required: true,
            checkDuplicateEmail: true
        },
        Phone: {
            required: true
        },
        Password: {
            required: true,
            minlength: 6,
            pwcheck: true 
        },
        ConfirmationPassword: {
            required: true,
            equalTo: "#Password"
        },
        RoleName: {
            required: true,
            
        }
    },
    messages: {
        FirstName: {
            required: "First Name is required.",
            minlength: "First Name must be between 2 and 50 characters.",
            maxlength: "First Name must be between 2 and 50 characters."
        },
        LastName: {
            required: "Last Name is required.",
            minlength: "Last Name must be between 2 and 50 characters.",
            maxlength: "Last Name must be between 2 and 50 characters."
        },
        UserName: {
            required: "User Name is required.",
            checkDuplicateUsername: "This username is already taken."
        },
        Email: {
            required: "Email is required.",
            checkDuplicateEmail: "This email is already registered."
        },
        Phone: {
            required: "Phone Number is required."
        },
        Password: {
            required: "Password is required.",
            minlength: "Password must be at least 6 characters long.",
            pwcheck: "Password must contain at least one lowercase letter (a-z)." 
        },
        ConfirmationPassword: {
            required: "Confirmation Password is required.",
            equalTo: "Password and Confirmation Password do not match."
        },
        RoleName: {
            required: "You must select a role."
        }
    },
    errorElement: 'div',
    errorPlacement: function (error, element) {
        error.addClass('invalid-feedback');
        if (element.closest('.note').length > 0) {
            element.closest('.note').append(error);
        } else {
            element.after(error);
        }
    },
    highlight: function (element, errorClass, validClass) {
        $(element)
            .addClass('is-invalid')
            .removeClass('is-valid');
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element)
            .removeClass('is-invalid')
            .addClass('is-valid');
    }
});

// Reset validation and prepare modal
export const CreateUserBtn = async (CreateBtnId) => {
    // Show Create Model
    $(CreateBtnId).off('click').click(async (e) => {
        e.preventDefault();
        resetFormValidation(formName, $(formName).valid());
        $('#myModalLabelUpdate').hide();
        $('#myModalLabelAdd').show();
        showCreateModal(modalCreateId, saveButtonId, updateButtonId);
        await populateDropdown('/Role/Getall', '#RolesDropdown', 'roleName', 'roleName', "Select Role");
    });
};

// Save button click event
$(saveButtonId).off('click').click(async (e) => {
    e.preventDefault();
    debugger;
    try {
        // Trigger form validation before submission
        if ($(formName).valid()) {
            const formData = $(formName).serialize();
            const result = await SendRequest({ endpoint: endpointCreate, method: 'POST', data: formData });
            debugger;
            if (result.success && result.status === 201) {
                $(modalCreateId).modal('hide');
                notification({ message: `${controllerName} Created successfully !`, type: "success", title: "Success" });
                await initGitAllList();
            } else {
                notificationErrors({ message: result.message, title: `Error: Create Failed  ${controllerName} ` });
            }
        }
    } catch (error) {
        notificationCatch({ message: error, title: `Error: Failed to Create ${controllerName}` });
    }
});
window.updateUser = async (id) => {

    try {
        resetFormValidation(formName, $(formName).valid());
        debugger
        $('#myModalLabelUpdate').show();
        $('#myModalLabelAdd').hide();
        $(formName)[0].reset();
        await populateDropdown('/Role/Getall', '#RolesDropdown', 'roleName', 'roleName', "Select Role");
        const result = await SendRequest({ endpoint: endpointGetById + id });
        if (result.success) {
            $(saveButtonId).hide();
            $(updateButtonId).show();
            //update and buind Entity Id
            debugger
            $('#FirstName').val(result.data.firstName);
            $('#LastName').val(result.data.lastName);
            $('#UserName').val(result.data.userName);
            $('#Email').val(result.data.email);
            $('#Phone').val(result.data.phone);
            $('#Password').val(result.data.password);
            $('#ConfirmationPassword').val(result.data.confirmationPassword).html("hiden");
            $('#RolesDropdown').val(result.data.roles);

            $(modalCreateId).modal('show');
            resetValidation(InitializegetUservalidation, formName);
            $(updateButtonId).off('click').on('click', async (e) => {
                e.preventDefault();
                debugger
                const formData = $(formName).serialize();
                const result = await SendRequest({ endpoint: endpointUpdate + id, method: "PUT", data: formData });
                if (result.success) {
                    notification({ message: `${controllerName} Updated successfully !`, type: "success", title: "Success" });
                    $(modalCreateId).modal('hide');
                    await initGitAllList();
                } else {
                    notificationErrors({ message: result.detail, title: `Error: Failed to Updated ${controllerName}` });
                    $(modalCreateId).modal('hide');
                }
            });
        }
        
    } catch (error) {
        notificationCatch({ message: error, title: `Error: Failed to Update ${controllerName}` });
    }
}

////////window.showDetails = async (id) => {
////////    loger("showDetails id " + id);
////////}

window.deleteUser = async (id) => {
    debugger;
    try {
        $(deleteModelId).modal('show');
        $(deleteBtn).off('click').on('click', async (e) => {
            e.preventDefault();
            const result = await SendRequest({ endpoint: endpointDelete, method: "DELETE", data: { id: id } });
            if (result.success) {
                $(deleteModelId).modal('hide');
                notification({ message: `${controllerName} Deleted successfully !`, type: "success", title: "Success" });
                await initGitAllList();
            } else {
                $(deleteModelId).modal('hide');
                notificationErrors({ message: result.detail, type: "error", title: "Error" });
            }
        });
    } catch (error) {
        notificationCatch({ message: error, title: `Error: Failed to Delete ${controllerName}` });
    } 
}