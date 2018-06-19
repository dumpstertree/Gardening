using UnityEngine;

namespace Dumpster.Core.Life.Chips {
	
	public abstract class StartupChip : MonoBehaviour {

		[SerializeField] private Eden.Life.BlackBox _blackBox;

		private void Awake () {
			_blackBox.OnStartup += OnStartup;
		}

		protected virtual void OnStartup () {}
	}
}