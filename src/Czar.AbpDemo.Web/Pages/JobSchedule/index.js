$(function () {
    var l = abp.localization.getResource('AbpDemo');

    var createModal = new abp.ModalManager(abp.appPath + 'JobSchedule/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'JobSchedule/EditModal');
    var dataTable = $('#JobInfoTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(czar.abpDemo.jobSchedule.jobInfo.getList),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l('ActionsEdit'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('ActionsDelete'),
                                confirmMessage: function (data) {
                                    return l('ActionsDeletionConfirmationMessage', data.record.name);
                                },
                                action: function (data) {
                                    czar.abpDemo.bookStore.book
                                        .delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l('ActionsSuccessfullyDeleted'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            },
            { data: "jobGroup" },
            { data: "jobName" },
            { data: "jobDescription" },
            { data: "jobStatus" },
            { data: "cronExpress" },
            { data: "starTime" },
            { data: "endTime" },
            { data: "nextTime" }
        ]
    }));


    editModal.onResult(function () {
        dataTable.ajax.reload();
    });
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });
    $("#NewJobInfoButton").click(function (e) {
        console.log("NewJobInfoButton");
        e.preventDefault();
        createModal.open();
    });
});

