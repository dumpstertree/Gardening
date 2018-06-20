using UnityEngine;
using Eden.Life;
using Dumpster.Events;

namespace Eden.Interactable {
	
	public class Stats : MonoBehaviour {

		public delegate void HealthChangedEvent( int currentHealth );
		public HealthChangedEvent OnHealthChanged;


		// **************** PUBLIC *****************
		
		public int CurrentHealth {
			get{ return _currentHealth; }
		}
		public int MaxHealth {
			get{ return _maxHealth; }
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

		public Stats () {

			_currentHealth = _maxHealth;
		}

		// **************** PRIVATE *****************
		
		[SerializeField] private int _maxHealth;
		[SerializeField] private int _currentHealth;
		[SerializeField] private Eden.Interactable.Hitable _hitable;

		[Header( "Smart Events" )]
		[SerializeField] SmartEvent[] _onHurt;
		[SerializeField] SmartEvent[] _onDeath;

		// *****************************************

		private void OnSetHealth ( int health ) {

			if ( _currentHealth > 0 && health <= 0 ) {
				FireDeathEvent();
			} else {
				FireHurtEvent();
			}

			_currentHealth = health;
			HandleOnHealthChanged();
		}

		private void HandleOnHealthChanged () {

			if ( OnHealthChanged != null ) {

				OnHealthChanged( _currentHealth );
			}
		}
		private void Awake () {

			_hitable.OnHit += HandleOnHit;
		}
		private void HandleOnHit( BlackBox user, HitData data ) {

			RemoveHealth( 1 );
		}
		private void FireHurtEvent() {

			foreach ( SmartEvent e in _onHurt ) {
				e.EventTriggered();
			}
		}
		private void FireDeathEvent() {

			foreach ( SmartEvent e in _onDeath ) {
				e.EventTriggered();
			}
		}


	}
}