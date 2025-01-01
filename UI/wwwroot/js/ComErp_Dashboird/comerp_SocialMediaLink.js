import { SendRequest, populateDropdown } from '../comerp_Utility/comerp_SendRequestUtility.js';
import { createActionButtons, hideLoader, initializeDataTable, resetFormValidation, resetValidation, showCreateModal, showLoader } from '../comerp_Utility/comerp_helpers.js';
import { notification, notificationCatch, notificationErrors } from '../comerp_Utility/comerp_notification.js';

// Dynamically define controller and form elements
const controllerName = "SocialMediaLink";
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
    await CreateSocialMediaLinkBtn(createButton);
});
const initGitAllList = async () => {
    await getSocialMediaLinkList();
}


// Dynamically fetch data for verbs or other entities
const getSocialMediaLinkList = async () => {
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
                company: entity.company.name,
                platform: entity.platform,
                linkUrl: entity.linkUrl,
                iconUrl: entity.iconUrl,
                isVisible: entity.isVisible,
                displayOrder: entity.displayOrder,
            };
        }
        return null;
    }).filter(Boolean);

    debugger;

    // Define the table schema
    const entitySchema = [
        { data: null, title: 'Icon', render: (data, type, row) => `<img src="images/Social_Icon/${row?.iconUrl}" alt="User Avatar" class="rounded-circle" style="width: 30px; height: 30px; object-fit: cover;" onerror="this.onerror=null;this.src='/ProjectRootImg/default-user.png';" />` }, 
        { data: null, title: 'Platform', render: (data, type, row) => row.platform || "Null" },
        { data: null, title: 'Link', render: (data, type, row) => row.linkUrl || "Null" },
        { data: null, title: 'Order', render: (data, type, row) => row.displayOrder || "Null" },
        {
            data: null, title: 'Visible', render: (data, type, row) => row.isVisible ? '<span style="color: green; font-weight: bold;">Active</span>'
                : '<span style="color: red; font-weight: bold;">Inactive</span>' },
        { data: null, title: 'Company', render: (data, type, row) => row.company || "Null" },
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
const InitializegetSocialMediaLinkvalidation = $(formName).validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        Platform: {
            required: true,
        },
        LinkUrl: {
            required: true,
        },
        IconUrl: {
            required: true,

        },
        DisplayOrder: {
            required: true,

        },
        CompanyId: {
            required: true
        },
        FormFile: {
            required: true
        }
    },
    messages: {
        
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
export const CreateSocialMediaLinkBtn = async (CreateBtnId) => {
    // Show Create Model
    $(CreateBtnId).off('click').click(async (e) => {
        e.preventDefault();
        resetFormValidation(formName, $(formName).valid());
        $(updateLabel).hide();
        $(addLabel).show();
        showCreateModal(modalCreateId, saveButtonId, updateButtonId);
        await populateDropdown('/Company/Getall', '#CompanyDropdown', 'id', 'name', "Select Company");
    });
};

// Save button click SocialMediaLink
$(saveButtonId).off('click').click(async (e) => {
    e.preventDefault();
    debugger;
    try {
        // Trigger form validation before submission
        if ($(formName).valid()) {
            const formData = new FormData($(formName)[0]);
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
window.updateSocialMediaLink = async (id) => {

    try {
        resetFormValidation(formName, $(formName).valid());
        debugger
        $(updateLabel).show();
        $(addLabel).hide();
        $(formName)[0].reset();
        await populateDropdown('/Company/Getall', '#CompanyDropdown', 'id', 'name', "Select Company");
        const result = await SendRequest({ endpoint: endpointGetById + id });
        if (result.success) {
            $(saveButtonId).hide();
            $(updateButtonId).show();
            //update and buind Entity Id
            debugger
            $('#Platform').val(result.data.platform);
            $('#LinkUrl').val(result.data.linkUrl);
            $('#IconUrl').val(result.data.iconUrl);
            $('#FormFile').val(result.data.formFile);
            $('#DisplayOrder').val(result.data.displayOrder);
            $('#CompanyDropdown').val(result.data.companyId);
            $('#IsVisible').prop('checked', result.data.isVisible);
            


            $(modalCreateId).modal('show');
            resetValidation(InitializegetSocialMediaLinkvalidation, formName);
            $(updateButtonId).off('click').on('click', async (e) => {
                e.preventDefault();
                debugger
                const formData = new FormData($(formName)[0]);
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

window.deleteSocialMediaLink = async (id) => {
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