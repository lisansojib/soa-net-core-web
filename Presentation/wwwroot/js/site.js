var rootPath;

$(function () {
    rootPath = window.location.protocol + '//' + window.location.host;

    toastr.options.escapeHtml = true;
    axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
    debugger;
    if (localStorage.getItem("token")) {
        axios.defaults.headers.common['Authorization'] = "Bearer " + localStorage.getItem("token");
    }

    loadProgressBar();
})