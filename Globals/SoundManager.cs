
using System.Collections.Generic;
using Godot;

public partial class SoundManager : Node
{
	public const string SOUND_MAIN_MENU = "main";
	public const string SOUND_SUCCESS = "success";
	public const string SOUND_GAME_OVER = "gameover";
	public const string SOUND_IN_GAME = "ingame";
	public const string SOUND_CATCH_GEM = "catchgem";
	public const string SOUND_EXPLODE = "explode";

	//static and readonly: to open imediately when the first instance of the class is created then it will never be loaded again
	private static readonly Dictionary<string, AudioStream> SOUNDS = new Dictionary<string, AudioStream> {
		{SOUND_MAIN_MENU, GD.Load<AudioStream>("res://assets/sound/bgm_action_5.mp3")},
		{SOUND_SUCCESS, GD.Load<AudioStream>("res://assets/sound/jazzyfrenchy.mp3")},
		{SOUND_GAME_OVER, GD.Load<AudioStream>("res://assets/sound/game_over.wav")},
		{SOUND_CATCH_GEM, GD.Load<AudioStream>("res://assets/sound/spell1_0.wav")},
		{SOUND_IN_GAME, GD.Load<AudioStream>("res://assets/sound/bgm_action_5.mp3")},
		{SOUND_EXPLODE, GD.Load<AudioStream>("res://assets/sound/explode.wav")}
	};
	public static SoundManager Instance {get; private set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// play a sound by key
	public static void PlaySound(AudioStreamPlayer player, string key) {
		if (!SOUNDS.ContainsKey(key)) {
			return;
		}

		player.Stop();
		player.Stream = SOUNDS[key];
		player.Play();
	}

	//overloaded play a sound by key
	public static void PlaySound(AudioStreamPlayer2D player, string key) {
		if (!SOUNDS.ContainsKey(key)) return;
		player.Stop();
		player.Stream = SOUNDS[key];
		player.Play();
	}

	public static void PlayGemCatch(AudioStreamPlayer2D player) {
		PlaySound(player, SOUND_CATCH_GEM);
	}

	public static void PlayExplosion(AudioStreamPlayer2D player) {
		PlaySound(player, SOUND_EXPLODE);
	}
}
