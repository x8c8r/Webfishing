extends Control

onready var TackleBox := $"/root/TackleBox"
onready var YAAM := $"/root/YAAM"

func _ready():
	_update_button_text()
	_update_quality_checkboxes()
	
	TackleBox.connect("mod_config_updated", self, "_update_ui")
	
func _on_Button_pressed():
	$Button.visible = false
	$Panel.visible = true

func _on_exit_pressed():
	$Panel.visible = false
	$Panel2.visible = false
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
	
func _on_baitautorefill_pressed():
	YAAM.config.BaitAutoRefill = !YAAM.config.BaitAutoRefill
	_update_config()
	
func _on_selectqualities_pressed():
	$Panel2.visible = !$Panel2.visible
	_update_config()
	
func _on_quality_checkbox_pressed(qual):
	YAAM.config.CatchQualities[qual] = !YAAM.config.CatchQualities[qual]
	_update_config()
	
func _update_config():
	TackleBox.set_mod_config("YAAM", YAAM.config)	
	pass
	
func _update_ui(id, config):
	if id != "YAAM": return
	
	_update_button_text()
	_update_quality_checkboxes()
	
# WELCOME TO THE GARBAGE DISPOSAL
func _update_button_text():
	$Panel/autocast.text = "Autocast: " + ("On" if YAAM.config.Autocast else "Off")
	$Panel/autoreel.text = "Autoreel: " + ("On" if YAAM.config.Autoreel else "Off")
	$Panel/automash.text = "Automash: " + ("On" if YAAM.config.Automash else "Off")
	$Panel/cast_slider/Label.text = "Cast Distance: " + str(YAAM.config.CastDistance)
	$Panel/cast_slider.value = YAAM.config.CastDistance
	$Panel/baitautorefill.text = "Bait Auto Refill: " + ("On" if YAAM.config.BaitAutoRefill else "Off")
	
func _update_quality_checkboxes():	
	$Panel2/normal.pressed = YAAM.config.CatchQualities.Normal
	$Panel2/shining.pressed = YAAM.config.CatchQualities.Shining
	$Panel2/glistening.pressed = YAAM.config.CatchQualities.Glistening
	$Panel2/opulent.pressed = YAAM.config.CatchQualities.Opulent
	$Panel2/radiant.pressed = YAAM.config.CatchQualities.Radiant
	$Panel2/alpha.pressed = YAAM.config.CatchQualities.Alpha

