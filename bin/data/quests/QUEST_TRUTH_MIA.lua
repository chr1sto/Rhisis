QUEST_TRUTH_MIA = {
	title = 'IDS_PROPQUEST_INC_001027',
	character = 'MaSa_Porgo',
	start_requirements = {
		min_level = 20,
		max_level = 30,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
	},
	rewards = {
		gold = 0,
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_001028',
			'IDS_PROPQUEST_INC_001029',
			'IDS_PROPQUEST_INC_001030',
			'IDS_PROPQUEST_INC_001031',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_001032',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_001033',
		},
		completed = {
			'IDS_PROPQUEST_INC_001034',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_001035',
		},
	}
}
