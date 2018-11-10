using UnityEngine;
using UnityEngine.UI;

namespace Eden.UI.Elements.Building {

	public class StatCell : MonoBehaviour {

		[SerializeField] Text _text;

		public void SetStat ( Stat stat ) {

			_text.text = stat.Level.ToString();
		}
	}
}