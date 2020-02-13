CREATE OR ALTER VIEW PERSONENV(
    NUMMER,
    VORNAME,
    FAMILIENNAME,
    GEBURTSDATUM,
    GENERATION,
    LEBEND,
    BEKOMMTKINDANTAG,
    MUTTER,
    VATER,
    PARTNER)
AS
SELECT p.Nummer, v.Vorname, f.Familienname, p.Geburtsdatum, p.Generation, p.lebend, p.bekommtkindantag, gvm.vorname || ' ' || gfm.familienname as Mutter, gvv.vorname || ' ' || gfv.familienname as Vater, gvp.vorname || ' ' || gfp.familienname as Partner
            FROM Personen p
            LEFT OUTER JOIN Vornamen v ON p.vorname_nr = v.nummer
            LEFT OUTER JOIN Familien f ON p.familie_nr = f.nummer
            LEFT OUTER JOIN Vornamen gvm ON gvm.nummer = (SELECT p2.vorname_nr from Personen p2 WHERE p2.nummer = p.mutter_nr)
            LEFT OUTER JOIN Familien gfm ON gfm.nummer = (SELECT p3.familie_nr FROM Personen p3 WHERE p3.nummer = p.mutter_nr)
            LEFT OUTER JOIN Vornamen gvv ON gvv.nummer = (SELECT p4.vorname_nr from Personen p4 WHERE p4.nummer = p.vater_nr)
            LEFT OUTER JOIN Familien gfv ON gfv.nummer = (SELECT p5.familie_nr FROM Personen p5 WHERE p5.nummer = p.vater_nr)
            LEFT OUTER JOIN Vornamen gvp ON gvp.nummer = (SELECT p6.vorname_nr from Personen p6 WHERE p6.nummer = p.partner_nr)
            LEFT OUTER JOIN Familien gfp ON gfp.nummer = (SELECT p7.familie_nr FROM Personen p7 WHERE p7.nummer = p.partner_nr)
            ORDER BY p.nummer
;