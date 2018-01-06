 
"use strict"; 
function rocketCD() {
    var targetdate = new Date("September 1, 2018, 00:01:00"); //Change to the date and time your app, game, website or business will launch, follow the format displayed
    var today = new Date();
    var TimeDiff = targetdate.getTime() - today.getTime();
                
    if (TimeDiff <= 0) {
        clearTimeout(timer);
        document.write("Msg to developer: launch is here, countdown is over.");
    }
                
    var secs = Math.floor(TimeDiff / 1000);
    var mins = Math.floor(secs / 60);
    var hours = Math.floor(mins / 60);
    var days = Math.floor(hours / 24);
    hours %= 24;
    mins %= 60;
    secs %= 60;
                
    document.getElementById("days").innerHTML = days; 
    document.getElementById("hours").innerHTML = hours;
    document.getElementById("mins").innerHTML = mins;
    document.getElementById("secs").innerHTML = secs;
                
    var timer = setTimeout('rocketCD()',1000);
}

