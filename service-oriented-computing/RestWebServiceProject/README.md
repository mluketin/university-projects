RestWebService Project
======================

RestWebService Project is small web service.
Service contains users and each user has books.

## Dependencies
To run application, you will need to install following modules:
- express
- express-session
- ua-parser-js

Open console in webservice root directory and run following command:

	npm install <module-name>

Run application from console:

	node server.js
	
Users are stored in users.json file.
Also some books are already in a books.json file.
Stored password is hashed. (for currently stored users, password is 'aaaa')
	



