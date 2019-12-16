QUEST_VOCASS_TRN1 = {
	title = 'IDS_PROPQUEST_INC_000735',
	character = 'MaFl_Elic',
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
			'IDS_PROPQUEST_INC_000736',
			'IDS_PROPQUEST_INC_000737',
			'IDS_PROPQUEST_INC_000738',
			'IDS_PROPQUEST_INC_000739',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000740',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000741',
		},
		completed = {
			'IDS_PROPQUEST_INC_000742',
			'IDS_PROPQUEST_INC_000743',
			'IDS_PROPQUEST_INC_000744',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000745',
		},
	}
}
