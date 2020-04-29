(function () {
    $(function () {
        $("#btn-login").on('click', function (e) {
            e.preventDefault();

            var btnLogin = $(this);
            var originalText = btnLogin.html();
            disableButton(btnLogin);

            var $formEl = $("#login-form");

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
            data.RememberMe = convertToBoolean(data.RememberMe);

            axios.post('/api/auth/login', data)
                .then(function (response) {
                    toastr.success("Login successful!");
                    localStorage.setItem("token", response.data.access_token);
                    window.location.href = "/account/profile";
                })
                .catch(function (err) {
                    toastr.error("Invalid username or password!");
                    enableButton(btnLogin, originalText);
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