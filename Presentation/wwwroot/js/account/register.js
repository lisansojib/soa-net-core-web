$(function () {
    loadProgressBar();

    $("#btn-register").on('click', function (e) {
        e.preventDefault();
        var btnLogin = $(this);
        var originalText = btnLogin.html();
        disableButton(btnLogin);

        var $formEl = $("#register-form");

        initializeValidation($formEl, validationConstraints);
        if (!isValidForm($formEl, validationConstraints)) {
            toastr.error("Please correct all valiation errors.");
            enableButton(btnLogin, originalText);
            return;
        }
        else {
            hideValidationErrors($formEl);
        }

        var data = formDataToJson($formEl);

        axios.post('/api/auth/register', data)
            .then(function () {
                toastr.success("Registration successful!");
                window.location.href = "/account/login";
            })
            .catch(function (err) {
                toastr.error("An error occured!");
                enableButton(btnLogin, originalText);       
            });
    });

    var validationConstraints = {
        Email: {
            presence: true,
            email: true
        },
        Password: {
            presence: true,
            length: {
                minimum: 6,
                maximum: 20
            }
        }
    };
})