using Dumpster.Core.BuiltInModules.Input;
using Dumpster.Core.BuiltInModules.UI;
using System.Collections.Generic;

namespace Eden.UI {

	public class InteractiveContext : Dumpster.Core.BuiltInModules.UI.Context, IInputReciever<Eden.Input.Package> {


		void IInputReciever<Eden.Input.Package>.RecieveInput ( Eden.Input.Package package ) {
			
			OnRecieverInput( package );
		}
		void IInputReciever<Eden.Input.Package>.EnteredInputFocus () {
			
			OnEnterInputFocus ();
		}
		void IInputReciever<Eden.Input.Package>.ExitInputFocus () {
			
			OnExitInputFocus ();
		}

		public InteractiveContext ( string contextIdentifier, string inputLayer, List<Panel> panels ) : base( contextIdentifier, panels ) {
				
			_inputLayer = inputLayer;
			EdensGarden.Instance.Input.RegisterToInputLayer( inputLayer, this );
		}

		protected string _inputLayer;
		protected void OnRecieverInput( Eden.Input.Package package) {}
		protected void OnEnterInputFocus () {}
		protected void OnExitInputFocus () {}
		
		public override void Present () {

			base.Present ();
	
			EdensGarden.Instance.Input.RequestInput( _inputLayer );
		}
		public override void Dismiss () {
			
			base.Dismiss ();

			EdensGarden.Instance.Input.RelinquishInput( _inputLayer );
		}
	}
}