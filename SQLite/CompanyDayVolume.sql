BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "CompanyDayVolume" (
	"UpdateAt"	Date,
	"ComCode"	text,
	"ComName"	text,
	"ComType"	text,
	"DayVolume"	int,
	"CurrentPrice"	real
);
COMMIT;
