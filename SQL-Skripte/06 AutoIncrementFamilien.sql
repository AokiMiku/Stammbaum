CREATE SEQUENCE GEN_FAMILIEN_ID;

SET TERM ^ ;

create trigger familien_bi for familien
active before insert position 0
as
begin
  if (new.nummer is null) then
    new.nummer = gen_id(gen_familien_id,1);
end^

SET TERM ; ^

