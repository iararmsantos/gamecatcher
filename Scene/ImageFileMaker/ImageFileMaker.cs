using Godot;

public partial class ImageFileMaker : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		string path = "res://assets/gems";
		DirAccess dir = DirAccess.Open(path);

		if (dir == null) {
			GD.Print("Directory not found: " + path);
			return;
		}

		ImageFilesList list = new ImageFilesList();

		string[] fileNames = dir.GetFiles();

		foreach (var fileName in fileNames)
		{
			if (!fileName.Contains(".import")) {
				list.FileNames.Add($"{path}/{fileName}");
				GD.Print($"File added: {path}/{fileName}");
			}
		}

		ResourceSaver.Save(list, "res://Resources/ImageFilesList.tres");
	}

}
