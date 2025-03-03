$(document).ready(function () {
    $('#viewtable').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "columnDefs": [
            { "orderable": false, "targets": [12] },
            { "width": "15%", "targets": 12 } // Enough space for buttons
        ],
        "pageLength": 10,
        "language": {
            "search": '<i class="fas fa-search me-2"></i> Search:',
            "lengthMenu": "Show _MENU_ entries",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "emptyTable": "No data available in table",
            "paginate": {
                "previous": '<i class="fas fa-chevron-left"></i>',
                "next": '<i class="fas fa-chevron-right"></i>'
            }
        },
        "order": [[0, 'asc']],
        "dom": '<"row mb-3"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>' +
            '<"row"<"col-sm-12"tr>>' +
            '<"row mt-3"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
        "drawCallback": function () {
            $('.btn-sm').addClass('shadow-sm');
        }
    });
    $("#countryDropdown").change(function () {
        var countryId = $(this).val();
        if (countryId) {
            $.ajax({
                url: '@Url.Action("GetStatesByCountry", "Home")',
                type: "GET",
                data: { countryId: countryId },
                success: function (data) {
                    var stateDropdown = $("#stateDropdown");
                    stateDropdown.empty();
                    stateDropdown.append('<option value="">Select a State</option>');

                    $.each(data, function (index, state) {
                        stateDropdown.append('<option value="' + state.Value + '">' + state.Text + '</option>');
                    });
                }
            });
        } else {
            $("#stateDropdown").empty();
            $("#stateDropdown").append('<option value="">Select a State</option>');
            $("#CityDropdown").empty();
            $("#CityDropdown").append('<option value="">Select a city</option>');

        }
    });

    $("#stateDropdown").change(function () {
        var StateId = $(this).val();
        if (StateId) {
            $.ajax({
                url: '@Url.Action("GetCityByState", "Home")',
                type: "GET",
                data: { StateId: StateId },
                success: function (data) {
                    var CityDropdown = $("#CityDropdown");
                    CityDropdown.empty();
                    CityDropdown.append('<option value="">Select a City</option>');

                    $.each(data, function (index, City) {
                        CityDropdown.append('<option value="' + City.Value + '">' + City.Text + '</option>');
                    });
                }
            });
        } else {
            $("#CityDropdown").empty();
            $("#CityDropdown").append('<option value="">Select a city</option>');
        }
    });

    $.ajax(
        {
            url: '@Url.Action("GetAllCountry", "Home")',
            type: "GET",
            success: function (data) {
                var countryfield = $("#countryDropdown");
                countryfield.empty();
                countryfield.append('<option value="">Select a country</option>');
                $.each(data, function (index, country) {

                    countryfield.append('<option value="' + country.Value + '">' + country.Text + '</option>');
                })

            }
        }
    );


    $("#file").change(function (event) {
        var file = event.target.files[0];

        if (file) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $("#previewImage").attr("src", e.target.result);
                $("#previewImage").show();
            };

            reader.readAsDataURL(file);
        } else {
            $("#previewImage").hide();
        }
    });

    $("#create_user").click(function () {
        $("#CreateModal").modal('show')
    }); 5

    $("#CreateNewUser").on('submit', function (e) {

        e.preventDefault();
        var formData = new FormData(this);


        $.ajax({
            url: '@Url.Action("Index","Home")',
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    alert("user created successfully");
                    window.location.href = '@Url.Action("ViewUser", "Home")';
                }
                else {
                    alert('Error: ' + response.message);
                }
            }

        });

    });

});

