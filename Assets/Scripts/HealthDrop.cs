using UnityEngine;
using Eden.Events;
using Dumpster.Events;
using Eden.Interactable;
using Eden.Properties;

public class HealthDrop : MonoBehaviour {

	private const float MIN_ORB_SIZE = 0.1f;
	private const float MAX_ORB_SIZE = 0.4f;
	private const float MIN_RANDOMNESS = 0.5f;
	private const float MAX_RANDOMNESS = 1.5f;
	private const int MAX_HEALTH_FOR_SIZING = 30;

	[SerializeField] private AddHealth _addHealth;
	[SerializeField] private Magnetic _magnetic;
	[SerializeField] SmartEvent[] _onMagnet;

	public int Health { get; private set; }

	public void SetHealth( int health ) {

		Health = health;

		var size = MIN_ORB_SIZE + ( Mathf.Clamp01( health/MAX_HEALTH_FOR_SIZING ) * (MAX_ORB_SIZE - MIN_ORB_SIZE) );
		var randomness = Random.Range( MIN_RANDOMNESS, MAX_RANDOMNESS );
		var scale = size * randomness;
		
		transform.localScale = new Vector3( scale, scale, scale );
	}
	private void Awake () {

		_magnetic.OnReachedTarget += HandleReachedTarget;
	}
	private void HandleReachedTarget( Eden.Properties.Magnet magnet ) {
	
		_addHealth.Health = Health;
		_addHealth.Stats = magnet.Root.GetComponentInChildren<Stats>();
		
		FireOnMagnet();
	}
	private void FireOnMagnet() {

		foreach ( SmartEvent e in _onMagnet ) {
			e.EventTriggered();
		}
	}
}
