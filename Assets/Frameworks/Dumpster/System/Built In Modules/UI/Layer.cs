using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.BuiltInModules {

	public class Layer {

		// **************** Constructor ***********************

		public Layer ( IContextDelgate contextDelegate ) {
			
			_contextDelegate = contextDelegate;
		}


		// **************** Public ***********************

		public string Identifier{
			get; private set;
		}

	
		public void Present ( string contextIdentifier, System.Action<Context> onComplete = null ) {
			
			if ( _layerHistory.Count > 0 ) {
				_layerHistory[ _layerHistory.Count - 1 ].Dismiss();
			}

			var context = _contextDelegate.GetContext( contextIdentifier );

			if ( onComplete != null ) {
				onComplete( context );
			}

			_layerHistory.Add( context );
			
			context.Present();
		}
		public void Dismiss ( string contextIdentifier ) {

			for ( int i = _layerHistory.Count-1; i>=0; i-- ) {
				
				var context = _layerHistory[ i ];
				if (context.Identifier == contextIdentifier) {
					
					_layerHistory.RemoveAt( i );
					context.Dismiss();

					_contextDelegate.ReturnContext( context );
				}
			}

			if( _layerHistory.Count > 0 ) {
				_layerHistory[ _layerHistory.Count - 1 ].Present();
			}
		}
		public void ChangeFocusLevel( int newLevel )  {
		}



		
		// **************** Private ***********************

		private List<Context> _layerHistory = new List<Context>();
		private IContextDelgate _contextDelegate;
	}
}