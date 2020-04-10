var socket = new WebSocket('http://localhost:4000/links');
socket.onopen = function () {
	console.log('Connected!');
};
socket.onmessage = function (event) {
	console.log('Received data: ' + event.data);
	socket.close();
};
socket.onclose = function () {
	console.log('Lost connection!');
};
socket.onerror = function () {
	console.log('Error!');
};
socket.send(JSON.stringify('data.json'));