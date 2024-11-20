import { SendRequest, populateDropdown } from '../comerp_Utility/comerp_SendRequestUtility.js';
import { createActionButtons, hideLoader, initializeDataTable, resetFormValidation, resetValidation, showCreateModal, showLoader } from '../comerp_Utility/comerp_helpers.js';
import { notification, notificationCatch, notificationErrors } from '../comerp_Utility/comerp_notification.js';

// Dynamically define controller and form elements
const controllerName = "Role";
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
const addLabel = `#${controllerName}ModalLabelAdd`;
const updateLabel = `#${controllerName}ModalLabelUpdate`;
$(document).ready(async function () {
    debugger
    await initGitAllList();
    await CreateRoleBtn(createButton);
});
const initGitAllList = async () => {
    await getRoleList();
}


// Dynamically fetch data for verbs or other entities
const getRoleList = async () => {
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
                name: entity.roleName,
            };
        }
        return null;
    }).filter(Boolean);

    debugger;

    // Define the table schema
    const entitySchema = [
        { data: null, title: 'Name', render: (data, type, row) => row.name || "Null" },
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


// Initialize validation
const InitializegetRolevalidation = $(formName).validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        RoleName: {
            required: true,
            minlength: 2,
            maxlength: 50
        }
    },
    messages: {
        RoleName: {
            required: "Role Name is required.",
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
export const CreateRoleBtn = async (CreateBtnId) => {
    // Show Create Model
    $(CreateBtnId).off('click').click(async (e) => {
        e.preventDefault();
        resetFormValidation(formName, $(formName).valid());
        $(updateLabel).hide();
        $(addLabel).show();
        showCreateModal(modalCreateId, saveButtonId, updateButtonId);
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
                notificationErrors({ message: result.message + result.detail , title: `Error: Create Failed  ${controllerName} ` });
            }
        }
    } catch (error) {
        notificationCatch({ message: error, title: `Error: Failed to Create ${controllerName}` });
    }
});
window.updateRole = async (id) => {

    try {
        resetFormValidation(formName, $(formName).valid());
        debugger
        $(updateLabel).show();
        $(addLabel).hide();
        $(formName)[0].reset();
        const result = await SendRequest({ endpoint: endpointGetById + id });
        if (result.success) {
            $(saveButtonId).hide();
            $(updateButtonId).show();
            //update and buind Entity Id
            debugger
            $('#RoleName').val(result.data.roleName);

            $(modalCreateId).modal('show');
            resetValidation(InitializegetRolevalidation, formName);
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

window.deleteRole = async (id) => {
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