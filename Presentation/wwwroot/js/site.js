$(function () {
    toastr.options.escapeHtml = true;
    axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';

    if (localStorage.getItem("token")) {
        axios.defaults.headers.common['Authorization'] = "Bearer " + localStorage.getItem("token");
    }

    loadProgressBar();
})