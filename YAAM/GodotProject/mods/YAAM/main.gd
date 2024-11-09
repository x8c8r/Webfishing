extends Node

onready var TackleBox := $"/root/TackleBox"
var quick_menu = preload("res://mods/YAAM/ui/yaam_quick_menu.tscn")

var config:Dictionary = {
	"Autocast": true,
	"Autoreel": true,
	"Automash": true,
	"CastDistance": 1.5,
	
	"AutocastRequiresBait": true,
	"BaitAutoRefill": true
}

func _ready():
	TackleBox.connect("mod_config_updated", self, "_update_config")
	
	_init_config(TackleBox.get_mod_config("YAAM"));
	
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
