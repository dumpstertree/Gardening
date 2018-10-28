using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;

namespace Dumpster.BuiltInModules {

	public interface IContextDelgate {
		Context GetContext ( string forContextIdentifier );
		void ReturnContext ( Context context );
	}

	[CreateAssetMenu(menuName = "Dumpster/Modules/UI")]
	public class UI : Module {

		
		// ***************** Public *********************

		public IContextDelgate ContextDelgate {
			set{ }
			get{ return _game.GetComponent<IContextDelgate>(); }
		}

		public void Present( int layer, string contextIdentifier, System.Action<Context> onComplete = null ) {

			if ( _layers[ layer ] == null ) {
				_layers[ layer ] = new Layer( ContextDelgate );
			}

			_layers[ layer ].Present( contextIdentifier, onComplete );
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