using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {

	public class Health : Dumpster.Core.Characteristic {

		
		// ************* Public *******************

		public const string DEATH = "Health.Death";
		public const string DECREASED = "Health.Decreased";
		public const string INCREASED = "Health.Increased";

		public int Current  {
			get{ return _health; }
		}
		public int Max {
			get{ return _maxHealth; }
		}
		public bool HasInfiniteHealth {
			get{ return _hasInfiniteHealth; }
		}

		public override List<string> GetNotifications () {

			return new List<string>() {
				DEATH,
				DECREASED,
				INCREASED,
			};
		}
		public void AddHealth ( int health ) {
			
			_health = Mathf.Clamp( _health + health, 0, _maxHealth );
			_actor.PostNotification( INCREASED );
		}
		public void SubtractHealth ( int health ) {

			if ( _health <= 0 || _hasInfiniteHealth ) {
				return;
			}

			_health = Mathf.Clamp( _health - health, 0, _maxHealth );
			_actor.PostNotification( DECREASED );

			if ( _health <= 0 ) {
				_actor.PostNotification( DEATH );
			}
		}


		// ************* Protected *******************
		
		protected override void OnInit() {
			
			_health = _maxHealth;
		}

		
		// ************* Private *******************

		[SerializeField] private int _health = 10;
		[SerializeField] private int _maxHealth = 10;
		[SerializeField] private bool _hasInfiniteHealth = false;

	}
}