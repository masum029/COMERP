import { SendRequest, populateDropdown } from '../comerp_Utility/comerp_SendRequestUtility.js';
import { createActionButtons, hideLoader, initializeDataTable, resetFormValidation, resetValidation, showCreateModal, showLoader } from '../comerp_Utility/comerp_helpers.js';
import { notification, notificationCatch, notificationErrors } from '../comerp_Utility/comerp_notification.js';

// Dynamically define controller and form elements
const controllerName = "AboutApi";
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
    await CreateAboutApiBtn(createButton);
});
const initGitAllList = async () => {
    await getAboutApiList();
}


// Dynamically fetch data for verbs or other entities
const getAboutApiList = async () => {
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
                name: entity.name,
                des: entity.description,
                date: entity.establishedDate ? entity.establishedDate.split('T')[0] : null,
                email: entity.contactEmail,
                phone: entity.phone,
                address: entity.address,
                website: entity.website,
                logoUrl: entity.logo,
                status: entity.isActive

            };
        }
        return null;
    }).filter(Boolean);

    debugger;

    // Define the table schema
    const entitySchema = [
        { data: null, title: 'Logo', render: (data, type, row) => `<img src="images/logo/${row?.logoUrl}" alt="User Avatar" class="rounded-circle" style="width: 30px; height: 30px; object-fit: cover;" onerror="this.onerror=null;this.src='/ProjectRootImg/default-user.png';" />` },
        { data: null, title: 'Name', render: (data, type, row) => row.name || "Null" },
        { data: null, title: 'Email', render: (data, type, row) => row.email || "Null" },
        { data: null, title: 'Phone', render: (data, type, row) => row.phone || "Null" },
        { data: null, title: 'Website', render: (data, type, row) => row.website || "Null" },
        {
            data: null, title: 'Status', render: (data, type, row) => row.status ? '<span style="color: green; font-weight: bold;">Active</span>'
                : '<span style="color: red; font-weight: bold;">Inactive</span>'
        },
        { data: null, title: 'Date', render: (data, type, row) => row.date || "Null" },
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
const InitializegetAboutApivalidation = $(formName).validate({
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
        AboutApiName: {
            required: true,

        },
        Email: {
            required: true,

        },
        Phone: {
            required: true
        },
        Password: {
            required: true,
            minlength: 6,

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
export const CreateAboutApiBtn = async (CreateBtnId) => {
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

// Save button click event
$(saveButtonId).off('click').click(async (e) => {
    e.preventDefault();
    debugger;
    try {
        // Trigger form validation before submission
        if ($(formName).valid()) {
            //const formData = $(formName).serialize();
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
window.updateAboutApi = async (id) => {
    try {
        resetFormValidation(formName, $(formName).valid());
        $(updateLabel).show();
        $(addLabel).hide();
        $(formName)[0].reset();

        // Populate the dropdown
        await populateDropdown('/Company/Getall', '#CompanyDropdown', 'id', 'name', "Select Company");

        const result = await SendRequest({ endpoint: endpointGetById + id });
        if (result.success) {
            const data = result.data;

            $(saveButtonId).hide();
            $(updateButtonId).show();

            // Populate main About fields
            $('#Title').val(data.title);
            $('#Description').val(data.description);
            $('#FormFile').val(data.formFile);
            $('#CompanyDropdown').val(data.companyId).change();
            $('#IsVisible').prop('checked', data.isVisible);

            // Clear existing SubAbout and SubTopic fields
            $('#subAboutContainer').empty();

            // Populate SubAbout and SubTopics dynamically
            if (data.subAbouts && data.subAbouts.length > 0) {
                data.subAbouts.forEach((subAbout, subAboutIndex) => {
                    const subAboutHtml = `
                        <div class="subAboutItem row g-3" data-index="${subAboutIndex}">
                            <div class="col-md-12 d-flex justify-content-end mt-3">
                                    <button type="button" class="btn btn-danger btnRemoveSubAbout" style="border-radius: 50%; width: 40px; height: 40px; padding: 0;">
                                        <i class="bi bi-trash"></i>
                                    </button>
                                </div>
                            <div class="col-md-6">
                                <label class="form-label">Sub About Title <span class="text-danger">*</span></label>
                                <input type="text" name="SubAbouts[${subAboutIndex}].Title" value="${subAbout.title}" class="form-control" placeholder="Enter Sub About Title">
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Icon</label>
                                <input type="text" name="SubAbouts[${subAboutIndex}].Icon" value="${subAbout.icon}" class="form-control" placeholder="Enter Icon URL">
                            </div>
                            <div class="col-md-12">
                                <label class="form-label">Sub About Description</label>
                                <textarea name="SubAbouts[${subAboutIndex}].Description" class="form-control" placeholder="Enter Sub About Description">${subAbout.description}</textarea>
                            </div>
                            <div class="col-md-12">
                                <button type="button" class="btn btn-secondary btnAddSubTopic" data-index="${subAboutIndex}">Add Sub Topic</button>
                            </div>
                            <div class="col-md-12 subTopicsContainer" data-index="${subAboutIndex}">
                                <!-- SubTopics will be dynamically added here -->
                            </div>
                            

                        </div>
                        `;
                    $('#subAboutContainer').append(subAboutHtml);

                    // Populate SubTopics for this SubAbout
                    if (subAbout.aboutTopics && subAbout.aboutTopics.length > 0) {
                        subAbout.aboutTopics.forEach((topic, topicIndex) => {
                            const subTopicHtml = `
                                <div class="subTopicItem">
                                    <div class="row g-3">
                                        <div class="col-md-6">
                                            <label class="form-label">Sub Topic Title</label>
                                            <input type="text" name="SubAbouts[${subAboutIndex}].AboutTopics[${topicIndex}].Title" value="${topic.title}" class="form-control" placeholder="Enter Sub Topic Title">
                                        </div>
                                        <div class="col-md-6">
                                            <label class="form-label">Icon</label>
                                            <input type="text" name="SubAbouts[${subAboutIndex}].AboutTopics[${topicIndex}].Icon" value="${topic.icon}" class="form-control" placeholder="Enter Icon">
                                        </div>
                                        <div class="col-md-12">
                                            <label class="form-label">Description</label>
                                            <textarea name="SubAbouts[${subAboutIndex}].AboutTopics[${topicIndex}].Description" class="form-control" placeholder="Enter Sub Topic Description">${topic.description}</textarea>
                                        </div>
                                        <div class="col-md-12 d-flex justify-content-end mt-3">
                                            <button type="button" class="btn btn-danger btnRemoveSubTopic" style="border-radius: 50%; width: 40px; height: 40px; padding: 0; display: flex; align-items: center; justify-content: center;">
                                                <i class="bi bi-trash"></i>
                                            </button>
                                        </div>

                                    </div>
                                </div>`;
                            $(`.subTopicsContainer[data-index="${subAboutIndex}"]`).append(subTopicHtml);
                        });
                    }
                });
            }

            $(modalCreateId).modal('show');
            resetValidation(InitializegetAboutApivalidation, formName);

            $(updateButtonId).off('click').on('click', async (e) => {
                e.preventDefault();
                const formData = new FormData($(formName)[0]);
                const result = await SendRequest({ endpoint: endpointUpdate + id, method: "PUT", data: formData });
                if (result.success) {
                    notification({ message: `${controllerName} Updated successfully!`, type: "success", title: "Success" });
                    $(modalCreateId).modal('hide');
                    await initGitAllList();
                } else {
                    notificationErrors({ message: result.detail, title: `Error: Failed to update ${controllerName}` });
                    $(modalCreateId).modal('hide');
                }
            });
        }
    } catch (error) {
        notificationCatch({ message: error, title: `Error: Failed to update ${controllerName}` });
    }
};

$(document).ready(function () {
    let subAboutIndex = 0;

    // Add SubAbout
    $("#btnAddSubAbout").click(function () {
        const subAboutHtml = `
            <div class="subAboutItem row g-3" data-index="${subAboutIndex}">
                <div class="col-md-6">
                    <label class="form-label">Sub About Title <span class="text-danger">*</span></label>
                    <input type="text" name="SubAbouts[${subAboutIndex}].Title" class="form-control" placeholder="Enter Sub About Title">
                </div>
                <div class="col-md-6">
                    <label class="form-label">Icon</label>
                    <input type="text" name="SubAbouts[${subAboutIndex}].Icon" class="form-control" placeholder="Enter Icon URL">
                </div>
                <div class="col-md-12">
                    <label class="form-label">Sub About Description</label>
                    <textarea name="SubAbouts[${subAboutIndex}].Description" class="form-control" placeholder="Enter Sub About Description"></textarea>
                </div>
                <div class="col-md-12">
                    <button type="button" class="btn btn-secondary btnAddSubTopic" data-index="${subAboutIndex}">Add Sub Topic</button>
                </div>
                <div class="col-md-12 subTopicsContainer" data-index="${subAboutIndex}">
                    <!-- SubTopics will be dynamically added here -->
                </div>
                <div class="col-md-12 d-flex justify-content-end mt-3">
                                    <button type="button" class="btn btn-danger btnRemoveSubAbout" style="border-radius: 50%; width: 40px; height: 40px; padding: 0;">
                                        <i class="bi bi-trash"></i>
                                    </button>
                                </div>
            </div>`;
        $("#subAboutContainer").append(subAboutHtml);
        subAboutIndex++;
    });

    // Remove SubAbout
    $(document).on("click", ".btnRemoveSubAbout", function () {
        $(this).closest(".subAboutItem").remove();
    });

    // Add SubTopic
    $(document).on("click", ".btnAddSubTopic", function () {
        const subAboutIndex = $(this).data("index");
        const subTopicsContainer = $(`.subTopicsContainer[data-index="${subAboutIndex}"]`);
        const subTopicIndex = subTopicsContainer.children().length;
        const subTopicHtml = `
            <div class="subTopicItem">
                <div class="row g-3">
                    <div class="col-md-6">
                        <label class="form-label">Sub Topic Title</label>
                        <input type="text" name="SubAbouts[${subAboutIndex}].AboutTopics[${subTopicIndex}].Title" class="form-control" placeholder="Enter Sub Topic Title">
                    </div>
                    <div class="col-md-6">
                        <label class="form-label">Icon</label>
                        <input type="text" name="SubAbouts[${subAboutIndex}].AboutTopics[${subTopicIndex}].Icon" class="form-control" placeholder="Enter Icon">
                    </div>
                    <div class="col-md-12">
                        <label class="form-label">Description</label>
                        <textarea name="SubAbouts[${subAboutIndex}].AboutTopics[${subTopicIndex}].Description" class="form-control" placeholder="Enter Sub Topic Description"></textarea>
                    </div>
                    <div class="col-md-12 d-flex justify-content-end mt-3">
                                            <button type="button" class="btn btn-danger btnRemoveSubTopic" style="border-radius: 50%; width: 40px; height: 40px; padding: 0; display: flex; align-items: center; justify-content: center;">
                                                <i class="bi bi-trash"></i>
                                            </button>
                                        </div>
                </div>
            </div>`;
        subTopicsContainer.append(subTopicHtml);
    });

    // Remove SubTopic
    $(document).on("click", ".btnRemoveSubTopic", function () {
        $(this).closest(".subTopicItem").remove();
    });
});
////////window.showDetails = async (id) => {
////////    loger("showDetails id " + id);
////////}

window.deleteAboutApi = async (id) => {
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




