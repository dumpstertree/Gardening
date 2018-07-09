﻿using UnityEngine;
using UnityEngine.UI;

namespace Eden.UI.Subpanels.Building {
	
	public class PartOverlay : BuildSubpanel {

		[SerializeField] private Eden.UI.Elements.Building.StatsList _statBlock;
		[SerializeField] private Text _nameText;

		public void SetPart ( Eden.Model.Building.Parts.Gun part ) {

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
}