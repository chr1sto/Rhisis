QUEST_FIND_DNAWAG = {
	title = 'IDS_PROPQUEST_INC_000957',
	character = 'MaSa_Heltung',
	start_requirements = {
		min_level = 20,
		max_level = 25,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
	},
	rewards = {
		gold = 0,
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000958',
			'IDS_PROPQUEST_INC_000959',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000960',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000961',
		},
		completed = {
			'IDS_PROPQUEST_INC_000962',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000963',
		},
	}
}
