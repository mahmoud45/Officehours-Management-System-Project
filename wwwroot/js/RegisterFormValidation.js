$(document).ready(function(){
    $("input[type='submit']").on("click", function(event){
        event.preventDefault();
        var password = $("input[name='Password']").val();
        var password_span = $("span[data-valmsg-for='Password']");
        
        if(password.search(" ") != -1)
        {
            password_span.html("Password must contains no whitespaces.").css("color", "red");
            return false;
        }else{
            $("#RegistrationForm").submit();
            return true;
        }
    });
});