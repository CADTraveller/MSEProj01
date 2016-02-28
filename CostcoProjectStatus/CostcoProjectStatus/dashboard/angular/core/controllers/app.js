var VerticalEnum = {
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
        alert("coool!");
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