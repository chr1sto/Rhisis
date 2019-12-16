QUEST_CLUE2_PORTRAIT = {
	title = 'IDS_PROPQUEST_INC_000921',
	character = 'MaFl_Tucani',
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
			'IDS_PROPQUEST_INC_000922',
			'IDS_PROPQUEST_INC_000923',
			'IDS_PROPQUEST_INC_000924',
			'IDS_PROPQUEST_INC_000925',
			'IDS_PROPQUEST_INC_000926',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000927',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000928',
		},
		completed = {
			'IDS_PROPQUEST_INC_000929',
			'IDS_PROPQUEST_INC_000930',
			'IDS_PROPQUEST_INC_000931',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000932',
		},
	}
}
