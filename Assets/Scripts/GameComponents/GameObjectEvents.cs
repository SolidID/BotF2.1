using System;
using UnityEngine;

namespace Assets.Scripts.GameComponents
{
	public class GameObjectEvents : MonoBehaviour
	{
		public event EventHandler MouseDown = (sender, args) => { };
		public event EventHandler MouseDrag = (sender, args) => { };
		public event EventHandler MouseEnter = (sender, args) => { };
		public event EventHandler MouseExit = (sender, args) => { };
		public event EventHandler MouseOver = (sender, args) => { };
		public event EventHandler MouseUp = (sender, args) => { };
		public event EventHandler MouseClick = (sender, args) => { };

		void OnMouseDown() { MouseDown.Invoke(this, EventArgs.Empty); }
		void OnMouseDrag() { MouseDrag.Invoke(this, EventArgs.Empty); }
		void OnMouseEnter() { MouseEnter.Invoke(this, EventArgs.Empty); }
		void OnMouseExit() { MouseExit.Invoke(this, EventArgs.Empty); }
		void OnMouseOver() { MouseOver.Invoke(this, EventArgs.Empty); }
		void OnMouseUp() { MouseUp.Invoke(this, EventArgs.Empty); }
		void OnMouseUpAsButton() { MouseClick.Invoke(this, EventArgs.Empty); }

	}
}
