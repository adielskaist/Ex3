var canvas = document.querySelector('canvas');
var x = canvas.width / 2 + lon * canvas.width / 360;
var y = canvas.height / 2 + lat * canvas.height / 180;
var ctx = canvas.getContext('2d');
ctx.beginPath();
ctx.arc(x, y, 1, 0, Math.PI * 2, false);
ctx.fillStyle = "red";
ctx.fill();