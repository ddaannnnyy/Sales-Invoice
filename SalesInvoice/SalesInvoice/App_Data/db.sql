CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY AUTOINCREMENT,
								  username TEXT NOT NULL UNIQUE,
								  pass TEXT NOT NULL,
								  location TEXT NOT NULL);

CREATE TABLE IF NOT EXISTS products (id INTEGER PRIMARY KEY AUTOINCREMENT,
									 productName TEXT NOT NULL UNIQUE,
									 productPrice REAL NOT NULL);


									 