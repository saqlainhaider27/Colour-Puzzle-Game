using UnityEngine;

public class FirebaseSettings : MonoBehaviour {
    private Firebase.FirebaseApp app;
    private void Start() {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                app = Firebase.FirebaseApp.DefaultInstance;
                // Debug.Log("Firebase dependencies are available.");
                // Initialize Firebase services here if needed
            } else {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }
}