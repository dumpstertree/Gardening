using Eden.Life;

namespace Eden.Model.Interactable {

	public class Hit {

		public BlackBox Hitter { get; }
		public int Power { get; }

		public Hit ( BlackBox hitter, int power ) {

			Hitter = hitter;
			Power = power;
		}
	}
}