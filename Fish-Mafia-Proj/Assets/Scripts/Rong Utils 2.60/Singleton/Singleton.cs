using UnityEngine;

namespace Xrofng
{
	/// <summary>
	/// Singleton pattern.
	/// </summary>
	public class Singleton<T> : BetterMonoBehaviour	where T : Component
	{
		protected static T _instance;
		public static bool HasInstance => _instance != null;

		/// <summary>
		/// Singleton design pattern
		/// </summary>
		/// <value>The instance.</value>
		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindAnyObjectByType<T> ();
					if (_instance == null)
					{
						GameObject obj = new GameObject ();
						_instance = obj.AddComponent<T> ();
					}
				}
				return _instance;
			}
		}

	    /// <summary>
	    /// On awake, we initialize our instance. Make sure to call base.Awake() in override if you need awake.
	    /// </summary>
	    protected override void Awake ()
		{
			if (!Application.isPlaying)
			{
				return;
			}

			_instance = this as T;			
		}

		protected override void Start()
		{
		}

		protected override void Update() { }
	}
}
