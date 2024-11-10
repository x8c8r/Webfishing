extends Control

signal _pressed

var title = "" setget ,get_title

func _setup(title):
	$Button.text = title
	self.title = title
	
func get_title():
	return title

func _on_Button_pressed():
	emit_signal("_pressed")
