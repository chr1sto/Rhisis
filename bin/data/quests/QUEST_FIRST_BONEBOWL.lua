QUEST_FIRST_BONEBOWL = {
	title = 'IDS_PROPQUEST_INC_000978',
	character = 'MaSa_Bowler',
	end_character = 'MaSa_Bowler',
	start_requirements = {
		min_level = 44,
		max_level = 60,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
		previous_quest = 'QUEST_ORIGIN_BONEBOWL',
	},
	rewards = {
		gold = 0,
		exp = 133864,
		items = {
			{ id = 'II_SYS_SYS_QUE_BONEBOWL', quantity = 1, sex = 'Any' },
		},
	},
	end_conditions = {
		items = {
			{ id = 'II_SYS_SYS_QUE_BLKMAR', quantity = 10, sex = 'Any', remove = true },
			{ id = 'II_SYS_SYS_QUE_BLKSPL', quantity = 5, sex = 'Any', remove = true },
			{ id = 'II_SYS_SYS_QUE_BLKHER', quantity = 5, sex = 'Any', remove = true },
		},
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000979',
			'IDS_PROPQUEST_INC_000980',
			'IDS_PROPQUEST_INC_000981',
			'IDS_PROPQUEST_INC_000982',
			'IDS_PROPQUEST_INC_000983',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000984',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000985',
		},
		completed = {
			'IDS_PROPQUEST_INC_000986',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000987',
		},
	}
}
