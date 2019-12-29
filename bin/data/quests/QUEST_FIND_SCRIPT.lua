QUEST_FIND_SCRIPT = {
	title = 'IDS_PROPQUEST_INC_000869',
	character = 'MaDa_Colar',
	end_character = 'MaSa_Troupemember1',
	start_requirements = {
		min_level = 37,
		max_level = 60,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
		previous_quest = 'QUEST_DISAPP_SCRIPT',
	},
	rewards = {
		gold = 0,
	},
	end_conditions = {
		items = {
			{ id = 'II_SYS_SYS_QUE_SCRIPT', quantity = 6, sex = 'Any', remove = true },
		},
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
