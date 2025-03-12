CREATE TABLE IF NOT EXISTS "habits" (
	"id"	INTEGER NOT NULL UNIQUE,
	"name"	TEXT NOT NULL UNIQUE COLLATE NOCASE,
	"periodicity"	INTEGER NOT NULL,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "occurrences" (
	"id"	INTEGER,
	"date"	TEXT,
	"habitid"	INTEGER,
	PRIMARY KEY("id"),
	FOREIGN KEY("habitid") REFERENCES "habits"("id")
);