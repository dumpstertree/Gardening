using UnityEngine;
using System.Collections.Generic;

namespace Dumpster.Core.BuiltInModules.Input {

	public abstract class Controller<T> : Module {
		
		// *************** Public ******************
		
		public void RegisterToInputLayer( string identifier, IInputReciever<T> reciever ) {

			Layer<T> layer;

			if ( _registeredLayers.ContainsKey( identifier ) ) {
				layer = _registeredLayers[ identifier ];
			} else {
				layer = new Layer<T>();
				_registeredLayers.Add( identifier, layer );
			}

			layer.AddToLayer( reciever );
		}
		public void DeregisterFromInputLayer( string identifier, IInputReciever<T> reciever ) {

			if ( _registeredLayers.ContainsKey( identifier ) ) {
				var layer = _registeredLayers[ identifier ];
				layer.RemoveFromLayer( reciever );
			}
		}
		public void RequestInput( string identifier ) {
			
			Layer<T> layer;
			if ( _registeredLayers.ContainsKey( identifier ) ) {
				layer = _registeredLayers[ identifier ];
			} else {
				layer = new Layer<T>();
				_registeredLayers.Add( identifier, layer );
			}

			if ( !_layers.Contains( layer ) ) {

				// dimiss old input layer
				PushInputPackage( GetEmptyPackage() );
				ExitFocus ();

				// push input into new layer
				_layers.Add( layer );
				EnterFocus ();
			}
		}
		public void RelinquishInput( string identifier ) {

			if ( _registeredLayers.ContainsKey( identifier ) ) {
				
				var layer  = _registeredLayers[ identifier ];
				if ( _layers.Contains( layer ) ) {
					
					// dimiss old input layer
					PushInputPackage( GetEmptyPackage() );
					ExitFocus ();
					_layers.Remove( layer );

					// push input into new input layer
					PushInputPackage( PollPackage() );
					EnterFocus();
				}
			}
		}


		// *************** Protected ******************
		
		protected override void OnInit () {

			 _layers = new List<Layer<T>>();
			 _registeredLayers = new Dictionary<string,Layer<T>>();
		}
		protected void PushInputPackage ( T package ) {

			if ( _currentLayer != null ) {
				_currentLayer.RecieveInput( package );
			}
		}
		private void EnterFocus () {

			if ( _currentLayer != null ) {
				_currentLayer.EnterFocus ();
			}
		}
		private void ExitFocus () {

			if ( _currentLayer != null ) {
				_currentLayer.ExitFocus ();
			}
		}

		
		// *************** Abstract ******************

		protected abstract T PollPackage (); 
		protected abstract T GetEmptyPackage ();

		
		// *************** Private ******************

		private List<Layer<T> > _layers;
		private Dictionary<string, Layer<T> > _registeredLayers;

		private Layer<T>  _currentLayer {
			get	{ return  ( _layers.Count > 0 ) ? _layers[ _layers.Count -1 ] : null; }
		}
	}
}










