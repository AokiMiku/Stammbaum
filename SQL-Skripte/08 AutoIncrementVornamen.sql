CREATE SEQUENCE GEN_VORNAMEN_ID;

SET TERM ^ ;

create trigger vornamen_bi for vornamen
active before insert position 0
as
begin
  if (new.nummer is null) then
    new.nummer = gen_id(gen_vornamen_id,1);
end^

SET TERM ; ^

