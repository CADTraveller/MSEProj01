﻿var VerticalEnum = {
    Warehouse: 0,
    Merchandising: 1,
    Membership: 2,
    Distribution: 3,
    International: 4,
    Ancillary: 5,
    eBusiness: 6,
    Corporate: 7,
}
var isLoggedIn = 0;
$(document).ready(function() {
    $('#searchProj').click(function() {
        document.location.href = '#/Search/'+ $('#searchText').val();
    });
    checkLogin();
});
document.getElementById("VerticalList").innerHTML = "";
for (var vertIter in VerticalEnum) {
    document.getElementById("VerticalList").innerHTML += "<li><a href='#ProjectList/" + VerticalEnum[vertIter] + "'>" + vertIter + " Solutions</a></li>";
}

function loginUser() {
    var loginData = {
        provider: 'Google',
        returnURL: ''
    }
    $.post('../Account/ExternalLogin', loginData)
    .then(function (result) {
        console.log(result.data);

    });
}

function logoutUser() {
    alert("logging out user");
    var loginData = {
        __RequestVerificationToken: 'MM5q2ZfVeJVJjjI5Rtw7Y2-ySXDNfiSUNLebsu5CUEawInv6snZFAF6OmM1dp-juQHSTgqZPOEwCKj6U0OyA19d7JxJER_r0ismwhM3uCRqHJs0iSJt6t9-RY20x5e0DfbFehQ_Ncj3uImYqBdC6TQ2'
    }
    $.post('../Account/LogOff', loginData)
    .then(function (result) {
        console.log(result.data);

    });
}

function checkLogin() {
    $.get('../Account/IsLogged', function (result) {
        if (result == "true") {
            document.getElementById("noLogin").style.display = "none";
            document.getElementById("yesLogin").style.display = "block";
            isLoggedIn = 1;
        } else {
            document.getElementById("noLogin").style.display = "block";
            document.getElementById("yesLogin").style.display = "none";
            isLoggedIn = 0;
        }

    });

}