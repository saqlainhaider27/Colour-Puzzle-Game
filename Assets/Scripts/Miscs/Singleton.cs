using UnityEngine;

public class Singleton<T> : MonoBehaviour
	where T : Component
{
	private static T _instance;
    public static T Instance {
        get {
            if(_instance == null) {
                _instance = FindObjectOfType<T>();
				if(_instance == null) {
					Debug.LogError(typeof(T).Name + " not found in scene!");
				}
            }
            return _instance;
        }
    }
}


public class SingletonPersistent<T> : MonoBehaviour
	where T : Component
{
	public static T Instance { get; private set; }
	
	public virtual void Awake ()
	{
		if (Instance == null) {
			Instance = this as T;
			DontDestroyOnLoad (this);
		} else {
			Destroy (gameObject);
		}
	}
}
