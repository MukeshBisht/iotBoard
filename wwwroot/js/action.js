function addTask(id) {
    
    if(id == -1){
        id = 5000;
    }

    $.ajax({
        type: "POST",
        url: "/api/Action/" + id,
        cache: false,
        success: function(data){
            alert(data + " Request is in Queue");
        }
      });
}