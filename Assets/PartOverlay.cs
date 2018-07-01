using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartOverlay : BuildSubpanel {

	[SerializeField] private StatBlock _statBlock;
	[SerializeField] private Text _nameText;

	public void SetPart ( Part part ) {

		_nameText.text = part.Name;
		_statBlock.SetBlock( part.BuilderStats );
	}

	protected override void Enable () {
	
		gameObject.SetActive( true );
		
	}
	protected override void Disable () {
	
		gameObject.SetActive( false );
	}
}
