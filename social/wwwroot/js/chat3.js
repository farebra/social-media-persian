
//Initialize SignalR
var connection = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub3')
    .build();
connection.on('ReciveMessage2', renderMessage2);
/*connection.on('Recive2Message2', render2Message2);*/
connection.on('Recive3Message2', render3Message2);
connection.on('Recive4Message2', render4Message2);


connection.start();

connection.onclose(() => {
    setTimeout(function () {
        connection.start();
    }, 2500);
})


function ready() {
    var chatForm = document.getElementById('chatForm');
    var user = $("#rTextBox").val();
    var groupid = $("#groupid").val();
   /* var user2 = $("#xTextBox").val();*/
    chatForm.addEventListener('submit',
        function (e) {
            e.preventDefault();
            var text = e.target[0].value;
            e.target[0].value = '';
            $(".emojionearea-editor").html('');

            sendMessage2(user, text,groupid);
            //console.log(user, text);

        });
    //////////////////////////////////////////////video
    $("#file2").change(function () {

        var user = $("#rTextBox").val();
        localStorage.setItem("userr", user);
        var groupid = $("#groupid").val();
        

        ////////////////////////////
        var file_data = $("#file").prop("files")[0]; // Getting the properties of file from file field
        var form_data = new FormData(); // Creating object of FormData class
        form_data.append("file", file_data) // Appending parameter named file with properties of file_field to form_data
        $.ajax({
            url: "/home/chatfile2", // Upload Script
            dataType: 'script',
            cache: false,
            contentType: false,
            processData: false,
            data: form_data, // Setting the data attribute of ajax with file_data
            type: 'post',
            success: function (data) {
                var json = JSON.parse(data);
                var cookieuser = localStorage.getItem("userr");
                sendFile(cookieuser, json["filename"], json["prefix"],groupid);
            }
        });

    });
    /////////////////////////voice

    var cnt = 1;
    $(".btnsendvoice").click(function () {
        cnt++;
        if (cnt % 2 == 0) {
            recorder && recorder.record();
            $(".btnsendvoice").css({ "color": "red" });
        }
        else {
            $(".btnsendvoice").css({ "color": "#69c1fb" });

            recorder && recorder.stop();

            createDownloadLink();

            recorder && recorder.clear();
        }
    })

    function createDownloadLink() {
        var user = $("#rTextBox").val();
        var groupid = $("#groupid").val();

        localStorage.setItem("userr", user);
   
        recorder && recorder.exportWAV(function (blob) {

            var randLetter = String.fromCharCode(65 + Math.floor(Math.random() * 26));
            var uniqid = randLetter + Date.now();
            var filename = uniqid + ".wav ";

            var xhr = new XMLHttpRequest();
            xhr.onload = function (e) {
                if (this.readyState === 4) {
                    console.log("Server returned: ", e.target.responseText);
                }
            };

            var fd = new FormData();
            fd.append("file", blob, filename);
            xhr.open("POST", "/home/voicefile2", true);
            xhr.send(fd);
            xhr.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    var result = this.responseText;
                    var json = JSON.parse(result);
                    var cookieuser = localStorage.getItem("userr");
                    sendVoice(cookieuser, json["voicename"],groupid);

                }
            };


        });
    }

}


//$("#messageTextBox").change(function () {
//    stateMessage2(user2, state);

//})





function sendVoice(username, pic,group) {


    connection.invoke('SendVoice2', username, pic,group);


}
function sendFile(username, pic,pref,group) {


    connection.invoke('SendFile2', username, pic,pref,group);


}
function sendMessage2(username, text,group) {
    //console.log(username, text);

    if (text && text.length) {

        connection.invoke('SendMessage2', username, text,group);


    }
}
//function stateMessage2(user3, state) {
//    //console.log(username, text);

//    if (text && text.length) {

//        connection.invoke('StateMessage2', user3, state);


//    }
//}

function renderMessage2(name, text, picname) {

    //$("#chatHistory").html(name);
    //$(".box-pic").attr({ "src": "picture/profile/" + time });
    //$(".box-com").html(message);
   
    $(".box-pro").append(
        '<div class="mes-pro" >'
        + '<img onClick="show(\'' + name + '\')" class="mes-pic" src="/picture/profile/' + picname + '"/>'
        + '<p onClick="show(\'' + name + '\')" class="mes-name">' + name + '</p>'
        + '</div><br/>'
        + '<p class="box-com text-white bg-secondary">' + text + '</p>'

        );
    

    //$(".box-pro").append(
    //    '<img class="box-pic" src="/picture/profile/' + picname + '"/>'
    //    + '<p class="box-name" id="chatHistory">' + name + '</p>'
    //    + '<p class="box-date" id="chatHistory">' + date + '</p>'
    //    + '<p class="box-com">' + text + '</p>'
    //);
    $(".mess-box").animate({ scrollTop: $('.mess-box')[0].scrollHeight }, 1000);

}
//function render2Message2(name, state) {

