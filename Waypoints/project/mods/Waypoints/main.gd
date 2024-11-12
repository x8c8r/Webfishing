extends Node

const MOD_VERSION = "1.0.1"

const MENU_BUTTON = preload("res://mods/Waypoints/Button/waypoints_button.tscn")
const MENU_SCENE = preload("res://mods/Waypoints/WaypointsMenu/waypoints_menu.tscn")

const Waypoint = preload("res://mods/Waypoints/waypoint.gd")

var waypoints:Dictionary = {}

var player
var menu_opened:bool = false

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
		
	var pos = player.global_transform.origin
	var zone = PlayerData.player_saved_zone if PlayerData.player_saved_zone != "" else "main_zone"
	var waypoint = Waypoint.new(title, pos, zone)
	waypoints[title] = waypoint
	
	_save()
	
func delete(title:String):
	if _get_waypoint(title) != null:
		waypoints.erase(title)
	_save()

func _teleport_to_point(actor:Actor, waypoint_key:String):
	var waypoint:Waypoint = _get_waypoint(waypoint_key)
	if (waypoint == null): return
	
	var zone = PlayerData.player_saved_zone if PlayerData.player_saved_zone != "" else "main_zone"
	
	if zone != waypoint.zone:
		var zone_name = waypoint.zone.replace("_", " ").capitalize()
		PlayerData._send_notification("You are not in the same zone as the waypoint! The zone of the waypoint is \""+zone_name+"\"", 1)
		return
	
	actor.locked = true
	SceneTransition._fake_scene_change()
	yield (SceneTransition, "_finished")
	actor.global_transform.origin = waypoint.position
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
			"position": {
				"x": way[point].position.x,
				"y": way[point].position.y,
				"z": way[point].position.z
			},
			"zone": way[point].zone,
		}
	var json = JSON.print(way)
	print(json)
	var PATH = "user://waypoints.sav"
	
	var save = File.new()
	save.open(PATH, File.WRITE)
	save.store_string(json+"\n")
	save.store_string(MOD_VERSION)
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
	if !_version_check(save.get_line()): return
	
	for point in way.keys():
		var wp = way[point]
		var pos = Vector3(wp.position.x, wp.position.y, wp.position.z)
		var zone
		if (way[point].has("zone")):
			zone = way[point].zone
		else: zone = ""
		
		waypoints[point] = Waypoint.new(point, pos, zone)
	
func _version_check(line) -> bool:
	if line == "":
		return false
	return true
		

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
