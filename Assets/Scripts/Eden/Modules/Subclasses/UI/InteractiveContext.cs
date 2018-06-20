using UnityEngine;
using Dumpster.Core.BuiltInModules.Input;
using Dumpster.Core.BuiltInModules.UI;
using System.Collections.Generic;

namespace Eden.UI {

	public class InteractiveContext : Dumpster.Core.BuiltInModules.UI.Context, IInputReciever<Eden.Input.Package> {

		void IInputReciever<Eden.Input.Package>.RecieveInput ( Eden.Input.Package package ) {
			
			foreach ( InteractivePanel panel in _registeredInteractivePanels ) {
				panel.ReciveInput( package );
			}
		}
		void IInputReciever<Eden.Input.Package>.EnteredInputFocus () {}
		void IInputReciever<Eden.Input.Package>.ExitInputFocus () {}

		public InteractiveContext ( string contextIdentifier, string inputLayer, List<InteractivePanel> interactivePanels, List<Panel> panels ) : base( contextIdentifier, panels )  {

			_registeredInteractivePanels = new List<InteractivePanel>();

			foreach ( InteractivePanel panel in interactivePanels ) {
				
				if ( !_registeredInteractivePanels.Contains( panel ) ) {
					_registeredInteractivePanels.Add( panel );
				}
			}

			_inputLayer = inputLayer;
			
			Init ();
		}

		protected string _inputLayer;
		protected List<InteractivePanel> _registeredInteractivePanels;
		
		private void Init () {
			
			foreach ( Panel p in _registeredInteractivePanels ) {
				p.Init ();
			}
		}
		public override void Destroy () {

			base.Destroy ();

			foreach ( Panel p in _registeredInteractivePanels ) {
				GameObject.Destroy( p.gameObject );
			}	
		}
		public override void Present () {

			base.Present ();

			foreach ( Panel p in _registeredInteractivePanels ) {
				p.Present ();
			}
			EdensGarden.Instance.Input.RegisterToInputLayer( _inputLayer, this );
			EdensGarden.Instance.Input.RequestInput( _inputLayer );
		}
		public override void Dismiss () {

			base.Dismiss ();
			
			foreach ( Panel p in _registeredInteractivePanels ) {
				p.Dismiss ();
			}

			EdensGarden.Instance.Input.RelinquishInput( _inputLayer );
			EdensGarden.Instance.Input.DeregisterFromInputLayer( _inputLayer, this ); // this should probably be tied to destroy
		}
		public override void EnterFocus () {

			base.EnterFocus ();
			
			foreach ( Panel p in _registeredInteractivePanels ) {
				p.EnterFocus ();
			}
		}
		public override void ExitFocus () {

			base.ExitFocus ();

			foreach ( Panel p in _registeredInteractivePanels ) {
				p.ExitFocus ();
			}
		}
		public override Panel GetContext( string panelIdentifier ) {
			
			foreach ( InteractivePanel p in _registeredInteractivePanels ) {
				if ( p.name == panelIdentifier ) {
					return p;
				}
			}

			return base.GetContext( panelIdentifier );
		}
	}
}