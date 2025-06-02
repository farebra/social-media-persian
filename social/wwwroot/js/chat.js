
//Initialize SignalR
var connection = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub')
    .build();
connection.on('ReciveMessage', renderMessage);
/*connection.on('Recive2Message', render2Message);*/
connection.on('Recive3Message', render3Message);
connection.on('Recive4Message', render4Message);
connection.on('Recive5Message', render5Message);


connection.start();
connection.onclose(() => {
    setTimeout(function () {
        connection.start();
    }, 2500);
})
   


 
function ready() {
    $("#camera-btn").click(function () {
 

        var user = $("#rTextBox").val();
        var user2 = $("#xTextBox").val();
        
     
        ////////////////////////////
        sendcamera(user, user2);


    });
    //////////////////////////////////////////////
    var chatForm = document.getElementById('chatForm');
  
    chatForm.addEventListener('submit',
        function (e) {
            e.preventDefault();
            var text = e.target[0].value;
            var user = $("#rTextBox").val();
            var user2 = $("#xTextBox").val();
            e.target[0].value = '';
            $(".emojionearea-editor").html('');
            sendMessage(user, user2, text);
            //console.log(user, text);

        });
    //////////////////////////////////////////////video
    $("#file").change(function () {

        var user = $("#rTextBox").val();
        var user2 = $("#xTextBox").val();
     
        ////////////////////////////
        var file_data = $("#file").prop("files")[0]; // Getting the properties of file from file field
        var form_data = new FormData(); // Creating object of FormData class
        form_data.append("file", file_data) // Appending parameter named file with properties of file_field to form_data
        $.ajax({
            url: "/home/chatfile", // Upload Script
            dataType: 'script',
            cache: false,
            contentType: false,
            processData: false,
            data: form_data, // Setting the data attribute of ajax with file_data
            type: 'post',
            success: function (data) {
                var json = JSON.parse(data);
                sendFile(user, user2, json["filename"], json["prefix"]);
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
        var user2 = $("#xTextBox").val();
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
            xhr.open("POST", "/home/voicefile", true);
            xhr.send(fd);
            xhr.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    var result = this.responseText;
                    var json = JSON.parse(result);
                    sendVoice(user, user2, json["voicename"]);

                }
            };
            

        });
    }

}


//$("#messageTextBox").change(function () {
//    var user = $("#rTextBox").val();
//    localStorage.setItem("userr", user);
//    var user2 = $("#xTextBox").val();
//    localStorage.setItem("userr2", user2);
//        state = "true";
//        var cookieuser = localStorage.getItem("userr");
//        var cookieuser2 = localStorage.getItem("userr2");
//        stateMessage(cookieuser, cookieuser2,state);

//    })
   
   
      

function sendcamera(user,user2) {


    connection.invoke('SendCamera', user,user2);


}
function sendVoice(username, username2, pic) {

   
    connection.invoke('SendVoice', username, username2, pic);


}
function sendFile(username, username2, pic,pref) {

   
    connection.invoke('SendFile', username, username2, pic,pref);


}
function sendMessage(username,username2, text) {
    console.log(username, username2);
    if (text && text.length) {

            connection.invoke('SendMessage', username, username2, text);

       
    }
}
//function stateMessage(user11,user22,state) {
//    //console.log(username, text);

   
//        connection.invoke('StateMessage',user11,user22, state);


//}

function renderMessage(text,color) {

    //$("#chatHistory").html(name);
    //$(".box-pic").attr({ "src": "picture/profile/" + time });
    //$(".box-com").html(message);
    if (color == "user1") {
        $(".box-pro").append(

         '<p class="box-com text-white bg-info">' + text + '</p>'
        );
    }
    else if(color == "user2") {
        $(".box-pro").append(
         '<p class="box-com text-white bg-secondary">' + text + '</p>'
        );
    }
    //$(".box-pro").append(
    //    '<img class="box-pic" src="/picture/profile/' + picname + '"/>'
    //    + '<p class="box-name" id="chatHistory">' + name + '</p>'
    //    + '<p class="box-date" id="chatHistory">' + date + '</p>'
    //    + '<p class="box-com">' + text + '</p>'
    //);
    $(".mess-box").animate({ scrollTop: $('.mess-box')[0].scrollHeight }, 1000);
}
//function render2Message(name,state) {

//    //$("#chatHistory").html(name);
//    //$(".box-pic").attr({ "src": "picture/profile/" + time });
//    //$(".box-com").html(message);
//    if (state == "true") {
//        $(".box-pro").append(
//            '<p class="box-name" id="chatHistory">' + name + 'در حال تایپ است</p>'
//        );
//    }
   //}

