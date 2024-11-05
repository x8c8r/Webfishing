extends Node

onready var TackleBox := $"/root/TackleBox"
var quick_menu = preload("res://mods/YAAM/ui/yaam_quick_menu.tscn")

var config:Dictionary = {
	"Autocast": true,
	"Autoreel": true,
	"Automash": true,
	"CastDistance": 1.5
}

func _ready():
	TackleBox.connect("mod_config_updated", self, "_update_config")
	
	_update_config("YAAM", TackleBox.get_mod_config("YAAM"));

func _update_config(mod_id, config):
	if mod_id == "YAAM":
		self.config = config
