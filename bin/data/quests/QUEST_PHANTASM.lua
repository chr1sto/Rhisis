QUEST_PHANTASM = {
	title = 'IDS_PROPQUEST_INC_001721',
	character = 'MaFl_DrEstly',
	start_requirements = {
		min_level = 20,
		max_level = 35,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
	},
	rewards = {
		gold = 0,
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_001722',
			'IDS_PROPQUEST_INC_001723',
			'IDS_PROPQUEST_INC_001724',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_001725',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_001726',
		},
		completed = {
			'IDS_PROPQUEST_INC_001727',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_001728',
		},
	}
}
