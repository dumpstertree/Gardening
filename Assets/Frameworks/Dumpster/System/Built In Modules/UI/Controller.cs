using System.Collections.Generic;

namespace Dumpster.Core.BuiltInModules.UI {

	public interface IContextDelgate {
		Context GetContext ( string forContextIdentifier );
		void ReturnContext ( Context context );
	}

	public class Controller : Module {

		
		// ***************** Public *********************

		public IContextDelgate ContextDelgate {
			private get; set;
		}

		public void Present( int layer, string contextIdentifier ) {

			if ( _layers[ layer ] == null ) {
				_layers[ layer ] = new Layer( ContextDelgate );
			}

			_layers[ layer ].Present( contextIdentifier );
		}
		public void Dismiss( int layer, string contextIdentifier ) {

			if ( _layers[ layer ] != null ) {

				_layers[ layer ].Dismiss( contextIdentifier );
			}
		}



		// ***************** Private *********************

		private Layer _currentLayer {
			get { return ( _layerStack.Count > 0 ) ? _layerStack[ _layerStack.Count-1 ] : null;  }
		}

		private const int MAX_NUM_OF_LAYERS = 10;

		private List<Layer> _layerStack;
		private Layer[] _layers = new Layer[ MAX_NUM_OF_LAYERS ];


		// ***************** Override *********************

		protected override void OnInit () {
			
			_layerStack = new List<Layer>();
		}
	}
}