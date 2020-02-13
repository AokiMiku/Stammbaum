DELETE FROM Personen;
DELETE FROM Familien;
DELETE FROM Vornamen;

ALTER SEQUENCE gen_familien_id RESTART WITH 0;
ALTER SEQUENCE gen_personen_id RESTART WITH 0;
ALTER SEQUENCE gen_vornamen_id RESTART WITH 0;


--Familien
INSERT INTO Familien (Familienname)
    VALUES ('Krüger');

INSERT INTO Familien (Familienname)
    VALUES ('Forstner');
    
INSERT INTO Familien (Familienname)
    VALUES ('Hinzmann');

INSERT INTO Familien (Familienname)
    VALUES ('Menzel');
    
INSERT INTO Familien (Familienname)
    VALUES ('Eppler');
    
INSERT INTO Familien (Familienname)
    VALUES ('Reder');
    
INSERT INTO Familien (Familienname)
    VALUES ('Schrage');
    
INSERT INTO Familien (Familienname)
    VALUES ('Lemmer');
    
INSERT INTO Familien (Familienname)
    VALUES ('Bohnert');
    
INSERT INTO Familien (Familienname)
    VALUES ('Stark');
    
INSERT INTO Familien (Familienname)
    VALUES ('Tully');
    
INSERT INTO Familien (Familienname)
    VALUES ('Arryn');
    
INSERT INTO Familien (Familienname)
    VALUES ('Hilmer');
    
INSERT INTO Familien (Familienname)
    VALUES ('Matern');
    
INSERT INTO Familien (Familienname)
    VALUES ('Behling');
    
INSERT INTO Familien (Familienname)
    VALUES ('Weyers');
    
INSERT INTO Familien (Familienname)
    VALUES ('Frey');
    
INSERT INTO Familien (Familienname)
    VALUES ('Schaffrath');
    
INSERT INTO Familien (Familienname)
    VALUES ('Manke');
    
INSERT INTO Familien (Familienname)
    VALUES ('Reetz');
    
INSERT INTO Familien (Familienname)
    VALUES ('Hübsch');
    
INSERT INTO Familien (Familienname)
    VALUES ('Wiegel');
    
INSERT INTO Familien (Familienname)
    VALUES ('Lackner');
    
INSERT INTO Familien (Familienname)
    VALUES ('Heinig');
    
INSERT INTO Familien (Familienname)
    VALUES ('Krammer');
    
INSERT INTO Familien (Familienname)
    VALUES ('Stevens');
    
INSERT INTO Familien (Familienname)
    VALUES ('Lembke');
    
INSERT INTO Familien (Familienname)
    VALUES ('Winzer');
    
INSERT INTO Familien (Familienname)
    VALUES ('Bamberger');
    
INSERT INTO Familien (Familienname)
    VALUES ('Liebe');
    
INSERT INTO Familien (Familienname)
    VALUES ('Achatz');
    
INSERT INTO Familien (Familienname)
    VALUES ('Kuntze');
    
INSERT INTO Familien (Familienname)
    VALUES ('Sorge');
    
INSERT INTO Familien (Familienname)
    VALUES ('Fels');
    
INSERT INTO Familien (Familienname)
    VALUES ('Strauss');
    
INSERT INTO Familien (Familienname)
    VALUES ('Sieger');
    
INSERT INTO Familien (Familienname)
    VALUES ('Kölling');
    
INSERT INTO Familien (Familienname)
    VALUES ('Dettmann');
    
INSERT INTO Familien (Familienname)
    VALUES ('Siemens');
    
INSERT INTO Familien (Familienname)
    VALUES ('Grün');
    
INSERT INTO Familien (Familienname)
    VALUES ('Anger');
    
INSERT INTO Familien (Familienname)
    VALUES ('Neef');
    
INSERT INTO Familien (Familienname)
    VALUES ('Dorner');
    
INSERT INTO Familien (Familienname)
    VALUES ('Edel');
    
INSERT INTO Familien (Familienname)
    VALUES ('Hase');
    
INSERT INTO Familien (Familienname)
    VALUES ('Kürschner');
    
INSERT INTO Familien (Familienname)
    VALUES ('Cooper');
    
INSERT INTO Familien (Familienname)
    VALUES ('Hoffstadter');
    
INSERT INTO Familien (Familienname)
    VALUES ('Edler');
    
INSERT INTO Familien (Familienname)
    VALUES ('Henning');
    
INSERT INTO Familien (Familienname)
    VALUES ('Schönberg');
    
INSERT INTO Familien (Familienname)
    VALUES ('Schnur');
    
INSERT INTO Familien (Familienname)
    VALUES ('Lex');
    
INSERT INTO Familien (Familienname)
    VALUES ('Klages');
    
INSERT INTO Familien (Familienname)
    VALUES ('Spitz');
    
INSERT INTO Familien (Familienname)
    VALUES ('Dunkel');
    
INSERT INTO Familien (Familienname)
    VALUES ('Kaya');
    
INSERT INTO Familien (Familienname)
    VALUES ('Hauschild');
    
INSERT INTO Familien (Familienname)
    VALUES ('Hillmann');

    
    
