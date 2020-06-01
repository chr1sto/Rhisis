QUEST_FIND_REDBANGT = {
	title = 'IDS_PROPQUEST_INC_000820',
	character = 'MaFl_Luda',
	end_character = 'MaFl_Luda',
	start_requirements = {
		min_level = 30,
		max_level = 60,
		job = { 'JOB_MERCENARY', 'JOB_ACROBAT', 'JOB_ASSIST', 'JOB_MAGICIAN' },
		previous_quest = '',
	},
	rewards = {
		gold = 0,
		exp = 14258,
	},
	end_conditions = {
		items = {
			{ id = 'II_SYS_SYS_QUE_REDBANGHEAD', quantity = 1, sex = 'Any', remove = true },
		},
	},
	dialogs = {
		begin = {
			'IDS_PROPQUEST_INC_000821',
			'IDS_PROPQUEST_INC_000822',
			'IDS_PROPQUEST_INC_000823',
			'IDS_PROPQUEST_INC_000824',
		},
		begin_yes = {
			'IDS_PROPQUEST_INC_000825',
		},
		begin_no = {
			'IDS_PROPQUEST_INC_000826',
		},
		completed = {
			'IDS_PROPQUEST_INC_000827',
		},
		not_finished = {
			'IDS_PROPQUEST_INC_000828',
		},
	}
}
