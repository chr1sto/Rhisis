QUEST_GETBACK_LUDASAFE = {
	title = 'IDS_PROPQUEST_INC_000831',
	character = 'MaFl_Luda',
	start_requirements = {
		min_level = 30,
		max_level = 60,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
	},
	rewards = {
		gold = 0,
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000832',
			'IDS_PROPQUEST_INC_000833',
			'IDS_PROPQUEST_INC_000834',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000835',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000836',
		},
		completed = {
			'IDS_PROPQUEST_INC_000837',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000838',
		},
	}
}
