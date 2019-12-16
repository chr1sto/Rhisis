QUEST_PLOT_COLACK = {
	title = 'IDS_PROPQUEST_INC_000841',
	character = 'MaSa_Colack',
	start_requirements = {
		min_level = 35,
		max_level = 60,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
	},
	rewards = {
		gold = 0,
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000842',
			'IDS_PROPQUEST_INC_000843',
			'IDS_PROPQUEST_INC_000844',
			'IDS_PROPQUEST_INC_000845',
			'IDS_PROPQUEST_INC_000846',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000847',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000848',
		},
		completed = {
			'IDS_PROPQUEST_INC_000849',
			'IDS_PROPQUEST_INC_000850',
			'IDS_PROPQUEST_INC_000851',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000852',
		},
	}
}
