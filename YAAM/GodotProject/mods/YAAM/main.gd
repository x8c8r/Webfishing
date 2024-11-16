extends Node

onready var TackleBox := $"/root/TackleBox"
var quick_menu = preload("res://mods/YAAM/ui/yaam_quick_menu.tscn")

var config:Dictionary = {
	"Autocast": true,
	"Autoreel": true,
	"Automash": true,
	"CastDistance": 1.5,
	
	"AutocastRequiresBait": true,
	"BaitAutoRefill": true,
	
	"CatchQualities": {
		"Normal": true,
		"Shining": true,
		"Glistening": true,
		"Opulent": true,
		"Radiant": true,
		"Alpha": true
	}
}

func _get_quality_data(name:String) -> int:
	match name:
		"Normal":
			return PlayerData.ITEM_QUALITIES.NORMAL
		"Shining":
			return PlayerData.ITEM_QUALITIES.SHINING
		"Glistening":
			return PlayerData.ITEM_QUALITIES.GLISTENING
		"Opulent":
			return PlayerData.ITEM_QUALITIES.OPULENT
		"Radiant":
			return PlayerData.ITEM_QUALITIES.RADIANT
		"Alpha":
			return PlayerData.ITEM_QUALITIES.ALPHA
		_:
			return -1
			
func _get_quality_name(quality:int) -> String:
	match quality:
		PlayerData.ITEM_QUALITIES.NORMAL:
			return "Normal"
		PlayerData.ITEM_QUALITIES.SHINING:
			return "Shining"
		PlayerData.ITEM_QUALITIES.GLISTENING:
			return "Glistening"
		PlayerData.ITEM_QUALITIES.OPULENT:
			return "Opulent"
		PlayerData.ITEM_QUALITIES.RADIANT:
			return "Radiant"
		PlayerData.ITEM_QUALITIES.ALPHA:
			return "Alpha"
		_:
			return "Invalid"

func get_catch_qualities():
	var q = []
	for quality in config.CatchQualities.keys():
		if config.CatchQualities[quality]:
			q.push_front(_get_quality_data(quality))
	print(q)
	return q

func _ready():
	TackleBox.connect("mod_config_updated", self, "_update_config")
	
	_init_config(TackleBox.get_mod_config("YAAM"));
	pass
	
func _init_config(conf:Dictionary):
	if config.size() != conf.size():
		for key in config.keys():
			if !conf.has(key):
				conf[key] = config[key]
	config = conf
	print(conf)

func _update_config(mod_id, config):
	if mod_id == "YAAM":
		self.config = config
