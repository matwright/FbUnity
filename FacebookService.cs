using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class FacebookService : MonoBehaviour
{
  
    
    
    public Button FbLoginButton;

    void Start () {
        Debug.Log("STart");
       
    }

    public void FbLogin()
    {
        var perms = new List<string>(){"public_profile", "email"};
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }


// Awake function from Unity's MonoBehavior
    void Awake ()
    {
        Debug.Log("Awakened");
        if (!FB.IsInitialized) {
            // Initialize the Facebook SDK
            Debug.Log("Initialize the Facebook SDK");
            FB.Init(InitCallback, OnHideUnity);
        } else {
            // Already initialized, signal an app activation App Event
            Debug.Log("Already initialized, signal an app activation App Event");
            FB.ActivateApp();
        }
        
        
    }

    private void InitCallback ()
    {
        if (FB.IsInitialized) {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        } else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity (bool isGameShown)
    {
        if (!isGameShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        } else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    } 
   

    private void AuthCallback (ILoginResult result) {
        if (FB.IsLoggedIn) {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
     
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions) {
                Debug.Log(perm);
            }
            FetchFbProfile ();
        } else {
            Debug.Log("User cancelled login");
        }
    }
    
    private void FetchFbProfile () {
        FB.API("/me?fields=first_name,last_name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string,string>(){});
    }
 
    private void FetchProfileCallback (IGraphResult result) {
 
        Debug.Log (result.RawResult);
 
       var  fbUserDetails = (Dictionary<string,object>)result.ResultDictionary;

     
        Debug.Log ("Profile: first name: " + fbUserDetails["first_name"]);
        Debug.Log ("Profile: last name: " + fbUserDetails["last_name"]);
        Debug.Log ("Profile: id: " + fbUserDetails["id"]);
        Debug.Log ("Profile: email: " + fbUserDetails["email"]);
        
       
 
    }
    
    
}
