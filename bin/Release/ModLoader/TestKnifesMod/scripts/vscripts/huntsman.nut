    local knife = null;
	
    while(knife = Entities.FindByModel(knife, "models/weapons/v_knife_default_t.mdl"))
        {
                knife.SetModel("models/weapons/v_knife_tactical.mdl");
        }
		
	while(knife = Entities.FindByModel(knife, "models/weapons/v_knife_default_ct.mdl"))
        {
                knife.SetModel("models/weapons/v_knife_tactical.mdl");
        }
