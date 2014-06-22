window.AddComm = AddComm = () ->
            alert "wow"
value = document.getElementById("comment").value
if value is ""
alert "You must write a comment"
return
comment =
    message: value
offerId: @Model.Id

$.post "/OfferPage/AddComment", comment, (data) ->
    $("#commentsList").append data
return