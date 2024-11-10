extends Node

const MENU_BUTTON = preload("res://mods/Waypoints/Button/waypoints_button.tscn")
const MENU_SCENE = preload("res://mods/Waypoints/WaypointsMenu/waypoints_menu.tscn")

const Waypoint = preload("res://mods/Waypoints/waypoint.gd")

var waypoints:Dictionary = {}

var player

func _ready():
	_load()
	get_tree().connect("node_added", self, "_node_added")

func teleport(waypoint:String):
	if (player != null):
		_teleport_to_point(player, waypoint)

func create(title:String):
	if _get_waypoint(title) != null:
		PlayerData._send_notification("Waypoint already exists!", 1)
		return
	if player == null:
		PlayerData._send_notification("Could not create!", 1)
		return
	var waypoint = Waypoint.new(title, player.global_transform.origin)
	waypoints[title] = waypoint.position
	
	_save()
	
func delete(title:String):
	if _get_waypoint(title) != null:
		waypoints.erase(title)
	_save()

func _teleport_to_point(actor:Actor, waypoint_key:String):
	var waypoint = _get_waypoint(waypoint_key)
	if (waypoint == null): return
	
	actor.locked = true
	SceneTransition._fake_scene_change()
	yield (SceneTransition, "_finished")
	actor.global_transform.origin = waypoint
	yield (get_tree().create_timer(0.3), "timeout")
	actor.locked = false

func _get_waypoint(waypoint) -> Waypoint:
	if waypoints.has(waypoint):
		return waypoints.get(waypoint)
	else:
		return null
		
func _save():
	var way = waypoints.duplicate(true)
	for point in way.keys():
		way[point] = {
			"x": way[point].x,
			"y": way[point].y,
			"z": way[point].z
		}
	var json = JSON.print(way)
	var PATH = "user://waypoints.sav"
	
	var save = File.new()
	save.open(PATH, File.WRITE)
	save.store_string(json)
	save.close()
	
func _load():
	var save = File.new()
	var PATH = "user://waypoints.sav"
	
	var save_exists = save.file_exists(PATH)
	
	if !save_exists:
		save.close()
		return
		
	save.open(PATH, File.READ)
		
	var way:Dictionary = JSON.parse(save.get_line()).result	
	for point in way.keys():
		var wp = way[point]
		waypoints[point] = Vector3(wp.x, wp.y, wp.z)
		

# -=-=-=-=-
# events hi blueberrywolf
# -=-=-=-=-

func _node_added(node):
	if get_tree().current_scene.name != "world": return
	
	if node.name == "main_map":	
		get_tree().current_scene.get_node("Viewport/main/entities").connect("child_entered_tree", self, "_entity_added")
		
func _entity_added(node):
	if node.name == "player":
		player = node
	
func _player_removed():
	player = null
