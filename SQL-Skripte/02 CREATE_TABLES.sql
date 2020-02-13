CREATE TABLE Familien
(
    Nummer          integer NOT NULL PRIMARY KEY,
    Familienname    varchar(40) NOT NULL
);

CREATE TABLE Personen
(
    Nummer          integer NOT NULL PRIMARY KEY,
    Vorname_Nr      integer NOT NULL,
    Familie_Nr      integer NOT NULL,
    Geburtsdatum    date NOT NULL,
    Mutter_Nr       integer,
    Vater_Nr        integer,
    Partner_Nr        integer,
    Lebend            BOOLEAN DEFAULT 1 NOT NULL
);

CREATE TABLE Vornamen
(
    Nummer          integer NOT NULL PRIMARY KEY,
    Vorname         varchar(40) NOT NULL,
    Geschlecht      char(1) NOT NULL
);

CREATE TABLE Einstellungen
(
	Nummer			integer NOT NULL PRIMARY KEY,
	SettingKey				varchar(40) NOT NULL,
	SettingValue			varchar(40) NOT NULL
);