QUEST_JAPEVE_BOZDA = {
	title = 'IDS_PROPQUEST_INC_000656',
	character = 'MaDa_Furan',
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
			{ id = 'II_SYS_SYS_BIN_BOZDARKON', quantity = 1, sex = 'Any' },
		},
	},
	end_conditions = {
		items = {
			{ id = 'II_SYS_SYS_QUE_GRPDARKON', quantity = 20, sex = 'Any', remove = true },
		},
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000657',
			'IDS_PROPQUEST_INC_000658',
			'IDS_PROPQUEST_INC_000659',
			'IDS_PROPQUEST_INC_000660',
			'IDS_PROPQUEST_INC_000661',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000662',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000663',
		},
		completed = {
			'IDS_PROPQUEST_INC_000664',
			'IDS_PROPQUEST_INC_000665',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000666',
			'IDS_PROPQUEST_INC_000667',
		},
	}
}
