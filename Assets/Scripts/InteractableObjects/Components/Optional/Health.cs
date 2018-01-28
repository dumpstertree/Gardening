using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable.OptionalComponent {

	public class Health : MonoBehaviour {

		public delegate void HealthChangedEvent( int currentHealth );
		public HealthChangedEvent OnHealthChanged;

		// **************** PUBLIC *****************
		
		public int CurrentHealth {
			get{ return _currentHealth; }
		}
		public void AddHealth( int amount ) {
			OnSetHealth( _currentHealth + amount );
		}
		public void RemoveHealth( int amount ) {
			OnSetHealth( _currentHealth - amount );
		}
		public void SetHealth( int amount ) {
			OnSetHealth( amount );
		}

		// **************** PRIVATE *****************
		
		[SerializeField] private int _maxHealth;

		private int _currentHealth;
		private OptionalComponent.Destroyable _destroyable;

		// *****************************************

		private void Awake(){

			_destroyable = GetComponent<OptionalComponent.Destroyable>();
			_currentHealth = _maxHealth;
		}
		private void OnSetHealth ( int health ) {

			_currentHealth = health;
			HandleOnHealthChanged();

			if ( _currentHealth <= 0 ) {

				if ( _destroyable != null ) {
				
					_destroyable.Destroy();
				}
			}
		}

		private void HandleOnHealthChanged () {

			if ( OnHealthChanged != null ) {

				OnHealthChanged( _currentHealth );
			}
		}
	}
}