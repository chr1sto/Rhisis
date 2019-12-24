QUEST_VOCACR_TRN3 = {
	title = 'IDS_PROPQUEST_INC_000503',
	character = 'MaFl_Tucani',
	end_character = 'MaDa_Tailer',
	start_requirements = {
		min_level = 15,
		max_level = 15,
		job = { 'JOB_VAGRANT' },
	},
	rewards = {
		gold = 0,
	},
	end_conditions = {
		items = {
			{ id = 'II_SYS_SYS_QUE_NTSKILL', quantity = 1, sex = 'Any', remove = true },
		},
		monsters = {
			{ id = 'MI_SHURAITURE', quantity = 1 },
		},
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000504',
			'IDS_PROPQUEST_INC_000505',
			'IDS_PROPQUEST_INC_000506',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000507',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000508',
		},
		completed = {
			'IDS_PROPQUEST_INC_000509',
			'IDS_PROPQUEST_INC_000510',
			'IDS_PROPQUEST_INC_000511',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000512',
		},
	}
}
