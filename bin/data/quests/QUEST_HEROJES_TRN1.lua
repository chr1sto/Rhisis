QUEST_HEROJES_TRN1 = {
	title = 'IDS_PROPQUEST_INC_000515',
	character = 'MaDa_Lorein',
	end_character = 'MaDa_Lorein',
	start_requirements = {
		min_level = 60,
		max_level = 60,
		job = { 'JOB_ACROBAT' },
	},
	rewards = {
		gold = {
			min_gold = 500000,
			max_gold = 550000,
		},
	},
	end_conditions = {
		monsters = {
			{ id = 'MI_DRILLER2', quantity = 20 },
		},
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000516',
			'IDS_PROPQUEST_INC_000517',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000518',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000519',
		},
		completed = {
			'IDS_PROPQUEST_INC_000520',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000521',
		},
	}
}
