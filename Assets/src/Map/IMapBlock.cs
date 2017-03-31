public interface IMapBlock
{
	bool BlockMove { get; }
	bool BlockSight { get; }
	bool Interactable { get; }
	Interactable MyInteractable { get; }
}
