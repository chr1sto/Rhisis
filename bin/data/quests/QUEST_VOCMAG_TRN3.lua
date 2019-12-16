QUEST_VOCMAG_TRN3 = {
	title = 'IDS_PROPQUEST_INC_000808',
	character = 'MaFl_Hastan',
	start_requirements = {
		min_level = 15,
		max_level = 15,
		job = { 'JOB_VAGRANT' },
	},
	rewards = {
		gold = 1500,
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000809',
			'IDS_PROPQUEST_INC_000810',
			'IDS_PROPQUEST_INC_000811',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000812',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000813',
		},
		completed = {
			'IDS_PROPQUEST_INC_000814',
			'IDS_PROPQUEST_INC_000815',
			'IDS_PROPQUEST_INC_000816',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000817',
		},
	}
}
