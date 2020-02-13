CREATE SEQUENCE GEN_EINSTELLUNGEN_ID;

SET TERM ^ ;

create trigger einstellungen_bi for einstellungen
active before insert position 0
as
begin
  if (new.nummer is null) then
    new.nummer = gen_id(gen_einstellungen_id,1);
end^

SET TERM ; ^

