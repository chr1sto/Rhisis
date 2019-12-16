QUEST_TRUTH_PAST = {
	title = 'IDS_PROPQUEST_INC_001049',
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
			'IDS_PROPQUEST_INC_001050',
			'IDS_PROPQUEST_INC_001051',
			'IDS_PROPQUEST_INC_001052',
			'IDS_PROPQUEST_INC_001053',
			'IDS_PROPQUEST_INC_001054',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_001055',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_001056',
		},
		completed = {
			'IDS_PROPQUEST_INC_001057',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_001058',
		},
	}
}
