QUEST_SPECHILD = {
	title = 'IDS_PROPQUEST_INC_001096',
	character = 'MaSa_Porgo',
	start_requirements = {
		min_level = 20,
		max_level = 40,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
	},
	rewards = {
		gold = 0,
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_001097',
			'IDS_PROPQUEST_INC_001098',
			'IDS_PROPQUEST_INC_001099',
			'IDS_PROPQUEST_INC_001100',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_001101',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_001102',
		},
		completed = {
			'IDS_PROPQUEST_INC_001103',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_001104',
		},
	}
}
