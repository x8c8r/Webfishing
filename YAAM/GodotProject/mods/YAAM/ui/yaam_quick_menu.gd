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
	$Panel/cast_slider/Label.text = "Cast Distance: " + str(YAAM.config.CastDistance)
	$Panel/cast_slider.value = YAAM.config.CastDistance

func _on_Button_pressed():
	$Button.visible = false
	$Panel.visible = true

func _on_exit_pressed():
	$Panel.visible = false
	$Button.visible = true

func _on_autocast_pressed():	
	YAAM.config.Autocast = !YAAM.config.Autocast
	_update_config()

func _on_autoreel_pressed():
	YAAM.config.Autoreel = !YAAM.config.Autoreel
	_update_config()

func _on_automash_pressed():
	YAAM.config.Automash = !YAAM.config.Automash
	_update_config()

func _on_cast_slider_value_changed(value):
	YAAM.config.CastDistance = value
	_update_config()
	
func _update_config():
	TackleBox.set_mod_config("YAAM", YAAM.config)	
	
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
	
