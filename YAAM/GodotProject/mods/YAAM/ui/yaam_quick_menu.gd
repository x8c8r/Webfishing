extends Control

onready var TackleBox := $"/root/TackleBox"
onready var YAAM := $"/root/YAAM"

func _ready():
	_update_button_text("", "")
	_setup_sounds()
	
	TackleBox.connect("mod_config_updated", self, "_update_button_text")
	
func _update_button_text(a, b):
	$Panel/autocast.text = "Autocast: " + ("On" if YAAM.config.Autocast else "Off")
	$Panel/autoreel.text = "Autoreel: " + ("On" if YAAM.config.Autoreel else "Off")
	$Panel/automash.text = "Automash: " + ("On" if YAAM.config.Automash else "Off")


func _on_Button_pressed():
	$Button.visible = false
	$Panel.visible = true
	pass # Replace with function body.


func _on_exit_pressed():
	$Panel.visible = false
	$Button.visible = true

func _on_autocast_pressed():
	var conf = {
		"Autocast": !YAAM.config.Autocast,
		"Autoreel": YAAM.config.Autoreel,
		"Automash": YAAM.config.Automash
	}
	
	TackleBox.set_mod_config("YAAM", conf)
	#_update_config(!YAAM.config.Autocast, null, null)


func _on_autoreel_pressed():
	var conf = {
		"Autocast": YAAM.config.Autocast,
		"Autoreel": !YAAM.config.Autoreel,
		"Automash": YAAM.config.Automash
	}
	
	TackleBox.set_mod_config("YAAM", conf)
	#_update_config(null, !YAAM.config.Autoreel, null)


func _on_automash_pressed():
	var conf = {
		"Autocast": YAAM.config.Autocast,
		"Autoreel": YAAM.config.Autoreel,
		"Automash": !YAAM.config.Automash
	}
	
	TackleBox.set_mod_config("YAAM", conf)
	#_update_config(null, null, !YAAM.config.Automash)

	
func _update_config(cast, reel, mash):
	if cast == null: cast = YAAM.config.Autocast
	if reel == null: reel = YAAM.config.Autoreel
	if mash == null: mash = YAAM.config.Automash
	
	var conf = {
		"Autocast": cast,
		"Autoreel": reel,
		"Automash": mash
	}
	
	TackleBox.set_mod_config("YAAM", conf)
	
	
func _setup_sounds():
	$Button.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	if $Button.has_signal("button_down"): $Button.connect("button_down", GlobalAudio, "_play_sound", ["button_down"])
	if $Button.has_signal("button_up"): $Button.connect("button_up", GlobalAudio, "_play_sound", ["button_up"])

	$Panel/autocast.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	if $Panel/autocast.has_signal("button_down"): $Panel/autocast.connect("button_down", GlobalAudio, "_play_sound", ["button_down"])
	if $Panel/autocast.has_signal("button_up"): $Panel/autocast.connect("button_up", GlobalAudio, "_play_sound", ["button_up"])
	
	$Panel/autoreel.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	if $Panel/autoreel.has_signal("button_down"): $Panel/autoreel.connect("button_down", GlobalAudio, "_play_sound", ["button_down"])
	if $Panel/autoreel.has_signal("button_up"): $Panel/autoreel.connect("button_up", GlobalAudio, "_play_sound", ["button_up"])
	
	$Panel/automash.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	if $Panel/automash.has_signal("button_down"): $Panel/automash.connect("button_down", GlobalAudio, "_play_sound", ["button_down"])
	if $Panel/automash.has_signal("button_up"): $Panel/automash.connect("button_up", GlobalAudio, "_play_sound", ["button_up"])
	
	$Panel/exit.connect("mouse_entered", GlobalAudio, "_play_sound", ["swish"])
	if $Panel/exit.has_signal("button_down"): $Panel/exit.connect("button_down", GlobalAudio, "_play_sound", ["button_down"])
	if $Panel/exit.has_signal("button_up"): $Panel/exit.connect("button_up", GlobalAudio, "_play_sound", ["button_up"])
	
