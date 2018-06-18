using UnityEngine;

namespace Dumpster.Core.Life {

	public abstract class BlackBox<T> : MonoBehaviour {

		public delegate void InitEvent();
		public InitEvent OnInit;

		public delegate void RunEvent();
		public RunEvent OnRun;

		public T Visual {
			get{ return GetVisual(); }
		}
		public Animator Animator {
			get { return _animator; }
		}

		[SerializeField] private Animator _animator;
		[SerializeField] private LogicChip _logicChip;

		private bool _isPowered = true;
		
		// **************** Private ********************

		private void Awake () {

			Init ();
			Run ();
		}
		private void Update () {

			if( _isPowered && CanThink() ){
				Think();
			}
		}
		private void FireOnInit () {

			if ( OnInit != null ) {
				OnInit ();
			}
		}

		private void FireOnRun () {
			
			if ( OnRun != null ) {
				OnRun ();
			}
		}


		// **************** Virtual ********************
		
		protected virtual void Init (){

			FireOnInit ();
		}
		protected virtual void Run () {

			FireOnRun ();
		}
		protected virtual void Think () {

			_logicChip.Analayze();
		}
		protected virtual void Shutdown () {

			_isPowered = false;
		}
		protected virtual void Reboot () {

			_isPowered = true;
		}		
		protected virtual bool CanThink () {
			return ( _animator != null && _animator.tag != "RestrictInput" || _animator == null );
		}

		// **************** Abstract ********************

		protected abstract T GetVisual ();
	}
}