//    //$("#chatHistory").html(name);
//    //$(".box-pic").attr({ "src": "picture/profile/" + time });
//    //$(".box-com").html(message);
//    if (state == "true") {
//        $(".box-pro").append(
//            '<p class="box-name" id="chatHistory">' + name + 'در حال تایپ است</p>'
//        );
//    }

//}

function render3Message2(name, pic,pref) {


    //$("#chatHistory").html(name);
    //$(".box-pic").attr({ "src": "picture/profile/" + time });
    //$(".box-com").html(message);

    //$("#chatHistory").html(name);
    //$(".box-pic").attr({ "src": "picture/profile/" + time });
    //$(".box-com").html(message);
    if (pref == "image/jpeg" || pref == "image/jpg" || pref == "image/png") {
        $(".box-pro").append(
            '<p class="box-com">' + name + '</p>'
            + '<img class="img-com-chat" onclick="openModal()" src="/picture/chat/' + pic + '"/>'
            + '<div id ="myModal" class="modal">'
            + '<span class="close cursor" onclick="closeModal()">&times;</span>'
            + '<img src="/picture/chat/' + pic + '" style="width:100%">'
            + '</div>'


        );
    }
    else if (pref == "audio/mp3") {
        $(".box-pro").append(
            '<p class="box-com">' + name + '</p>'
            + '<audio controls style="width: 200px;">'
            + '<source src="/picture/chat/' + pic + '" type="audio/mp3">'
            + '</audio>')
    }
    else if (pref == "audio/wav") {
        $(".box-pro").append(
            '<p class="box-com">' + name + '</p>'
            + '<audio controls style="width: 200px;">'
            + '<source src="/picture/chat/' + pic + '" type="audio/wav">'
            + '</audio>'
        )
    }

    else if (pref == "video/mp4") {

        $(".box-pro").append(
            '<p class="box-com">' + name + '</p>'
            + '<div onClick="showp(\'' + dd[i].pic + '\');" class="post-pic-click">'
            + '<video class="img-com-user">'
            + '<source src="/picture/chat/' + pic + '" type="video/mp4">'
            + '</video>'
            + '</div>'
        )

    }
    else if (pref != "video/mp4") {


        $(".box-pro").append(
            '<p class="box-com">' + name + '</p>'
            + '<a class="img-com-chat2" href="/picture/chat/' + pic + '"><i class="fa fa-download" aria-hidden="true"></i>دانلود فایل ارسال شده'+pic+'</a>'

        )
    }


    //if (user2 == name) {
    //    $(".box-pro").append(
    //        '<img class="img-com-user" src="/picture/post/' + pic + '"/>'

    //    );
    //}
    //$(".box-pro").append(
    //    '<img class="box-pic" src="/picture/profile/' + picname + '"/>'
    //    + '<p class="box-name" id="chatHistory">' + name + '</p>'
    //    + '<p class="box-date" id="chatHistory">' + date + '</p>'
    //    + '<p class="box-com">' + text + '</p>'
    //);
    $(".mess-box").animate({ scrollTop: $('.mess-box')[0].scrollHeight }, 1000);

}

function render4Message2(name, voice) {


    //$("#chatHistory").html(name);
    //$(".box-pic").attr({ "src": "picture/profile/" + time });
    //$(".box-com").html(message);

    $(".box-pro").append(
        '<p class="box-com">' + name + '</p>'
        + '<audio controls style="width: 200px;">'
        + '<source src="/media/voice/' + voice + '" type="audio/wav">'
        + '</audio>'


    );

    $(".mess-box").animate({ scrollTop: $('.mess-box')[0].scrollHeight }, 1000);

}
document.addEventListener('DOMContentLoaded', ready);


/////////////////////////////////////////////lightbox
function openModal() {
    document.getElementById("myModal").style.display = "block";
}

function closeModal() {
    document.getElementById("myModal").style.display = "none";
}

//var slideIndex = 1;
//showSlides(slideIndex);

//function plusSlides(n) {
//    showSlides(slideIndex += n);
//}
//function currentSlide(n) {
//    showSlides(slideIndex = n);
//}

//function showSlides(n) {
//    var i;
//    var slides = document.getElementsByClassName("mySlides");
//    var dots = document.getElementsByClassName("demo");
//    var captionText = document.getElementById("caption");
//    if (n > slides.length) { slideIndex = 1 }
//    if (n < 1) { slideIndex = slides.length }
//    for (i = 0; i < slides.length; i++) {
//        slides[i].style.display = "none";
//    }
//    for (i = 0; i < dots.length; i++) {
//        dots[i].className = dots[i].className.replace(" active", "");
//    }
//    slides[slideIndex - 1].style.display = "block";
//    dots[slideIndex - 1].className += " active";
//    captionText.innerHTML = dots[slideIndex - 1].alt;
//}
    //////////////////////////////////////////////////////