--Vornamen
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Frank', 'M');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Lisa', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Pete', 'M');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Sarah', 'W');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Mike', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Kim', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Rich', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Valentina', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Eddard', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Wanda', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Odin', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Arya', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Patrick', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Asuna', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Pascal', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Avril', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Salomo', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Julie', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Sergius', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Isabel', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Thilo', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Lulu', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Oscar', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Patricia', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Theodor', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Paula', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Richard', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Pia', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Rickard', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Seraphina', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Trevor', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Riana', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Tristan', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Susanne', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Stan', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Valeria', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Romulus', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Yana', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Valentin', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Vanessa', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Yannick', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Zarah', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Maurice', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Jenifer', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Moritz', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Sylvia', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Michael', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Sophie', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Marco', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Sophia', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Harald', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Ruth', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Konrad', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Roxanne', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Konstantin', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Shirley', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Marcel', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Ostara', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Janos', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Sabrina', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Daniel', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Samira', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Kai', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Melissa', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Gandolf', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Tauriel', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Gaius', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Minna', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Damian', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Marianne', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Balthasar', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Veronica', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Hobarth', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Morgana', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Gabriel', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Luisa', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Dennis', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Caroline', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Alexander', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Maturin', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Philipp', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Djamila', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Caldor', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Lucia', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Edmure', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Lydia', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('David', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Becca', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Aaron', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Lyanna', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Carsten', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Tabea', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Gordon', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Tara', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Francesco', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Theresa', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Francis', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Trixi', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Agni', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Thyra', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Rudra', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Shiva', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Alastar', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Zora', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Altair', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Yanina', 'W');
    
INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Larry', 'M');

INSERT INTO Vornamen (Vorname, Geschlecht)
    VALUES ('Janine', 'W');
    

--Personen
INSERT INTO Personen (Vorname_Nr, Familie_Nr, Geburtsdatum, Mutter_Nr, Vater_Nr, Partner_Nr, Lebend, Generation)
    VALUES (5, 4, '30.12.1993', null, null, null, 1, 0);
    
INSERT INTO Personen (Vorname_Nr, Familie_Nr, Geburtsdatum, Mutter_Nr, Vater_Nr, Partner_Nr, Lebend, Generation)
    VALUES (6, 2, '21.10.1995', null, null, null, 1, 0);
    
INSERT INTO Personen (Vorname_Nr, Familie_Nr, Geburtsdatum, Mutter_Nr, Vater_Nr, Partner_Nr, Lebend, Generation)
    VALUES (4, 3, '07.06.1994', null, null, null, 1, 0);
    
INSERT INTO Personen (Vorname_Nr, Familie_Nr, Geburtsdatum, Mutter_Nr, Vater_Nr, Partner_Nr, Lebend, Generation)
    VALUES (3, 1, '22.05.1997', null, null, null, 1, 0);
    
INSERT INTO Personen (Vorname_Nr, Familie_Nr, Geburtsdatum, Mutter_Nr, Vater_Nr, Partner_Nr, Lebend, Generation)
    VALUES (7, 6, '06.04.1992', null, null, null, 1, 0);
    
INSERT INTO Personen (Vorname_Nr, Familie_Nr, Geburtsdatum, Mutter_Nr, Vater_Nr, Partner_Nr, Lebend, Generation)
    VALUES (1, 11, '15.09.1994', null, null, null, 1, 0);
    
INSERT INTO Personen (Vorname_Nr, Familie_Nr, Geburtsdatum, Mutter_Nr, Vater_Nr, Partner_Nr, Lebend, Generation)
    VALUES (2, 5, '30.11.1993', null, null, null, 1, 0);
    
INSERT INTO Personen (Vorname_Nr, Familie_Nr, Geburtsdatum, Mutter_Nr, Vater_Nr, Partner_Nr, Lebend, Generation)
    VALUES (4, 41, '08.08.1998', null, null, null, 1, 0);
	
INSERT INTO Personen (Vorname_Nr, Familie_Nr, Geburtsdatum, Mutter_Nr, Vater_Nr, Partner_Nr, Lebend, Generation)
    VALUES (25, 31, '22.07.1998', null, null, null, 1, 0);
	
INSERT INTO Personen (Vorname_Nr, Familie_Nr, Geburtsdatum, Mutter_Nr, Vater_Nr, Partner_Nr, Lebend, Generation)
    VALUES (32, 44, '14.07.1999', null, null, null, 1, 0);

--UPDATE Personen SET Partner_Nr = 8 WHERE Nummer = 1;    
--UPDATE Personen SET Partner_Nr = 1 WHERE Nummer = 8;
    
--INSERT into personen (vorname_nr, familie_nr, geburtsdatum, mutter_nr, vater_nr, partner_nr, Lebend)
--    VALUES (6, 4, '30.05.2016', 8, 1, null, 1);

--INSERT INTO PERSONEN (VORNAME_NR, FAMILIE_NR, GEBURTSDATUM, MUTTER_NR, VATER_NR, PARTNER_NR, LEBEND) 
--    VALUES (12, 4, '16.04.2017', 8, 1, NULL, 1);