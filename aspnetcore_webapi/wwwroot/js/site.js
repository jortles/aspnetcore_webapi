// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

function myFetch(url, method, data = {}) {
    var requestInfo = {
        method: method,
        headers: { 'Content-Type': 'application/json' },
        cache: 'no-cache',
        credentials: 'same-origin',
        redirect: 'manual'
    };
    if (method == 'POST' || method == 'PUT') {
        requestInfo.body = JSON.stringify(data);
    }

    return fetch(url, requestInfo);
}

document.getElementById("signout").addEventListener("click", e => {
    document.location.href = '/';
});


document.getElementById("registeradminButton").addEventListener("click", e => {
    var registerStatus = document.getElementById('registerStatus');
    registerStatus.innerText = "";
    var firstName = document.getElementById("firstName").value.trim();
    var lastName = document.getElementById("lastName").value.trim();
    var email = document.getElementById("email").value.trim();
    var username = document.getElementById("username").value.trim();
    var password = document.getElementById("password").value;
    var confirmPassword = document.getElementById("confirmPassword").value;
    if (password != confirmPassword) {
        registerStatus.innerText = "Password and Confirm Password Did Not Match";
        return;
    }

    data = { 'firstname': firstName, 'lastname': lastName, 'email': email, 'username': username, 'password': password, 'confirmpassword': confirmPassword };
    myFetch('/api/v1/RegisterAdmin', 'PUT', data)
        .then(response => {
            if (!response.ok) {
                throw new Error(response.status);
            }
        })
        .then(data => {
            alert('Registration was successful! Please log in.');
            document.location.href = "/";
        })
        .catch(error => {
            console.log(error);
            registerStatus.innerText = "Registration Failed";
            document.location.href = "#registerStatus";
        });
});

document.getElementById("loginButton").addEventListener("click", e => {
    document.getElementById('loginStatus').innerText = "";
    var email = document.getElementById("loginEmail").value;
    var password = document.getElementById("loginPassword").value;
    data = { 'email': email, 'password': password };
    myFetch('/api/v1/Login', 'POST', data)
        .then(response => {
            if (!response.ok) {
                throw new Error(response.status);
            }
        })
        .then(data => {
            document.getElementById('loginButton').classList.add('d-none');
            document.getElementById('confirmTwoFactorButton').classList.remove('d-none');
            document.getElementById('loginUsernamePasswordContainer').classList.add('d-none');
            document.getElementById('loginTwoFactorFormGroup').classList.remove('d-none');
            document.getElementById('forgotpButton').classList.add('d-none');
        })
        .catch(error => {
            console.log(error);
            document.getElementById('loginStatus').innerText = "Login Failed";
        });
});

document.getElementById("confirmTwoFactorButton").addEventListener("click", e => {
    document.getElementById('loginStatus').innerText = "";
    var email = document.getElementById("loginEmail").value;
    var password = document.getElementById("loginPassword").value;
    var code = document.getElementById('loginTwoFactorValue').value;
    data = { 'email': email, 'password': password, 'TwoFactorValue': code };
    myFetch('/api/v1/TwoFactor', 'POST', data)
        .then(response => {
            if (response.ok) {

                return response.json();
            }
            else
                throw new Error(response.status);
        })
        .then(data => {
            if (username == "admin") {
                document.location.href = '/Administration';
            }
            else {
                document.location.href = '/Home/Profile';
            }

        })
        .catch(error => {
            console.log(error);
            document.getElementById('loginStatus').innerText = "Login Failed";
        });
});