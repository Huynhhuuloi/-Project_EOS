
$(document).ready(function () {
    $('#cancelBtn').click(function () {
        $('#addEmployeeForm').addClass('hidden');
        $('#showFormBtn').removeClass('hidden');
    });

    // Thêm sự kiện cho nút "Save"
    $('#saveBtn').click(function () {
        saveEmployee();
    });
    $('#editButton').click(function () {
        var employeeID = $(this).data('employee-id');
        console.log('Employee ID:', employeeID); // In ra ID của nhân viên để kiểm tra
        // Cho phép chỉnh sửa các trường Department, Service, Designation, và Salary
        $('#editDepartment').prop('disabled', false);
        $('#editService').prop('disabled', false);
        $('#editDesignation').prop('disabled', false);
        $('#editSalary').prop('disabled', false);
        $(this).hide();
        $('#saveEditBtn').show();
        $('#cancelEditBtn').show();

    });

    $('#saveEditBtn').click(function () {
        saveEditEmployee();
    });
    $('#cancelEditBtn').click(function () {
        // Vô hiệu hóa các trường chỉnh sửa
        $('#editDepartment').prop('disabled', true);
        $('#editService').prop('disabled', true);
        $('#editDesignation').prop('disabled', true);
        $('#editSalary').prop('disabled', true);
        $('#addEmployeeForm').hide();

        $('#editButton').show();
        $('#saveEditBtn').hide();
        $('#cancelEditBtn').hide();
    });
});
function showCreateForm() {
    $('#addEmployeeForm').show();
    $('#showFormBtn').hide();
}

function cancelCreate() {
    $('#addEmployeeForm').hide();
    $('#showFormBtn').show();
}

function isValidEmail(email) {
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

function saveNewEmployee() {
    var employeename = $('#createEmployeeName').val();
    var department = $('#createDepartmentName').val();
    var service = $('#createServiceName').val();
    var designation = $('#createDesignation').val();
    var email = $('#createEmail').val();
    var salary = $('#createSalary').val();
    var joindate = $('#createJoinDate').val();
    var active = $('#createActive').val() || "false"; // Nếu #createActive không có giá trị, mặc định là "false"

    // In ra console các giá trị
    console.log('EmployeeName:', employeename);
    console.log('DepartmentID:', department);
    console.log('ServiceID:', service);
    console.log('Designation:', designation);
    console.log('Email:', email);
    console.log('Salary:', salary);
    console.log('JoinDate:', joindate);
    console.log('Is_Active:', active);

    $.ajax({
        url: '/Admin/EmployeesManage/AddEmployee',
        type: 'POST',
        data: {
            EmployeeName: employeename,
            DepartmentID: department,
            ServiceID: service,
            Designation: designation,
            Email: email,
            Salary: salary,
            JoinDate: joindate,
            Is_Active: active
        },
        success: function (response) {
            if (response.success) {
                alert('Employee added successfully');
                reloadAllEmployees();
                clearFormFields();
            } else {
                alert('Failed to add employee: ' + (response.message || 'Unknown error'));
            }
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText); // In ra lỗi chi tiết trong console log
            alert("An error occurred while adding the employee. Check console log for details.");
        }
    });
}


function reloadAllEmployees() {
    window.location.href = '/Admin/EmployeesManage'; // Load lại trang
}

function clearFormFields() {
    $('#createEmployeeName').val('');
    $('#createDepartmentName').val('');
    $('#createServiceName').val('');
    $('#createDesignation').val('');
    $('#createEmail').val('');
    $('#createSalary').val('');
    $('#createJoinDate').val('');
    $('#createActive').prop('checked', false);
}



function saveEditEmployee() {
    var employeeID = $('#editButton').data('employee-id');

    var department = $('#editDepartment').val();
    var service = $('#editService').val();
    var designation = $('#editDesignation').val();
    var salary = parseFloat($('#editSalary').val());

    // Debug
    console.log('Employee ID:', employeeID);
    console.log('Department:', department);
    console.log('Service:', service);
    console.log('Designation:', designation);
    console.log('Salary:', salary);





    // Gửi dữ liệu lên máy chủ thông qua Ajax
    $.ajax({
        url: '/Admin/EmployeesManage/EditEmployee',
        type: 'POST',
        data: {
            EmployeeID: employeeID,
            DepartmentID: department,
            ServiceID: service,
            Designation: designation,
            Salary: salary
        },
        success: function (response) {
            if (response.success) {
                // Hiển thị thông báo khi lưu thành công
                alert('Employee information updated successfully');
                // Reload trang hoặc thực hiện các hành động cần thiết
                location.reload();
                $('#addEmployeeForm').hide();
            } else {
                // Hiển thị thông báo khi lưu thất bại
                alert('Failed to update employee information: ' + (response.message || 'Unknown error'));
            }
        },
        error: function (xhr, status, error) {
            // Hiển thị thông báo khi có lỗi xảy ra trong quá trình gửi dữ liệu lên máy chủ
            console.error(xhr.responseText);
            alert("An error occurred while updating employee information. Check console log for details.");
        }
    });
}
