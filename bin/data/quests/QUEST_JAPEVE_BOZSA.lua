QUEST_JAPEVE_BOZSA = {
	title = 'IDS_PROPQUEST_INC_000642',
	character = 'MaSa_Furan',
	end_character = '',
	start_requirements = {
		min_level = 1,
		max_level = 129,
		job = { 'JOB_VAGRANT', 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN', 'JOB_KNIGHT', 'JOB_BLADE', 'JOB_JESTER', 'JOB_RANGER', 'JOB_RINGMASTER', 'JOB_BILLPOSTER', 'JOB_PSYCHIKEEPER', 'JOB_ELEMENTOR' },
		previous_quest = '',
	},
	rewards = {
		gold = 0,
		exp = 0,
		items = {
			{ id = 'II_SYS_SYS_BIN_BOZSAINT', quantity = 1, sex = 'Any' },
		},
	},
	end_conditions = {
		items = {
			{ id = 'II_SYS_SYS_QUE_GRPSAINT', quantity = 20, sex = 'Any', remove = true },
		},
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000643',
			'IDS_PROPQUEST_INC_000644',
			'IDS_PROPQUEST_INC_000645',
			'IDS_PROPQUEST_INC_000646',
			'IDS_PROPQUEST_INC_000647',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000648',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000649',
		},
		completed = {
			'IDS_PROPQUEST_INC_000650',
			'IDS_PROPQUEST_INC_000651',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000652',
			'IDS_PROPQUEST_INC_000653',
		},
	}
}
