﻿let video = null;
let canvas = null;
let context = null;
let streaming = false;
let stream = null;
let width = 100;    // We will scale the photo width to this.
let height = 0;     // This will be computed based on the input stream
let filter = 'sepia(1)';

function onStart(options) {
    video = document.getElementById(options.videoID);
    canvas = document.getElementById(options.canvasID);
    context = canvas.getContext('2d');
    width = options.width;
    filter = options.filter;

    navigator.mediaDevices.getUserMedia({ video: true, audio: false })
        .then(function (stream1) {
            stream = stream1;
            video.srcObject = stream1;
            video.play();
        })
        .catch(function (err) {
            console.log("An error occurred: " + err);
        });

    video.addEventListener('canplay', function () {
        if (!streaming) {
            height = video.videoHeight / (video.videoWidth / width);

            if (isNaN(height)) {
                height = width / (4 / 3);
            }

            video.setAttribute('width', width);
            video.setAttribute('height', height);
            canvas.setAttribute('width', width);
            canvas.setAttribute('height', height);
            streaming = true;
        }
    }, false);

    video.addEventListener("play", function () {
        console.log('play');
        timercallback();
    }, false);
}
// stop both mic and camera
function stopBothVideoAndAudio(stream) {
    stream.getTracks().forEach((track) => {
        if (track.readyState == 'live') {
            track.stop();
        }
    });
}

// stop only camera
function stopVideoOnly(stream) {
    stream.getTracks().forEach((track) => {
        if (track.readyState == 'live' && track.kind === 'video') {
            track.stop();
        }
    });
}

// stop only mic
function stopAudioOnly(stream) {
    stream.getTracks().forEach((track) => {
        if (track.readyState == 'live' && track.kind === 'audio') {
            track.stop();
        }
    });
}
function onStop() {
    if (stream) {
        stopBothVideoAndAudio(stream);
        streaming = false;
    }
}

function timercallback() {
    if (video.paused || video.ended) {
        return;
    }
    computeFrame();
    setTimeout(function () {
        timercallback();
    }, 0);
}

function computeFrame() {
    context.drawImage(video, 0, 0, width, height);
    context.filter = filter;
}

window.WebCamFunctions = {
    start: (options) => { onStart(options); },
    stop: () => { onStop(); }
};


window.CaptureCanvas = async (canvasId, imgId) => {
    var cnvs = document.getElementById(canvasId),
        ctx = cnvs.getContext('2d'),
        mirror = document.getElementById(imgId);
    mirror.width = cnvs.width;
    mirror.height = cnvs.height;
    let dataURL = cnvs.toDataURL('image/png');
    mirror.src = dataURL;
    let parsed = dataURL.split(',')[1];
    return parsed;
    //.replace('data:image/png;base64,','');
};

//async function CaptureCanvas (canvasId, imgId){
//    var cnvs = document.getElementById(canvasId);
//    var ctx = cnvs.getContext('2d');
//    var mirror = document.getElementById(imgId);
//    mirror.width = cnvs.width;
//    mirror.height = cnvs.height;
//    let dataURL = cnvs.toDataURL('image/png');
//    mirror.src = dataURL;
//    let parsed = dataURL.split(',')[1];
//    return parsed;
//    //.replace('data:image/png;base64,','');
//}