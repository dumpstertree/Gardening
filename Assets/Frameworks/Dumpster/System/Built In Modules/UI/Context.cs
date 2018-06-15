using UnityEngine;
using System.Collections.Generic;

namespace Dumpster.Core.BuiltInModules.UI {
	
	public class Context {

		public string Identifier {
			get; private set;
		}
		private List<Panel> _registeredPanels;

		public Context ( string contextIdentidier, List<Panel> panels ) {
			
			Identifier = contextIdentidier;
			_registeredPanels = new List<Panel>();

			foreach ( Panel panel in panels ) {
				
				if ( !_registeredPanels.Contains( panel ) ) {
					_registeredPanels.Add( panel );
				}
			}

			Init ();
		}

		private void Init () {
			
			foreach ( Panel p in _registeredPanels ) {
				p.Init ();
			}
		}

		public virtual void Destroy () {

			foreach ( Panel p in _registeredPanels ) {
				GameObject.Destroy( p.gameObject );
			}	
		}
		public virtual void Present () {

			foreach ( Panel p in _registeredPanels ) {
				p.Present ();
			}
		}
		public virtual void Dismiss () {
			
			foreach ( Panel p in _registeredPanels ) {
				p.Dismiss ();
			}
		}
		public virtual void EnterFocus () {
			
			foreach ( Panel p in _registeredPanels ) {
				p.EnterFocus ();
			}
		}
		public virtual void ExitFocus () {

			foreach ( Panel p in _registeredPanels ) {
				p.ExitFocus ();
			}
		}
	}
}