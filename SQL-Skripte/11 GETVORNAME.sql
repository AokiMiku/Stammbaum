CREATE OR ALTER PROCEDURE GETVORNAME (
    vornamenr integer)
returns (
    geschlecht char(1),
    vorname varchar(40))
as
begin
  vorname = (SELECT v.Vorname
                FROM Vornamen v
                WHERE v.Nummer = :vornamenr);
  geschlecht = (SELECT v.Geschlecht
                FROM Vornamen v
                WHERE v.Nummer = :vornamenr);
                suspend;
end
