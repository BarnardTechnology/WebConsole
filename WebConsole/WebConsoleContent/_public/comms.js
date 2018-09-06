function Comms(serverAddress, prefix, CommandLink, onconnect) {
	var ws;
	var callbacks;

	function init() {
		ws = new WebSocket("ws://" + serverAddress + "/" + prefix);
		callbacks = {};

		ws.onopen = function (event) {
			//console.log("ws open");
			if (onconnect) {
				onconnect();
			}
		}

		ws.onmessage = function (event) {
			//console.log('onmessage');
			//console.log(event.data);
			var respObj = JSON.parse(event.data);
			if (respObj.name === "__response") {
				// this command is a reply
				if (callbacks[respObj.guid]) {
					callbacks[respObj.guid](respObj.arguments[0]);
					delete callbacks[respObj];
				}
			} else if (CommandLink[respObj.name]) {
				// we have a function in the CommandLink
				CommandLink[respObj.name].apply(this, respObj.arguments);
			}
		};

		ws.onclose = function (event) {
			if (event.code == 3001) {
				console.log('Comms WebSocket was closed.');
			} else {
				console.log('Comms WebSocket experienced a connection error.');
			}
			ws = null;
			setTimeout(init, 4000);
		}
	}

	init();

	function guid() {
		function s4() {
			return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
		};
		return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
	}

	function SendCommand(commandName, args, callback) {
		// TODO: currently we only send commands if the websocket is connected. Should we cache up commands if not connected, and then push them through on connection?
		if (ws) {
			var cmdObj = { "name": commandName, "arguments": args };
			cmdObj.TimeSent = new Date();
			cmdObj.guid = guid();
			//console.log("sending " + commandName + " with guid " + cmdObj.guid);
			callbacks[cmdObj.guid] = callback;
			ws.send(JSON.stringify(cmdObj));
		} else {
			console.log('Command "' + commandName + '" not set - the WebSocket is not connected.');
		}
	}

	this.SendCommand = SendCommand;
}