using UnityEngine;
using Eden.Life;

namespace Eden.Interactable {
	
	public class Stats : MonoBehaviour {

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

		public Stats () {

			_currentHealth = _maxHealth;
		}

		// **************** PRIVATE *****************
		
		[SerializeField] private int _maxHealth;
		[SerializeField] private int _currentHealth;
		[SerializeField] private Eden.Interactable.Hitable _hitable;

		// *****************************************

		private void OnSetHealth ( int health ) {

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
	}
}