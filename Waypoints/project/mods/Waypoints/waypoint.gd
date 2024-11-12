extends Spatial

var title:String
var position:Vector3
var zone:String

func _init(title:String, position:Vector3, zone:String):
	self.title = title
	self.position = position
	self.zone = zone
