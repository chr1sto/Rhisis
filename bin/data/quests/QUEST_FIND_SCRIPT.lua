QUEST_FIND_SCRIPT = {
	title = 'IDS_PROPQUEST_INC_000869',
	character = 'MaDa_Colar',
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
			'IDS_PROPQUEST_INC_000870',
			'IDS_PROPQUEST_INC_000871',
			'IDS_PROPQUEST_INC_000872',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000873',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000874',
		},
		completed = {
			'IDS_PROPQUEST_INC_000875',
			'IDS_PROPQUEST_INC_000876',
			'IDS_PROPQUEST_INC_000877',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000878',
		},
	}
}
