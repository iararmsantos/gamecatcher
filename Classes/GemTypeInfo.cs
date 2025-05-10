
public class GemTypeInfo
{
    public bool IsSpecial { get; set; }
    public int Rarity { get; set; } = 1; // Higher = more common
    public string DisplayName { get; set; }
    public float InitialSpeed { get; set; } = 90f;
}
