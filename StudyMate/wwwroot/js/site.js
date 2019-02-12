﻿// Write your JavaScript code.
function Timer(h, m) {
	// Set the date we're counting down to
	var currentDate = new Date();
	n = (h * 3600000) + (m * 60000) + currentDate.getTime();

	var countDownDate = new Date(n).getTime();

	// Update the count down every 1 second
	var x = setInterval(function () {

		// Get todays date and time
		var now = new Date().getTime();

		// Find the distance between now and the count down date
		var distance = countDownDate - now;

		// Time calculations for days, hours, minutes and seconds
		
		var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
		var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
		var seconds = Math.floor((distance % (1000 * 60)) / 1000);

		// Output the result in an element with id="demo"
		timePortion = document.getElementsByClassName("timer-show");
		for (let k = 0; k < timePortion.length; k++) {
			timePortion[k].innerHTML = hours + " : " + minutes + " : " + seconds;
			timePortion[k].style.display = "block";
		}
		
		// If the count down is over, write some text 
		if (distance < 0) {
			clearInterval(x);
			alert('Expired');
			finish();
		}
	}, 1000);
}
