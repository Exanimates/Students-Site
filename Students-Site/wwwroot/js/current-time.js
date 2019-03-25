function startTime(elemId) {
	var spanCurrent = document.getElementById(elemId);

	function getTime() {
		var today = new Date();
		var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
		var time = today.getHours() + ":" + checkTime(today.getMinutes()) + ":" + checkTime(today.getSeconds());

		spanCurrent.textContent = date + ' ' + time;
	}

	setInterval(getTime, 500);
}

function checkTime(i) {
	if (i < 10) {i = "0" + i};
	return i;
}