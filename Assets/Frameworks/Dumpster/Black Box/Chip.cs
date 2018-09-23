using UnityEngine;

namespace Dumpster.Core.Life {

	public abstract class Chip<T> : MonoBehaviour where T : class {

		public BlackBox<T> BlackBox { 
			get; private set; 
		}
		
		public virtual void Install ( BlackBox<T> blackbBox ) {
			
			BlackBox = blackbBox;

			BlackBox.OnInit += Init;
			BlackBox.OnStartup += Startup;
			BlackBox.OnShutDown += Shutdown;
			BlackBox.OnGetVisual += GetVisual;
		}


		protected virtual void Init () {}
		protected virtual void Startup () {}
		protected virtual void Shutdown () {}
		protected virtual void GetVisual ( T visual ) {}
	}
}