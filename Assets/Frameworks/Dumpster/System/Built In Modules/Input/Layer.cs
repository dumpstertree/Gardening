using System.Collections.Generic;

namespace Dumpster.Core.BuiltInModules.Input {

	public class Layer<T> {

		private List< IInputReciever<T>> _recievers = new List< IInputReciever<T>>();

		public void AddToLayer(  IInputReciever<T> reciever ) {

			_recievers.Add( reciever );
		}
		public void RemoveFromLayer( IInputReciever<T> reciever ){

			if ( _recievers.Contains( reciever ) ) {
				_recievers.Remove( reciever );
			}
		}
		public void RecieveInput ( T package ){
			
			for ( int i=_recievers.Count-1; i>=0; i-- ){
				var r = _recievers[ i ];
				r.RecieveInput( package );
			}
		}
		public void EnterFocus () {

			for ( int i=_recievers.Count-1; i>=0; i-- ){
				var r = _recievers[ i ];
				r.EnteredInputFocus ();
			}
		}
		public void ExitFocus () {

			for ( int i=_recievers.Count-1; i>=0; i-- ){
				var r = _recievers[ i ];
				r.ExitInputFocus ();
			}
		}
	}
}