using Alcamala.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Alcamala.Pages;

public partial class Login
{
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public required FirebaseService FirebaseService { get; set; }
    [Inject] public required FirebaseAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private string _username = string.Empty;
    private string _password = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        if (authenticationState.User.Identity?.IsAuthenticated is true)
        {
            NavigationManager.NavigateTo("/");
        }

        AuthenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
    }

    private async void OnClickLogIn()
    {
        await FirebaseService.TryLogInAsync(_username, _password);
    }

    private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        NavigationManager.NavigateTo("counter");
    }

    private async void OnClickLogOut()
    {
        await FirebaseService.LogOutAsync();
    }
}