function deleteUser(userId) {
    if (confirm("Are you sure you want to delete this user? Whose id is " + userId)) {
        $.ajax({
            url: '@Url.Action("Delete", "Home")',
            type: "POST",
            data: { Id: userId },
            success: function (response) {
                if (response.success) {
                    alert("User deleted successfully whose name is " + response.Name);
                    window.location.reload();
                } else {
                    alert("Error: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("An error occurred: " + error);
            }
        });
    }
}

function edituser(userId) {
    //edit Country dropdown
    $.ajax(
        {
            url: '@Url.Action("GetAllCountry", "Home")',
            type: "GET",
            success: function (data) {
                var countryfield = $("#editcountry");
                countryfield.empty();
                countryfield.append('<option value="">Select a country</option>');
                $.each(data, function (index, country) {

                    countryfield.append('<option value="' + country.Value + '">' + country.Text + '</option>');

                })

            }
        }
    );

    $.ajax({
        url: '@Url.Action("Edit", "Home")',
        type: "POST",
        data: { Id: userId },
        success: function (response) {

            //edit state dropdown
            $.ajax({
                url: '@Url.Action("GetStatesByCountry", "Home")',
                type: "GET",
                data: { countryId: response.message.SelectedCountryId },
                success: function (data) {
                    var stateDropdown = $("#editstate");
                    stateDropdown.empty();
                    stateDropdown.append('<option value="">Select a State</option>');

                    $.each(data, function (index, state) {
                        stateDropdown.append('<option value="' + state.Value + '">' + state.Text + '</option>');
                        if (state.Value.toString() == response.message.SelectedStateId.toString()) {
                            $("#editstate").val(response.message.SelectedStateId);


                        }

                    });
                }
            });

            //edit city dropdown
            $.ajax({
                url: '@Url.Action("GetCityByState", "Home")',
                type: "GET",
                data: { StateId: response.message.SelectedStateId },
                success: function (data) {
                    var CityDropdown = $("#editcity");
                    CityDropdown.empty();
                    CityDropdown.append('<option value="">Select a City</option>');

                    $.each(data, function (index, City) {
                        CityDropdown.append('<option value="' + City.Value + '">' + City.Text + '</option>');

                        if (City.Value.toString() == response.message.SelectedCityId.toString()) {
                            $("#editcity").val(response.message.SelectedCityId);


                        }
                    });
                }
            });

            if (response.success) {
                $("#userId").val(userId);
                $("#editFirstName").val(response.message.FirstName);
                $("#editLastName").val(response.message.LastName);
                $("#editEmail").val(response.message.Email);
                $("#editMobileNo").val(response.message.MobileNo);
                $("#editDob").val(response.message.Dob);

                $(".editGender").each(function () {
                    if ($(this).val().toString() === response.message.Gender1.toString()) {
                        $(this).prop("checked", true);
                    }
                });


                $("#editPassword").val(response.message.Password);
                $("#editConfirmPassword").val(response.message.ConfirmPassword);
                $("#editAddress").val(response.message.Address);
                $("#editcountry").val(response.message.SelectedCountryId);

                $("#editstate").val(response.message.SelectedStateId);
                $("#editCity").val(response.message.SelectedCityId);

                $("#edit_image").attr("src", "/Uploads/" + response.message.ImagePath.toString());



                $("#user_id").val(response.message.user_id)


                $("#editUserModal").modal("show");
            } else {
                alert("Error: " + response.message);
            }
        },
        error: function (xhr, status, error) {
            alert("An error occurred: " + error);
        }
    });

    //edit country changing event

    $("#editcountry").change(function () {
        var countryId = $(this).val();
        if (countryId) {
            $.ajax({
                url: '@Url.Action("GetStatesByCountry", "Home")',
                type: "GET",
                data: { countryId: countryId },
                success: function (data) {
                    var stateDropdown = $("#editstate");
                    stateDropdown.empty();
                    stateDropdown.append('<option value="">Select a State</option>');

                    $.each(data, function (index, state) {
                        stateDropdown.append('<option value="' + state.Value + '">' + state.Text + '</option>');
                    });
                }
            });
        } else {
            $("#editstate").empty();
            $("#editstate").append('<option value="">Select a State</option>');
            $("#editcity").empty();
            $("#editcity").append('<option value="">Select a city</option>');

        }
    });

    //edit state changing event

    $("#editstate").change(function () {
        var StateId = $(this).val();
        if (StateId) {
            $.ajax({
                url: '@Url.Action("GetCityByState", "Home")',
                type: "GET",
                data: { StateId: StateId },
                success: function (data) {
                    var CityDropdown = $("#editcity");
                    CityDropdown.empty();
                    CityDropdown.append('<option value="">Select a City</option>');

                    $.each(data, function (index, City) {
                        CityDropdown.append('<option value="' + City.Value + '">' + City.Text + '</option>');
                    });
                }
            });
        } else {
            $("#editcity").empty();
            $("#editcity").append('<option value="">Select a city</option>');
        }
    });

    //edit file
    $("#editfile").change(function (event) {

        var file = event.target.files[0];

        if (file) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $("#edit_image").attr("src", e.target.result);
                $("#edit_image").show();
                $('#edit_image').prop('required', true);
            };

            reader.readAsDataURL(file);
        } else {
            $("#edit_image").hide();
            $('#edit_image').prop('required', false);
        }
    });

    $(document).ready(function () {

        $("#EditUserForm").on('submit', function (e) {

            e.preventDefault();
            var formData = new FormData(this);

            $.ajax({
                url: '@Url.Action("EditUser", "Home")',
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.success) {
                        alert("user Edited successfully");
                        window.location.href = '@Url.Action("ViewUser", "Home")';
                    }
                    else {
                        alert(response.message);
                    }
                }

            });

        });

    });
}

function detailsUser(userId) {
    $.ajax({
        url: '@Url.Action("Details", "Home")',
        type: "POST",
        data: { Id: userId },
        dataType: "json", // Ensure JSON response
        success: function (response) {
            // Check if response has the expected structure
            if (response && response.message) {
                var user = response.message;

                // Update image
                $("#dImage").attr("src", user.ImagePath ? "/Uploads/" + user.ImagePath : "/images/default-avatar.png");

                // Update text content
                $("#fullname").text(user.FirstName + " " + user.LastName);
                $("#dEmail").text(user.Email);
                $("#Mobile").text(user.MobileNo);
                $("#Gender").text(user.Gender1);
                $("#DDob").text(user.Dob);
                $("#DAddress").text(user.Address);
                $("#Country").text(user.SelectedCountry);
                $("#State").text(user.selectedState);
                $("#City").text(user.selectedCity);
                $("#user_id").text(user.user_id);

                // Update Edit/Delete links with userId
                $("#editLink").attr("href", '@Url.Action("Edit", "Home")' + '?id=' + userId);
                $("#deleteLink").attr("href", '@Url.Action("Delete", "Home")' + '?id=' + userId);

                // Show modal
                $("#detail").modal("show");
            } else {
                console.error("Invalid response:", response);
                alert("Failed to load user details.");
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX error:", status, error);
            alert("An error occurred while fetching details.");
        }
    });
}

function deleteUserFromHidden() {
    var userId = $("#user_id").val();

    console.log("User ID from hidden field:", userId);

    if (!userId) {
        alert("User ID is not set. Cannot delete.");
        return;
    }


    try {
        deleteUser(userId);
    } catch (error) {
        console.error("Error in deleteUser:", error);
        alert("An error occurred while trying to delete the user.");
    }
}