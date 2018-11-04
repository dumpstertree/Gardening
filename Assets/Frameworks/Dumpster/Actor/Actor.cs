﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core.BuiltInModules;

namespace Dumpster.Core {

	public class Actor : MonoBehaviour {

		[SerializeField] private bool _enabled = true;

		public T GetCharacteristic<T>( bool throwError = false ) where T : class {

			foreach ( Characteristic c in _properties ) {
				if ( c.GetType() == typeof( T ) ) {
					return c as T;
				}
			}
			
			if ( throwError ) {
				Debug.LogError( "No 'Characteristic' of the type '" + typeof( T ) + "'", gameObject );
			}

			return null;
		} 
		public void PostNotification ( string notification ) {
			
			foreach ( Characteristic c in _properties ) {
				c.RecieveNotification( notification );
			}
		}
		
		public void DestroyActor () {

			if (!_isBeingDestroyed) {
				
				Game.GetModule<Async>()?.WaitForEndOfFrame( () => Destroy( gameObject ) );
				_isBeingDestroyed = true;
			}
		}

		private void Awake () {

			_properties = GetComponentsInChildren<Characteristic>();

			foreach ( Characteristic c in _properties ) {
				c.Install( this );
			}
			
			foreach ( Characteristic c in _properties ) {
				c.Init ();
			}

			foreach ( Characteristic c in _properties ) {
				c.Run ();
			}
		}
		private void Update () {
			
			if ( enabled ) {
				
				foreach ( Characteristic c in _properties ) {
					c.ActorUpdate ();
				}
			}
		}

		private Characteristic[] _properties;
		private bool _isBeingDestroyed;
	}
}