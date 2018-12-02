using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Modules {

	[CreateAssetMenu(menuName = "Eden/Module/Zen")]	
	public class Zen : Dumpster.Core.Module {

		
		// ************* Public ******************

		public int Level {
			get{ return _currentLevel; }
		}
		public int CurrentZen {
			get { return _currentZen; }
		}
		public int MaxZen {
			get { return _maxLevels * _zenPerLevel; }
		}
		
		public delegate void LevelUpEvent ( int level );
		public LevelUpEvent OnLevelUp;

		public delegate void BreakEvent ();
		public BreakEvent OnBreak;

		public void AddZen ( int zen ) {

			_currentZen += zen;
			_lastTimeStamp = Time.time;

			_currentZen = Mathf.Clamp( _currentZen, 0, _maxLevels * _zenPerLevel );

			var level =  Mathf.FloorToInt( _currentZen / _zenPerLevel );
			if ( level > _currentLevel ) {
				
				_currentLevel = level;
				FireLevelUpEvent( level );
			}
		}
		public void BreakZen () {

			_currentZen = 0;
			
			FireBreakEvent();
		}
		
		
		// ************* Protected ******************
		
		protected override void OnLateUpdate() {
			
			if ( _currentZen > 0 ) {

				if ( Time.time - _lastTimeStamp > _timeBeforeDeteriate  ) {
					
					_currentZen -= _deteration;
					_lastTimeStamp = Time.time;

					_currentZen = Mathf.Clamp( _currentZen, 0, _maxLevels * _zenPerLevel );

					if ( _currentZen == 0 ) {
						FireBreakEvent();
					}
				}
			}
		}
		protected override void OnReload() {
			
			OnLevelUp = null;
			OnBreak = null;
		}

	
		// ************* Private ******************
		

		[SerializeField] private int _maxLevels = 3;
		[SerializeField] private int _zenPerLevel = 33;
		[SerializeField] private int _deteration = 1;
		[SerializeField] private float _timeBeforeDeteriate = 1f;

		private int _currentZen;
		private int _currentLevel;
		private float _lastTimeStamp;

		private void FireLevelUpEvent ( int newLevel ) {

			if ( OnLevelUp != null ) {
				OnLevelUp( newLevel );
			}
		}
		private void FireBreakEvent () {
			
			if ( OnBreak != null ) {
				OnBreak ();
			}
		}
	}
}