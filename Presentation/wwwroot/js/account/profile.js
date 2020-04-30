(function () {
    var $btnChangePhoto,
        $profilePic,
        $btnUpdateProfilePic;

    $(function () {
        $btnChangePhoto = $("#btn-change-photo");
        $profilePic = $("#ProfilePic");
        $btnUpdateProfilePic = $("#btn-update-profile-pic");

        $btnChangePhoto.on("click", function (evt) {
            evt.preventDefault();
            $profilePic.trigger("click");
        })

        $profilePic.on("change", function (evt) {
            evt.preventDefault();
            previewImage(evt, "preview-profile-pic");
            enableElement($btnUpdateProfilePic);
        });

        $btnUpdateProfilePic.on("click", function (evt) {
            evt.preventDefault();
            debugger;

            var profilePic = $("#ProfilePic")[0].files[0];
            if (!profilePic) return;

            var btnContent = $btnUpdateProfilePic.html();
            setLoadingButton($btnUpdateProfilePic);

            var formData = new FormData();
            formData.append("profilePic", profilePic);

            const config = {
                headers: {
                    'content-type': 'multipart/form-data',
                    'Authorization': "Bearer " + localStorage.getItem("token")
                }
            }

            axios.post("/api/manage/upload-photo", formData, config)
                .then(function () {
                    toastr.success("Profile picture uploaded successfully!");
                    disableElement($btnUpdateProfilePic);
                })
                .catch(function (err) {
                    console.error(err);
                    toastr.error("An error occured, please try again.");
                })
                .then(function () {
                    resetLoadingButton($btnUpdateProfilePic, btnContent);
                });
        })
    })
})();