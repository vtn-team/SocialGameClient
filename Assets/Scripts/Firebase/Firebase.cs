using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Outgame;
using System;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Firebaseログ記録する人人
/// NOTE: https://github.com/firebase/quickstart-unity
/// </summary>
public class FirebaseAnalyticsLogger
{
    static FirebaseAnalyticsLogger _instance = new FirebaseAnalyticsLogger();
    private FirebaseAnalyticsLogger() { }

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool firebaseInitialized = false;

    string _udid = "";


    static public void Setup(string udid)
    {
        _instance.SetupInner();
        _instance._udid = udid;
    }

    void SetupInner()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                    "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        Debug.Log("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        Debug.Log("Set user properties.");
        // Set the user's sign up method.
        FirebaseAnalytics.SetUserProperty(
            FirebaseAnalytics.UserPropertySignUpMethod,
            "Google");

        FirebaseAnalytics.SetUserId(_udid);
        FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        firebaseInitialized = true;
    }


    /// <summary>
    /// 以下ログを追加していく
    /// </summary>
    
    static public void AnalyticsLogin()
    {
        // Log an event with no parameters.
        Debug.Log("Logging a login event.");
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
    }

    static public void AnalyticsSceneView(ViewID viewID)
    {
        // Log an event with no parameters.
        Debug.Log($"Logging a scene view: {viewID.ToString()}");
        FirebaseAnalytics.LogEvent("Scene", "View", viewID.ToString());
    }

    static public void AnalyticsDoAction(string action, string log)
    {
        // Log an event with a float.
        Debug.Log($"Logging a action event : {action}");
        FirebaseAnalytics.LogEvent("Action", action, log);
    }

    /*
    // Reset analytics data for this app instance.
    public void ResetAnalyticsData()
    {
        Debug.Log("Reset analytics data.");
        FirebaseAnalytics.ResetAnalyticsData();
    }
    */

    // Get the current app instance ID.
    Task<string> DisplayAnalyticsInstanceId()
    {
        return FirebaseAnalytics.GetAnalyticsInstanceIdAsync().ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.Log("App instance ID fetch was canceled.");
            }
            else if (task.IsFaulted)
            {
                Debug.Log(String.Format("Encounted an error fetching app instance ID {0}",
                                        task.Exception.ToString()));
            }
            else if (task.IsCompleted)
            {
                Debug.Log(String.Format("App instance ID: {0}", task.Result));
            }
            return task;
        }).Unwrap();
    }
}