(function () {
    $(function () {
        $("#btn-register").on('click', function (e) {
            e.preventDefault();
            var btnLogin = $(this);
            var originalText = btnLogin.html();
            setLoadingButton(btnLogin);

            var $formEl = $("#register-form");

            initializeValidation($formEl, validationConstraints);
            if (!isValidForm($formEl, validationConstraints)) {
                toastr.error("Please correct all valiation errors.");
                resetLoadingButton(btnLogin, originalText);
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
                .catch(function () {
                    toastr.error("An error occured!");
                })
                .then(function () {
                    resetLoadingButton($btnUpdateProfilePic, btnContent);
                });
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
})();