using System.Collections.Generic;
using UnityEngine;

namespace Eden.UI.Elements.Building {
	
	public class StatsList : MonoBehaviour {

		public void SetBlock ( IStatBlockDelegate statBlockDelegate ) {

			ClearStats();

			foreach ( Stat s in statBlockDelegate.GetStats() ) {
				CreateStat( s );
			}
		}


		[SerializeField] private StatCell _statVisualPrefab;
		[SerializeField] private Transform _content;

		private List<StatCell> _statVisuals = new List<StatCell>();

		private void ClearStats () {

			for ( int i=_statVisuals.Count-1; i>=0; i-- ){
				var s = _statVisuals[ i ];
				Destroy( s.gameObject );
			}

			_statVisuals.Clear();
		}
		private void CreateStat ( Stat stat ) {

			var inst = Instantiate( _statVisualPrefab );
			inst.transform.SetParent( _content );
			inst.SetStat( stat );

			_statVisuals.Add( inst );
		}
	}


	public interface IStatBlockDelegate {

		List<Stat> GetStats ();

	}
	public struct Stat {

		public string Name { get; }
		public Sprite Sprite { get; }
		public int Level { get; }

		public Stat ( string name, Sprite sprite, int level ) {

			Name = name;
			Sprite =sprite;
			Level = level;
		}
	}
}