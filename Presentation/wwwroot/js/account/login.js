(function () {
    $(function () {
        $("#btn-login").on('click', function (e) {
            e.preventDefault();

            var btnLogin = $(this);
            var originalText = btnLogin.html();
            setLoadingButton(btnLogin);

            var $formEl = $("#login-form");

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
            data.RememberMe = convertToBoolean(data.RememberMe);

            axios.post('/api/auth/login', data)
                .then(function (response) {
                    toastr.success("Login successful!");
                    localStorage.setItem("token", response.data);
                    window.location.href = "/account/profile";
                })
                .catch(function () {
                    toastr.error("Invalid username or password!");
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