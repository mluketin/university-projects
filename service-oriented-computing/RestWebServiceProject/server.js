var express = require('express');
var session = require('express-session');
var bodyParser = require('body-parser');
var fs = require("fs");
var url = require('url');
var UAParser = require('ua-parser-js'); //for browser detection

var app = express();
app.use(session({secret: 'ssshhhhh'}));
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

//read users and books from 'database'
var usersJson;
var booksJson;

fs.readFile( "database/" + "users.json", 'utf8', function (err, data) {
        usersJson = JSON.parse(data);
});

fs.readFile( "database/" + "books.json", 'utf8', function (err, data) {
        booksJson = JSON.parse(data);
});

//create server
var server = app.listen(8081, function () {
    var host = server.address().address;
    var port = server.address().port;
    console.log("App is listening at http://%s:%s", host, port)
});

//returns index.html
app.get('/api', function (req, res) {
   logRequest("/api", req);
   res.sendfile('public/index.html');
})

//returns documentation on how to use api
app.get('/docs', function (req, res) {
		  logRequest("/docs", req);

   	var scriptPath = 'public/doc.txt';
	res.sendfile(scriptPath);
})

//fetching scripts for html pages
app.get('/public/script', function (req, res) {
	var url_parts = url.parse(req.url, true);
	var query = url_parts.query;
	 
	var scriptPath = 'public/script/' + query.script;
	res.sendfile(scriptPath);
})


app.get('/login', function (req, res) {
		  logRequest("/login", req);

	var header=req.headers['authorization']||'',        // get the header
      token=header.split(/\s+/).pop()||'',            // and the encoded auth token
      auth=new Buffer(token, 'base64').toString(),    // convert from base64
      parts=auth.split(/:/),                          // split on colon
      username=parts[0],
      hash=parts[1];
      
	var user = userExists(username, hash);
	console.log(user);
	var responseBody;
	if(user != null) {
		responseBody = {
			"id": user.id,
			"name": user.name,
			"surname": user.surname
		}
	} else {
		//return 401 
		res.status(401);
		responseBody = {message: "Unauthorized access"};
	}
	
	console.log(responseBody);
	res.header("Content-Type",'application/json');
	res.send(JSON.stringify(responseBody));
});




//users resource
app.post('/users', function (req, res) {
		  logRequest("/users", req);

	console.log(req.headers);
	console.log(req.body);
	console.log(req.body.hasOwnProperty('name'));
	   
	
	  
	var user = null;

	var response;
	
		response = {registered: true, message: "User registered successfully"};
		user = {
			"id": usersJson.index+1,
			"username": req.body.username,
			"hash": req.body.hash,
			"name": req.body.name,
			"surname": req.body.surname
		}
		usersJson.index += 1;
		
		usersJson.users.splice(0,0,user);
		console.log(usersJson);
	
		
	res.header("Content-Type",'application/json');
	res.send(JSON.stringify(response));
});



app.get('/users', function (req, res) {
		  logRequest("/users", req);

	var header=req.headers['authorization']||'',        // get the header
      token=header.split(/\s+/).pop()||'',            // and the encoded auth token
      auth=new Buffer(token, 'base64').toString(),    // convert from base64
      parts=auth.split(/:/),                          // split on colon
      username=parts[0],
      hash=parts[1];
      
	var user = userExists(username, hash);
	var responseBody;
	if(user == null) {
		res.status(401);
		responseBody = {message: "Unauthorized access"};
	} else {
		responseBody = usersJson;
	}
		
    res.header("Content-Type",'application/json');
	res.send(JSON.stringify(responseBody));
});

app.get('/users/:id',  function (req, res) {
	var id = req.params.id;
	logRequest("/users/" + id, req);
	
	var header=req.headers['authorization']||'',        // get the header
      token=header.split(/\s+/).pop()||'',            // and the encoded auth token
      auth=new Buffer(token, 'base64').toString(),    // convert from base64
      parts=auth.split(/:/),                          // split on colon
      username=parts[0],
      hash=parts[1];
      
	var user = userExists(username, hash);
	var responseBody;
	if(user == null) {
		res.status(401);
		responseBody = {message: "Unauthorized access"};
	} else {
		var requiredUser = getUserById(id);
		if(requiredUser == null) {
		 	res.status(204);
			responseBody = {message: "Content does not exist"};
		} else {
			responseBody = requiredUser;
		}
	}
		
    res.header("Content-Type",'application/json');
	res.send(JSON.stringify(responseBody));
});

