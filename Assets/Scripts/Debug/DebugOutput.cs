using System;
using System.Text;
using Assets.Scripts.Configuration;
using Assets.Scripts.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Debug
{
	public class DebugOutput : MonoBehaviour
	{
		public static DebugOutput Instance { get; private set; }
		public Text ComponentToWriteTo;

		public DebugOutput()
		{
			Instance = this;
			_messages = new StringBuilder();
		}

		private readonly StringBuilder _messages;

		// Use this for initialization
		void Start()
		{
		}


		void LateUpdate()
		{
			Write();
		}

		public void AddMessage(string msg)
		{
			_messages.AppendLine(msg);
		}

		private void Write()
		{
			if (_messages != null && _messages.Length > 0)
			{
				ComponentToWriteTo.text = _messages.ToString();
				_messages.Remove(0, _messages.Length);
			}
			else
			{
				ComponentToWriteTo.text = String.Empty;
			}
		}
	}
}
