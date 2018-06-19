using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShutdownChip : Dumpster.Core.Life.Chips.ShutdownChip {

	[SerializeField] private GameObject _particleEffect;

	protected override void OnShutdown() {	

		var inst = Instantiate( _particleEffect );
		inst.transform.position = transform.position;
	}
}
