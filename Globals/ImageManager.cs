using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class ImageManager : Node
{
	public static ImageManager Instance {get; private set;}
	private Dictionary<Gem.GemType, Texture2D> _gemTextures = new();

	private Godot.Collections.Array<ItemImage> _itemImages = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		LoadItemImages();
	}

	private void AddFileToList(string filePath) {
		var itemImage = new ItemImage {
			ItemName = Path.GetFileName(filePath),
			ItemTexture = GD.Load<Texture2D>(filePath)
		};

		_itemImages.Add(itemImage);
	}

	private void LoadItemImages() {
		var imageResources = GD.Load<ImageFilesList>("res://Resources/ImageFilesList.tres");

		foreach (var filePath in imageResources.FileNames)
		{
			AddFileToList(filePath);
		}

		BuildGemTextures();
	}


	public static ItemImage GetRandomItemImage() {
		return Instance._itemImages.PickRandom();
	}

	public static ItemImage GetImage(int index) {
		return Instance._itemImages[index];
	}

	public static void ShuffleImages() {
		Instance._itemImages.Shuffle();
	}	

	private void BuildGemTextures()
	{
		foreach (Gem.GemType type in Enum.GetValues(typeof(Gem.GemType)))
		{
			// Assumes filenames like "Red.png", "Blue.png", etc.
			string expectedFileName = $"{type}.png";

			var itemImage = _itemImages.FirstOrDefault(i => i.ItemName.Equals(expectedFileName, StringComparison.OrdinalIgnoreCase));
			
			if (itemImage != null)
			{
				_gemTextures[type] = itemImage.ItemTexture;
			}
			else
			{
				GD.PrintErr($"No texture found for gem type: {type} (expected file: {expectedFileName})");
			}
		}
	}

	public static Texture2D GetTextureForType(Gem.GemType type)
	{
		if (Instance._gemTextures.TryGetValue(type, out var texture))
		{
			return texture;
		}
		
		GD.PrintErr($"Texture not found for GemType: {type}");
		return null;
	}

}
