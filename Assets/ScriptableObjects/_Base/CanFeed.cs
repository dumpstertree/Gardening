using UnityEngine;

partial class InventoryItem {

	public bool CanFeed { get{ return _canFeed; } }

	[HeaderAttribute("Feed")]
	[SerializeField] private bool _canFeed;
	[SerializeField] private FeedData _feedData;
}