app.put('/users/:id', function (req, res) {
	
    var id = req.params.id;
	logRequest("/users/" + id, req);
	var header=req.headers['authorization']||'',        // get the header
      token=header.split(/\s+/).pop()||'',            // and the encoded auth token
      auth=new Buffer(token, 'base64').toString(),    // convert from base64
      parts=auth.split(/:/),                          // split on colon
      username=parts[0],
      hash=parts[1];
      
	var user = userExists(username, hash);
	var responseBody;
	if(user == null) {
		res.status(401);
		responseBody = {message: "Unauthorized access"};
	} else {
		if(user.id == id) {
			var jsonBody = req.body;
			
			user.name = jsonBody.name;
			user.surname = jsonBody.surname;
			
			var index;
			for(i = 0; i < usersJson.users.length; i++) {
				if(usersJson.users[i].username == username) {
					if(usersJson.users[i].hash == hash) {
						index = i;
			}
			usersJson.users.splice(index, 1, user);
			console.log(usersJson.users);
		}
	}
		} else {
			res.status(401);
			responseBody = {message: "Unauthorized access"};
		}			
	} 
	
    res.header("Content-Type",'application/json');
	res.send(JSON.stringify(responseBody));
});


app.delete('/users/:id',  function (req, res) {
	var id = req.params.id;
		logRequest("/users/" + id, req);

	var header=req.headers['authorization']||'',        // get the header
      token=header.split(/\s+/).pop()||'',            // and the encoded auth token
      auth=new Buffer(token, 'base64').toString(),    // convert from base64
      parts=auth.split(/:/),                          // split on colon
      username=parts[0],
      hash=parts[1];
      
	var user = userExists(username, hash);
	var responseBody;
	if(user == null) {
		res.status(401);
		responseBody = {message: "Unauthorized access"};
	} else {
		var index;
			for(i = 0; i < usersJson.users.length; i++) {
				if(usersJson.users[i].id == id) {
						index = i;
				}
			}
			usersJson.users.splice(index, 1);
			console.log(usersJson.users);
	}
		
    res.header("Content-Type",'application/json');
	res.send(JSON.stringify(responseBody));
});


//books resource
app.get('/users/:id/books',  function (req, res) {
	var id = req.params.id;
		logRequest("/users/" + id + "/books", req);

	var header=req.headers['authorization']||'',        // get the header
      token=header.split(/\s+/).pop()||'',            // and the encoded auth token
      auth=new Buffer(token, 'base64').toString(),    // convert from base64
      parts=auth.split(/:/),                          // split on colon
      username=parts[0],
      hash=parts[1];
      
	var user = userExists(username, hash);
	var responseBody;
	if(user == null) {
		res.status(401);
		responseBody = {message: "Unauthorized access"};
	} else {
		var requiredUser = getUserById(id);
		if(requiredUser == null) {
		 	res.status(204);
			responseBody = {message: "Content does not exist"};
		} else {
			responseBody = getBooksByUserId(id);
		}
	}
		
    res.header("Content-Type",'application/json');
	res.send(JSON.stringify(responseBody));
	
	
});

app.get('/users/:id/books/:bookid',  function (req, res) {
	var id = req.params.id;
	var bookId = req.params.bookid;
	logRequest("/users/" + id + "/books/" + bookId, req);
	var header=req.headers['authorization']||'',        // get the header
      token=header.split(/\s+/).pop()||'',            // and the encoded auth token
      auth=new Buffer(token, 'base64').toString(),    // convert from base64
      parts=auth.split(/:/),                          // split on colon
      username=parts[0],
      hash=parts[1];
      
	var user = userExists(username, hash);
	var responseBody;
	if(user == null) {
		res.status(401);
		responseBody = {message: "Unauthorized access"};
	} else {
		var requiredUser = getUserById(id);
		if(requiredUser == null) {
		 	res.status(204);
			responseBody = {message: "Content does not exist"};
		} else {
			
			var responseBodyJson = getBookById(id, bookId);
			if(responseBodyJson == null) {
				res.status(204);
				responseBody = {message: "Content does not exist"};
			} else {
				responseBody = responseBodyJson;
			}
		}
	}
		
    res.header("Content-Type",'application/json');
	res.send(JSON.stringify(responseBody));
	
	
});

app.delete('/users/:id/books/:bookId', function (req, res) {
    var id = req.params.id;
	var bookId = req.params.bookId;
	console.log(id, bookId);
	logRequest("/users/" + id + "/books/" + bookId, req);
	var header=req.headers['authorization']||'',        // get the header
      token=header.split(/\s+/).pop()||'',            // and the encoded auth token
      auth=new Buffer(token, 'base64').toString(),    // convert from base64
      parts=auth.split(/:/),                          // split on colon
      username=parts[0],
      hash=parts[1];
      
	var user = userExists(username, hash);
	var responseBody;
	if(user == null) {
		res.status(401);
		responseBody = {message: "Unauthorized access"};
	} else {
		if(user.id == id) { //if book is owned by user
			var jsonBody = req.body;
			
			
			var index = null;
			for(i = 0; i < booksJson.books.length; i++) {
				if(booksJson.books[i].userId == id) {
					if(booksJson.books[i].id == bookId) {
						index = i;
					}
				}
			}
			if(index == null) {
				res.status(204);
				responseBody = {message: "No content"};	
				
			} else {
				booksJson.books.splice(index, 1);
			}
		} else {
			res.status(401);
			responseBody = {message: "Unauthorized access"};
		}			
	} 
	
    res.header("Content-Type",'application/json');
	res.send(JSON.stringify(responseBody));
});

