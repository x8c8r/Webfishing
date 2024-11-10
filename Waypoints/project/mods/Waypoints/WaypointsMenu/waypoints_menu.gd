extends Control

onready var Waypoints = $"/root/Waypoints"

var selected = null

func _ready():
	_update_menu()

func _on_waypoint_delete():
	if selected != null:
		Waypoints.delete(selected.get_title())
		selected = null
		_update_menu()

func _update_menu():
	for child in $Panel/Panel/VBoxContainer.get_children():
		$Panel/Panel/VBoxContainer.remove_child(child)
		
	$teleport.disabled = true
	$delete.disabled = true
		
	for waypoint in Waypoints.waypoints:
		var wp = preload("res://mods/Waypoints/WaypointsMenu/waypoint_button.tscn").instance()
		
		wp._setup(waypoint)
		$Panel/Panel/VBoxContainer.add_child(wp)
		
		wp.connect("_pressed", self, "_select_waypoint", [wp])

func _select_waypoint(wp):
	if selected != null:
		pass
	selected = wp
	
	$teleport.disabled = false
	$delete.disabled = false
	
func _teleport_to_waypoint():
	if selected != null:
		Waypoints.teleport(selected.title)
		
func _open():
	visible = true

func _close():
	visible = false

func _on_close_pressed():
	_close()

func _on_teleport_pressed():
	Waypoints.teleport(selected.get_title())

func _on_add_pressed():
	$add_new.visible = true
	pass

func _on_delete_pressed():
	Waypoints.delete(selected.get_title())
	_update_menu()

func _on_create_pressed():
	Waypoints.create($add_new/Panel/Panel/waypoint_title.text)
	_update_menu()
	$add_new.visible = false

func _on_cancel_pressed():
	$add_new/Panel/Panel/waypoint_title.text = "Waypoint"
	$add_new.visible = false

