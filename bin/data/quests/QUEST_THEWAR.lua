QUEST_THEWAR = {
	title = 'IDS_PROPQUEST_INC_001743',
	character = 'MaFl_DrEstly',
	end_character = 'MaFl_DrEstly',
	start_requirements = {
		min_level = 20,
		max_level = 35,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
	},
	rewards = {
		gold = 0,
	},
	end_conditions = {
		monsters = {
			{ id = 'MI_VICEVEDUQUE', quantity = 5 },
		},
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_001744',
			'IDS_PROPQUEST_INC_001745',
			'IDS_PROPQUEST_INC_001746',
			'IDS_PROPQUEST_INC_001747',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_001748',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_001749',
		},
		completed = {
			'IDS_PROPQUEST_INC_001750',
			'IDS_PROPQUEST_INC_001751',
			'IDS_PROPQUEST_INC_001752',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_001753',
		},
	}
}