app.put('/users/:id/books/:bookId', function (req, res) {
    var id = req.params.id;
	var bookId = req.params.bookId;
	logRequest("/users/" + id + "/books/" + bookId, req);
	var header=req.headers['authorization']||'',        // get the header
      token=header.split(/\s+/).pop()||'',            // and the encoded auth token
      auth=new Buffer(token, 'base64').toString(),    // convert from base64
      parts=auth.split(/:/),                          // split on colon
      username=parts[0],
      hash=parts[1];
      
	var user = userExists(username, hash);
	var responseBody;
	if(user == null) {
		res.status(401);
		responseBody = {message: "Unauthorized access"};
	} else {
		if(user.id == id) { //if book is owned by user
			var jsonBody = req.body;
			
			
			var index = null;
			for(i = 0; i < booksJson.books.length; i++) {
				if(booksJson.books[i].userId == id) {
					if(booksJson.books[i].id == bookId) {
						index = i;
			}
			if(index == null) {
				res.status(204);
				responseBody = {message: "No content"};	
				
			} else {
				var book = {
					userId: id,
					id: bookId,
					title: req.body.title,
					summary: req.body.summary
				};
				booksJson.books.splice(index, 1, book);
				console.log(booksJson.books);
			}
		}
	}
		} else {
			res.status(401);
			responseBody = {message: "Unauthorized access"};
		}			
	} 
	
    res.header("Content-Type",'application/json');
	res.send(JSON.stringify(responseBody));
});

app.post('/users/:id/books', function (req, res) {
	 var id = req.params.id;
	logRequest("/users/" + id + "/books", req);
	var header=req.headers['authorization']||'',        // get the header
      token=header.split(/\s+/).pop()||'',            // and the encoded auth token
      auth=new Buffer(token, 'base64').toString(),    // convert from base64
      parts=auth.split(/:/),                          // split on colon
      username=parts[0],
      hash=parts[1];
      
	var user = userExists(username, hash);
	var responseBody;
	if(user == null) {
		res.status(401);
		responseBody = {message: "Unauthorized access"};
	} else {
		if(user.id == id) { //if book is owned by user
			
			var maxId = 0;
			for(i = 0; i < booksJson.books.length; i++) {
				if(booksJson.books[i].userId == id) {
					if(booksJson.books[i].id > maxId) {
						maxId = booksJson.books[i].id;
					}
				}
			}
			
			var book = {
				userId: req.body.userId,
				id: maxId+1,
				title: req.body.title,
				summary: req.body.summary
			};
			booksJson.books.splice(0, 0, book);
			//console.log(booksJson.books);

		} else {
			res.status(401);
			responseBody = {message: "Unauthorized access"};
		}			
	} 
	
    res.header("Content-Type",'application/json');
	res.send(JSON.stringify(responseBody));
	
	
	
});

//writes request in log (path and browser)
function logRequest(urlPath, req) {
	var parser = new UAParser();
	var ua = req.headers['user-agent'];
	var browserName = parser.setUA(ua).getBrowser().name;
	if(browserName == undefined) {
		browserName = "unknown"
	} else {
		browserName = browserName.toLowerCase();
	}
	
	fs.appendFile('log/log.txt', urlPath + " " + browserName + "\n", function (err) {
		console.log(err);
	});
}

function usernameExists(username) {
	var user = null;
	for(i = 0; i < usersJson.users.length; i++) {
		if(usersJson.users[i].username === username) {
			user = usersJson.users[i];
		}
	}
	return user;
}

function userExists(username, hash) {
	var user = null;
	for(i = 0; i < usersJson.users.length; i++) {
		if(usersJson.users[i].username === username) {
			if(usersJson.users[i].hash === hash) {
				user = usersJson.users[i];
			}
		}
	}
	return user;
}

function getUserById(id) {
	var user = null;
	for(i = 0; i < usersJson.users.length; i++) {
		if(usersJson.users[i].id == id) {
				user = usersJson.users[i];
		}
	}
	return user;
}

function getBooksByUserId(userId) {
	var books = {
		books: []
	};
	
	console.log("\n");
	console.log(booksJson.books);
	
	for(i = 0; i < booksJson.books.length; i++) {
		if(booksJson.books[i].userId == userId) {
			books.books.push(booksJson.books[i]);
		}
	}
	return books;
}

function getBookById(userId, bookId) {
	for(i = 0; i < booksJson.books.length; i++) {
		if(booksJson.books[i].userId == userId) {
			if(booksJson.books[i].id == bookId) {
				return booksJson.books[i];
			}
		}
	}
	return null;
}

//detecting changes
if (process.platform === "win32") {
  var rl = require("readline").createInterface({
    input: process.stdin,
    output: process.stdout
  });
  
  rl.on("SIGINT", function () {
    process.emit("SIGINT");
  });
}


process.on("SIGINT", function () {
  //graceful shutdown
  console.log("EXIT EXIT");
  process.exit();
});