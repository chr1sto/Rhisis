QUEST_GETBACK_PORTRAIT = {
	title = 'IDS_PROPQUEST_INC_000935',
	character = 'MaFl_Ancimys',
	start_requirements = {
		min_level = 26,
		max_level = 30,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
	},
	rewards = {
		gold = 0,
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000936',
			'IDS_PROPQUEST_INC_000937',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000938',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000939',
		},
		completed = {
			'IDS_PROPQUEST_INC_000940',
			'IDS_PROPQUEST_INC_000941',
			'IDS_PROPQUEST_INC_000942',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000943',
		},
	}
}
