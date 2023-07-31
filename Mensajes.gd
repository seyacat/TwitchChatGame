extends Label


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	$"../TwitchChat".connect("new_message",self,"get_message")
	
	pass # Replace with function body.

func get_message(data):
	
	if data.has("cmd") && data.has("username"):
		modulate.a = 1;
		if data.cmd == "JOIN":
			text = "Bienvenido "+ data.username
		if data.cmd == "PART":
			text = "Adios "+ data.username
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if(modulate.a > 0):
		modulate.a = modulate.a - delta/10;
	else:
		modulate.a = 0
	pass