function render3Message(name, pic,pref) {
   

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
            +'<div onClick="showp(\'' + dd[i].pic + '\');" class="post-pic-click">'
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

function render4Message(name, voice) {


    //$("#chatHistory").html(name);
    //$(".box-pic").attr({ "src": "picture/profile/" + time });
    //$(".box-com").html(message);
   
    $(".box-pro").append(
        '<p class="box-com">' + name + '</p>'
        +'<audio controls style="width: 200px;">'
        + '<source src="/media/voice/' + voice +'" type="audio/wav">'
    +'</audio>'


    );
    $(".mess-box").animate({ scrollTop: $('.mess-box')[0].scrollHeight }, 1000);

}
function render5Message(puser, puser1, puser2) {
   
   
    //$("#chatHistory").html(name);
    //$(".box-pic").attr({ "src": "picture/profile/" + time });
    //$(".box-com").html(message);
   
        $(".box-pro").append(
            '<p class="box-com">' + puser1 + ' / ' + puser2 + '</p>'
        + '<p id="box-com2" class="box-com2">' + puser + '</p>'
            + '<button onclick="callUser()" class="img-com-chat22"><i class="fa fa-camera" aria-hidden="true"></i>Accept Call ' + puser1 + '</button>'
            + '&nbsp&nbsp<audio id="svideo" autoplay>'
            + '<source src="/media/voice/ringtone/ring.mp3">'
            + '</audio>'
            + '<br/><button onclick="endcall()" class="img-com-chat22">cancel Call</button>'
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
////////////////////////////////////////////////////////////

//async function callUser() {


//    $(".end-btn").show();
//    $(".phone-btn").hide();
//    $(".camera-btn").hide();
//    $(".back-btn").hide();
//    $(".mess-box").hide();
//    $(".form-box").hide();
//    $("#end-call").show();

//    var peerId = document.getElementById("box-com2").innerHTML;

//    var stream = await navigator.mediaDevices.getUserMedia({
//        video: true,
//        audio: true,
//    });// switch to the video call and play the camera preview

//    document.getElementById("live").style.display = "block";
//    document.getElementById("local-video").srcObject = stream;
//    document.getElementById("local-video").play();// make the call
//    var call = peer.call(peerId, stream);
//    call.on("stream", (stream) => {
//        document.getElementById("remote-video").srcObject = stream;
//        document.getElementById("remote-video").play();
//    });
//    call.on("data", (stream) => {
//        document.getElementById("#remote-video").srcObject = stream;
//    });
//    call.on("error", (err) => {
//        console.log(err);
//    });
//    call.on('close', () => {
//        endCall()
//    })// save the close function
//    currentCall = call;
//    //}
//    //})
//}
//var peer = new Peer();

//var getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia;
//peer.on('call', function (call) {
//    alert(peer);
//    alert(call);

//    getUserMedia({ video: true, audio: true }, function (stream) {
//        console.log('Received stream1', stream);

//        call.answer(stream); // Answer the call with an A/V stream.
//        document.querySelector("#live").style.display = "block";
//        call.on('stream', function (remoteStream) {
//            $(".aler").hide();

//            // Show stream in some video/canvas element.
//            console.log('Received stream2', remoteStream);
//            const video = document.getElementById("remote-video");
//            video.srcObject = remoteStream;
//            video.onloadedmetadata = () => {
//                video.play();
//            };
//        });
//    })
//}, function (err) {
//    console.log('Failed to get local stream', err);

//});
   /////////////////////////////////////////////////master peer top
// async function callUser() {
       
    
  

//        $(".end-btn").show();
//        $(".phone-btn").hide();
//        $(".camera-btn").hide();
//        $(".back-btn").hide();
//        $(".mess-box").hide();
//        $(".form-box").hide();
//        $("#end-call").show();

//     const peerId = document.getElementsByClassName("box-com2").value;
            
//            var stream =await navigator.mediaDevices.getUserMedia({
//                video: true,
//                audio: true,
//            });// switch to the video call and play the camera preview

//            document.getElementById("live").style.display = "block";
//            document.getElementById("local-video").srcObject = stream;
//            document.getElementById("local-video").play();// make the call
//     var call = peer.call(peerId, stream);
//            call.on("stream", (stream) => {
//                document.getElementById("remote-video").srcObject = stream;
//                document.getElementById("remote-video").play();
//            });
//            call.on("data", (stream) => {
//            document.getElementById("#remote-video").srcObject = stream;
//            });
//            call.on("error", (err) => {
//                console.log(err);
//            });
//            call.on('close', () => {
//                endCall()
//            })// save the close function
//            currentCall = call;
//           //}
//           //})
//           }
//                var getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia;
//                peer.on('call', function (call) {
//                    getUserMedia({ video: true, audio: true }, function (stream) {
                     

//                        call.answer(stream); // Answer the call with an A/V stream.
//                        document.querySelector("#live").style.display = "block";
//                        call.on('stream', function (remoteStream) {
                        

//                            // Show stream in some video/canvas element.
//                            console.log('Received stream2', remoteStream);
//                            const video = document.getElementById("remote-video");
//                            video.srcObject = remoteStream;
//                            video.onloadedmetadata = () => {
//                                video.play();
//                            };
//                        });
//                    })
//                }, function (err) {
//                    console.log('Failed to get local stream', err);

//                });
//                ///////////////////////////////////////////
//function endcall() {
//    $(".end-btn").hide();
//    $(".phone-btn").show();
//    $(".camera-btn").show();
//    $(".back-btn").show();
//    $(".mess-box").show();
//    $(".form-box").show();
//    $(".aler").hide();
//    $(".aler2").hide();

//    // Go back to the menu

//    document.querySelector("#live").style.display = "none";// If there is no current call, return
//    if (!currentCall) return;// Close the call, and reset the function
//    try {
//        currentCall.close();
//    } catch { }
//    currentCall = undefined;
//}

//    //////////////////////////////////////////////////////
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