$(document).ready(function(){
    $("#EditProfileForm").on("submit", function(event){
        event.preventDefault();

        var Email = $("input[name='Email']").val();
        var FullName = $("input[name='FullName']").val();
        var UserName = $("input[name='UserName']").val();
        var token = $("input[name='__RequestVerificationToken']").val();
        
        $.ajax({
            type: 'POST',
            url: window.location.href,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({Email: Email, FullName: FullName, UserName: UserName}),
            headers:{RequestVerificationToken : token},
            success: function(data){
                if(data.success)
                {
                    $("#msg").html("Data was saved successfully.").css("color", "green");
                }
                else{
                    $("#msg").html(data.error).css("color", "red");
                } 
            },
            error: function(error){ $("#msg").html("error occured").css("color", "red"); } 
        });
    })
})