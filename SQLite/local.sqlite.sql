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
CREATE TABLE IF NOT EXISTS "TradeInfo" (
	"SysId"	TEXT,
	"ComCode"	TEXT NOT NULL,
	"TradeDate"	DATE NOT NULL,
	"TradePrice"	REAL NOT NULL,
	"TradeVolume"	INTEGER NOT NULL,
	"StockCenterName"	TEXT,
	"Memo"	TEXT,
	PRIMARY KEY("SysId")
);
CREATE TABLE IF NOT EXISTS "CustomGroup" (
	"Name"	TEXT NOT NULL,
	"IsFavorite"	INTEGER NOT NULL DEFAULT 1,
	"GroupTypeName"	TEXT,
	PRIMARY KEY("Name")
);
CREATE TABLE IF NOT EXISTS "CustomGroupComCode" (
	"GroupName"	TEXT NOT NULL,
	"ComCode"	TEXT NOT NULL,
	PRIMARY KEY("GroupName","ComCode")
);
CREATE TABLE IF NOT EXISTS "CompanyROE" (
	"UpdateAt"	DATE NOT NULL,
	"ComCode"	TEXT NOT NULL,
	"ROEHeader"	TEXT,
	"ROEValue"	REAL
);
COMMIT;
