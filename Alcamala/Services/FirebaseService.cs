using Google.Cloud.Firestore;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Alcamala.Services;

public class FirebaseService
{
    private const string ProjectID = "alcamala-firebase";

    private FirestoreDb? _database;
    public FirestoreDb Database => _database ??= FirestoreDb.Create(ProjectID);

    private IJSRuntime JSRuntime { get; }
    private FirebaseAuthenticationStateProvider _firebaseAuthenticationStateProvider;

    private DotNetObjectReference<FirebaseService> _ref;

    public FirebaseService(IJSRuntime jSRuntime, FirebaseAuthenticationStateProvider firebaseAuthenticationStateProvider)
    {
        _ref = DotNetObjectReference.Create(this);

        JSRuntime = jSRuntime;
        _firebaseAuthenticationStateProvider = firebaseAuthenticationStateProvider;

        JSRuntime.InvokeVoidAsync("Firebase.initializeFirebaseService", _ref);
    }

    [JSInvokable]
    public void OnAuthStateChanged(string? userInfo)
    {
        if (!string.IsNullOrEmpty(userInfo))
        {
            try
            {
                var firebaseUser = JsonConvert.DeserializeObject<FirebaseUser>(userInfo);

                if (firebaseUser is not null)
                {
                    _firebaseAuthenticationStateProvider.OnUserLogin(firebaseUser);
                }
            }
            catch
            {
                // Json deserialization error
            }
        }
        else
        {
            _firebaseAuthenticationStateProvider.OnUserLogout();
        }
    }

    public async Task TryLogInAsync(string email, string password)
    {
        var loginResult = await JSRuntime.InvokeAsync<string>("Firebase.trySignInWithEmailAndPassword", email, password);
    }

    public async Task LogOutAsync()
    {
        await JSRuntime.InvokeVoidAsync("Firebase.signOut");
    }

    //[JSInvokable]
    //public async Task AfterSignIn(bool success, string? error)
    //{
    //    // basically you may want to cache the result of login
    //    //afterSignInInfo = success ? new AfterSignInInfo(info) : null;
    //    //// return serialized sign-in data as string for storing in the cookie
    //    //return success ? JsonSerializer.Serialize(info!) : null;
    //}
}
