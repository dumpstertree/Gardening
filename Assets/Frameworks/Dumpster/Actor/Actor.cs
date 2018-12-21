using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core.BuiltInModules;

namespace Dumpster.Core {

	public class Actor : MonoBehaviour {

		[SerializeField] private bool _enabled = true;

		public bool Enabled {
			get { return _enabled; }
			set {  SetEnabled( value ); }
		}

		public T GetCharacteristic<T>( bool throwError = false ) where T : class {

			foreach ( Characteristic c in _characteristics ) {
				if ( c.GetType() == typeof( T ) && !c.Secret ) {
					return c as T;
				}
			}
			
			if ( throwError ) {
				Debug.LogError( "No 'Characteristic' of the type '" + typeof( T ) + "'", gameObject );
			}

			return null;
		}
		public List<T> GetCharacteristics<T>() where T : class {

			var charactersitics = new List<T>();
			foreach ( Characteristic c in _characteristics ) {
				if ( c.GetType() == typeof( T ) && !c.Secret ) {
					charactersitics.Add( c as T );
				}
			}

			return charactersitics;
		} 
		public void PostNotification ( string notification ) {
			
			foreach ( Characteristic c in _characteristics ) {
				c.RecieveNotification( notification );
			}
		}
		
		public void DestroyActor ( float inSeconds = 0f ) {

			if (!_isBeingDestroyed) {
				
				Game.GetModule<Async>()?.WaitForEndOfFrame( () => Destroy( gameObject, inSeconds ) );
				_isBeingDestroyed = true;
			}
		}
		public List<string> GetNotifications () {
			
			var notifications = new List<string>();
			
			notifications.AddRange( Characteristic.GetStaticNotifications() );
			
			foreach( Characteristic c in GetComponentsInChildren<Characteristic>() ) {
				 notifications.AddRange( c.GetNotifications() );
			}

			return notifications;
		}

		private void Awake () {

			_characteristics = GetComponentsInChildren<Characteristic>();

			foreach ( Characteristic c in _characteristics ) {
				c.Install( this );
			}
			
			foreach ( Characteristic c in _characteristics ) {
				c.Init ();
			}

			foreach ( Characteristic c in _characteristics ) {
				c.Run ();
			}
		}
		private void Update () {
			
			if ( Enabled ) {
				
				foreach ( Characteristic c in _characteristics ) {
					c.ActorUpdate ();
				}
			}
		}

		private void SetEnabled ( bool newEnabled ) {

			if( _enabled != newEnabled ) {

				_enabled = newEnabled;
				if ( _enabled ) {
					foreach ( Characteristic c in _characteristics ) {
						c.ActorEnabled();
					}
				}
				if ( !_enabled ) {
					foreach ( Characteristic c in _characteristics ) {
						c.ActorDisabled();
					}
				}
			}
		}
		private Characteristic[] _characteristics;
		private bool _isBeingDestroyed;
	}
}