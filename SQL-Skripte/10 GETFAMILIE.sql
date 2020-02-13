CREATE OR ALTER PROCEDURE GETFAMILIE (
    personnr integer)
returns (
    familienname varchar(40))
as
declare variable familienr integer;
begin
  FamilieNr = (SELECT p.familie_nr
                FROM Personen p
                WHERE p.nummer = :personnr);
  Familienname = (SELECT f.Familienname
                    FROM Familien f
                    WHERE f.Nummer = :familienr);
  suspend;
end
