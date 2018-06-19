using UnityEngine;

namespace Dumpster.Core.Life.Chips {

	public abstract class ShutdownChip : MonoBehaviour {

		[SerializeField] protected Eden.Life.BlackBox _blackBox;

		private void Awake () {
			_blackBox.OnShutDown += OnShutdown;
		}

		protected virtual void OnShutdown () {}
	}
}
