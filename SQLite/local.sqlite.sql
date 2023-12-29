BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "CompanyKDJ" (
	"UpdateAt"	DATE NOT NULL,
	"ComCode"	TEXT NOT NULL,
	"ComName"	TEXT NOT NULL,
	"DayK"	REAL,
	"DayD"	REAL,
	"DayJ"	REAL,
	"WeekK"	REAL,
	"WeekD"	REAL,
	"WeekJ"	REAL,
	"MonthK"	REAL,
	"MonthD"	REAL,
	"MonthJ"	REAL,
	PRIMARY KEY("UpdateAt","ComCode")
);
CREATE TABLE IF NOT EXISTS "CompanyDayVolume" (
	"UpdateAt"	Date NOT NULL,
	"ComCode"	text NOT NULL,
	"ComName"	text NOT NULL,
	"ComType"	text NOT NULL,
	"DayVolume"	int,
	"CurrentPrice"	real,
	PRIMARY KEY("UpdateAt","ComCode")
);
COMMIT;
