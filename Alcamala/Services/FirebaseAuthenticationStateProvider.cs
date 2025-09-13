using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace Alcamala.Services;

public class FirebaseAuthenticationStateProvider : RemoteAuthenticationService<RemoteAuthenticationState, RemoteUserAccount, OidcProviderOptions>
{
    private NavigationManager _navigationManager;
    private ILocalStorageService _localStorageService;

    public FirebaseUser? CurrentUser { get; private set; }

    private ClaimsIdentity _identity = new();

    public FirebaseAuthenticationStateProvider(IJSRuntime jsRuntime, IOptionsSnapshot<RemoteAuthenticationOptions<OidcProviderOptions>> options, NavigationManager navigation, AccountClaimsPrincipalFactory<RemoteUserAccount> accountClaimsPrincipalFactory, ILogger<RemoteAuthenticationService<RemoteAuthenticationState, RemoteUserAccount, OidcProviderOptions>>? logger, ILocalStorageService localStorageService) : base(jsRuntime, options, navigation, accountClaimsPrincipalFactory, logger)
    {
        _navigationManager = navigation;
        _localStorageService = localStorageService;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(_identity)));
    }

    public async void OnUserLogin(FirebaseUser user)
    {
        CurrentUser = user;

        List<Claim> claims = [
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(nameof(FirebaseUser.Uid), user.Uid),
            new Claim(ClaimTypes.Name, user.FirstName ?? user.Email),
            new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty)
        ];

        _identity = new ClaimsIdentity(claims, "authentication");

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        await _localStorageService.SetItemAsync("email", user.Email);

        _navigationManager.NavigateTo("");
    }

    public void OnUserLogout()
    {
        CurrentUser = null;

        _identity = new();

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

public class FirebaseUser
{
    public required string Uid { get; init; }
    public required string Email { get; init; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Age { get; set; }
}