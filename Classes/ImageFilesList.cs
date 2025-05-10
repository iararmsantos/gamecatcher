
using Godot;

[GlobalClass]
public partial class ImageFilesList : Resource
{
    [Export] public Godot.Collections.Array<string> FileNames = new();
}
