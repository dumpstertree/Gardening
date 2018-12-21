﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {
	
	public class Spawn : Dumpster.Characteristics.NotificationResponder {

		
		// *************** Protected ********************

		protected override void Respond () {

			CreateInstance ();
		}


		// *************** Private ********************

		[Header( "Spawn" )]
		[SerializeField] private Transform _spawner;
		[SerializeField] private GameObject _prefab;
		[SerializeField] private Transform _parent;

		private void CreateInstance () {

			var inst = Instantiate( _prefab );

			inst.transform.SetParent( _parent, false );

			inst.transform.position = _spawner.position;
			inst.transform.rotation = _spawner.rotation;
		}
	}
}