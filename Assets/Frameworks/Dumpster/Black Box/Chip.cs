using UnityEngine;

namespace Dumpster.Core.Life {

	public abstract class Chip<T> : MonoBehaviour {

		private BlackBox<T> _blackBox;
		
		public void Install ( BlackBox<T> blackbBox ) {
			
			_blackBox = blackbBox;

			_blackBox.OnInit += OnInit;
			_blackBox.OnRun += OnRun;
		}
		protected virtual void OnInit () {}
		protected virtual void OnRun () {}
	}
}