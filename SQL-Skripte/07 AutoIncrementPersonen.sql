CREATE SEQUENCE GEN_PERSONEN_ID;

SET TERM ^ ;

create trigger personen_bi for personen
active before insert position 0
as
begin
  if (new.nummer is null) then
    new.nummer = gen_id(gen_personen_id,1);
end^

SET TERM ; ^

