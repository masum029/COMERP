import { SendRequest, populateDropdown } from '../Utility/SendRequestUtility.js';
import { clearMessage, createActionButtons, initializeDataTable, loger, resetFormValidation, resetValidation, showCreateModal } from '../Utility/helpers.js';
import { notification } from '../Utility/notification.js';
// Dynamically define controller and form elements
const controllerName = "Description";
const createButton = `#Create${controllerName}Btn`;
const formName = `#${controllerName}Form`;
const modalCreateId = `#${controllerName}ModelCreate`;
const saveButtonId = `#btnSave${controllerName}`;
const updateButtonId = `#btnUpdate${controllerName}`;
const dataTableId = `${controllerName}Table`;
const deleteBtn = '#btnDelete';
const deleteModelId = `#delete${controllerName}Model`;
const endpointCreate = `/${controllerName}/Create`;
const endpointGetAll = `/${controllerName}/GetAll`;
const endpointGetById = `/${controllerName}/GetById/`;
const endpointUpdate = `/${controllerName}/Update/`;
const endpointDelete = `/${controllerName}/Delete`;
$(document).ready(async function () {
    await initGitAllList();
    await CreateDescriptionBtn(createButton);
});
const initGitAllList = async () => {
    await getDescriptionList();
}
function decodeHtml(html) {
    return $('<div>').html(html).text();
}
// Dynamically fetch data for verbs or other entities
const getDescriptionList = async () => {
    try {
        const result = await SendRequest({ endpoint: endpointGetAll });
        if (result.success) {
            await onSuccessEntities(result.data);
            debugger
        } else {
            console.error(`Failed to retrieve ${controllerName} list:`, result.message);
        }
    } catch (error) {
        console.error(`Error fetching ${controllerName} list:`, error);
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
                body: decodeHtml(entity?.body)?.substring(0, 100), // Decode HTML content
            };
        }
        return null;
    }).filter(Boolean);

    debugger;

    // Define the table schema
    const entitySchema = [
        { data: null, title: 'Name', render: (data, type, row) => row.body || "N/A" },
        {
            data: null,
            title: 'Action',
            render: (data, type, row) => createActionButtons(row, [
                { label: 'Delete', btnClass: 'btn-danger', callback: `delete${controllerName}` }
            ])
        }
    ];

    debugger;

    // Initialize the DataTable with the transformed data
    await initializeDataTable(entitiesList, entitySchema, dataTableId);
};



// Initialize validation
const InitializegetDescriptionvalidation = $(formName).validate({
    onkeyup: function (element) {
        $(element).valid();
    },
    rules: {
        subCatagoryId: {
            required: true,
        },
        formateId: {
            required: true,
        },
        body: {
            required: true,
        }
    },
    messages: {
        Name: {
            required: "Name is required.",
        }
    },
    errorElement: 'div',
    errorPlacement: function (error, element) {
        error.addClass('invalid-feedback');
        element.closest('.form-group').append(error);
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass('is-invalid');
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass('is-invalid');
    }
});

export const CreateDescriptionBtn = async (CreateBtnId) => {
    //Sow Create Model
    $(CreateBtnId).off('click').click(async (e) => {
        e.preventDefault();
        
        resetFormValidation(formName, InitializegetDescriptionvalidation);
        $('#myModalLabelUpdate').hide();
        $('#myModalLabelAdd').show();
        debugger
        showCreateModal(modalCreateId, saveButtonId, updateButtonId);
        await populateDropdown('/SubCategory/GetAll', '#subCatagoryDropdown', 'id', 'name', "Select Tanse");
        await populateDropdown('/SentenceForms/GetAll', '#formateDropdown', 'id', 'name', "Select Format");
        
    });
}
//Save Button


$(saveButtonId).off('click').click(async (e) => {
    e.preventDefault();
    debugger
    try {
        if ($(formName).valid()) {
            const formData = $(formName).serialize();
            const result = await SendRequest({ endpoint: endpointCreate, method: 'POST', data: formData });
            // Clear previous messages
            $('#successMessage').hide();
            $('#UserError').hide();
            $('#EmailError').hide();
            $('#PasswordError').hide();
            $('#GeneralError').hide();
            debugger
            if (result.success && result.status === 201) {
                $(modalCreateId).modal('hide');
                notification({ message: `${controllerName} Created successfully !`, type: "success", title: "Success" });
                await initGitAllList();
            }
        }
    } catch (error) {
        console.error('Error in click handler:', error);
        $(modalCreateId).modal('hide');
        notification({ message: ` ${controllerName} Created failed . Please try again.`, type: "error", title: "Error" });
    }

});




window.updateDescription = async (id) => {
    resetFormValidation(formName, InitializegetDescriptionvalidation);
    clearMessage('successMessage', 'globalErrorMessage');
    debugger
    $('#myModalLabelUpdate').show();
    $('#myModalLabelAdd').hide();
    $(formName)[0].reset();
    await populateDropdown('/SubCategory/GetAll', '#subCatagoryDropdown', 'id', 'name', "Select Sub Category");
    await populateDropdown('/SentenceForms/GetAll', '#formateDropdown', 'id', 'name', "Select Sentance Forms");
    const result = await SendRequest({ endpoint: endpointGetById + id });
    if (result.success) {
        $(saveButtonId).hide();
        $(updateButtonId).show();
        //update and buind Entity Id
        


        $('#body').val(result.data.body);
        $('#formateDropdown').val(result.data.formateId);
        $('#subCatagoryDropdown').val(result.data.subCatagoryId);
        

        $(modalCreateId).modal('show');
        resetValidation(InitializegetDescriptionvalidation, formName);
        $(updateButtonId).off('click').on('click', async (e) => {
            e.preventDefault();
            debugger
            const formData = $(formName).serialize();
            const result = await SendRequest({ endpoint: endpointUpdate + id, method: "PUT", data: formData });
            if (result.success) {
                $(modalCreateId).modal('hide');
                notification({ message: `${controllerName} Updated successfully !`, type: "success", title: "Success" });

                await initGitAllList();
            } else {
                $(modalCreateId).modal('hide');
                notification({ message: `${controllerName} Updated failed . Please try again. !`, type: "error", title: "Error" });
            }
        });
    }
    loger(result);
}

////////window.showDetails = async (id) => {
////////    loger("showDetails id " + id);
////////}




window.deleteDescription = async (id) => {
    clearMessage('successMessage', 'globalErrorMessage');
    debugger;
    $(deleteModelId).modal('show');
    $(deleteBtn).off('click').on('click', async (e) => {
        e.preventDefault();
        debugger;
        const result = await SendRequest({ endpoint: endpointDelete, method: "DELETE", data: { id: id } });

        if (result.success) {
            $(deleteModelId).modal('hide');
            notification({ message: `${controllerName} Deleted successfully !`, type: "success", title: "Success" });
            await initGitAllList();
        } else {
            $(deleteModelId).modal('hide');
            notification({ message: result.detail, type: "error", title: "Error" });
        }
    });
}
