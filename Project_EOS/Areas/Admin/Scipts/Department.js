function showCreateForm() {
    document.getElementById('createForm').style.display = 'block';
}

function cancelCreate() {
    document.getElementById('createForm').style.display = 'none';
}

function saveNewDepartment() {
    var name = $('#createName').val();
    var description = $('#createDescription').val();

    $.ajax({
        url: '/Admin/DepartmentManage/AddDepartment',
        type: 'POST',
        data: {
            DepartmentName: name,
            DepartmentDescription: description
        },
        success: function (response) {
            if (response.success) {
                alert('Department added successfully');

                // Clear the form and hide it
                document.getElementById('createForm').style.display = 'none';
                $('#createName').val('');
                $('#createDescription').val('');

                // Append new row to the table with the newly created department data
                var newRow = `<tr>
                                <td></td>
                                <td><a href="/Admin/DepartmentManage/AllEmployeesByDepartment?departmentID=${response.departmentID}&departmentName=${response.departmentName}">${response.departmentName}</a></td>
                                <td><span class="department-description">${response.departmentDescription}</span></td>
                                <td>
                                    <button class="edit-button">Edit</button>
                                    <button class="update-button" style="display:none;" data-id="${response.departmentID}">Update</button>
                                    <button class="cancel-button" style="display:none;">Cancel</button>
                                </td>
                              </tr>`;
                $('#numbered-table tbody').append(newRow);

            } else {
                alert('Failed to add department');
            }
        },
        error: function () {
            alert('An error occurred while adding the department');
        }
    });
}



$(document).ready(function () {
    

    $('.edit-button').click(function () {
        var row = $(this).closest('tr');
        row.find('span').hide();
        row.find('input').show();
        row.find('.edit-button').hide();
        row.find('.update-button').show();
        row.find('.cancel-button').show();
    });

    $('.cancel-button').click(function () {
        var row = $(this).closest('tr');
        row.find('span').show();
        row.find('input').hide();
        row.find('.edit-button').show();
        row.find('.update-button').hide();
        row.find('.cancel-button').hide();
    });

    $('.update-button').click(function () {
        var row = $(this).closest('tr');
        var departmentID = $(this).data('id');
        var departmentDescription = row.find('.edit-department-description').val();

        $.ajax({
            url: '/Admin/DepartmentManage/UpdateDepartment',
            type: 'POST',
            data: {
                departmentID: departmentID,
                departmentDescription: departmentDescription,
            },
            success: function (response) {
                if (response.success) {
                    row.find('.department-description').text(departmentDescription);
                    row.find('span').show();
                    row.find('input').hide();
                    row.find('.edit-button').show();
                    row.find('.update-button').hide();
                    row.find('.cancel-button').hide();
                } else {
                    alert('Failed to update department');
                }
            }
        });
    });
});
