QUEST_VOCMER_TRN3 = {
	title = 'IDS_PROPQUEST_INC_000711',
	character = 'MaFl_Langdrong',
	start_requirements = {
		min_level = 15,
		max_level = 15,
		job = { 'JOB_VAGRANT' },
	},
	rewards = {
		gold = 0,
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000712',
			'IDS_PROPQUEST_INC_000713',
			'IDS_PROPQUEST_INC_000714',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000715',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000716',
		},
		completed = {
			'IDS_PROPQUEST_INC_000717',
			'IDS_PROPQUEST_INC_000718',
			'IDS_PROPQUEST_INC_000719',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000720',
		},
	}
}
