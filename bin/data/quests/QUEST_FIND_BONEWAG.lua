QUEST_FIND_BONEWAG = {
	title = 'IDS_PROPQUEST_INC_000946',
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
			'IDS_PROPQUEST_INC_000947',
			'IDS_PROPQUEST_INC_000948',
			'IDS_PROPQUEST_INC_000949',
			'IDS_PROPQUEST_INC_000950',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000951',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000952',
		},
		completed = {
			'IDS_PROPQUEST_INC_000953',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000954',
		},
	}
}
