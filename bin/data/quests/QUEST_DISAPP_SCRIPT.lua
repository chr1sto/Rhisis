QUEST_DISAPP_SCRIPT = {
	title = 'IDS_PROPQUEST_INC_000855',
	character = 'MaSa_Troupemember1',
	start_requirements = {
		min_level = 37,
		max_level = 60,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
	},
	rewards = {
		gold = 0,
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000856',
			'IDS_PROPQUEST_INC_000857',
			'IDS_PROPQUEST_INC_000858',
			'IDS_PROPQUEST_INC_000859',
			'IDS_PROPQUEST_INC_000860',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000861',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000862',
		},
		completed = {
			'IDS_PROPQUEST_INC_000863',
			'IDS_PROPQUEST_INC_000864',
			'IDS_PROPQUEST_INC_000865',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000866',
		},
	}
}
