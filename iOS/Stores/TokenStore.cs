using Xamarin.Forms;
using Pleioapp.iOS;
using Xamarin.Auth;
using System.Linq;
using System.Collections.Generic;

[assembly: Dependency(typeof(CredentialStore))]
namespace Pleioapp.iOS
{
	public class CredentialStore : ITokenStore
	{
		static string context = "Pleio";
		AccountStore store;

		public CredentialStore() {
			store = AccountStore.Create();
			var records = store.FindAccountsForService (context);
		}

		public void saveToken(AuthToken token) {
			clearTokens ();

			var newAccount = new Account ();
			newAccount.Username = token.accessToken;
			if (token.expiresIn != null) { newAccount.Properties.Add ("exires_in", token.expiresIn); }
			if (token.tokenType != null) { newAccount.Properties.Add ("token_type", token.tokenType); }
			if (token.scope != null) { newAccount.Properties.Add ("scope", token.scope); }
			if (token.refreshToken != null) { newAccount.Properties.Add ("refresh_token", token.refreshToken); }
			store.Save (newAccount, context);
		}

		public AuthToken getToken() {
			var account = store.FindAccountsForService (context).FirstOrDefault ();

			if (account == null) {
				return null;
			} else {
				var token = new AuthToken();
				token.accessToken = account.Username;

				if (account.Properties.ContainsKey("expires_in")) { token.expiresIn = account.Properties ["expires_in"]; }
				if (account.Properties.ContainsKey("token_type")) { token.tokenType = account.Properties ["token_type"]; }
				if (account.Properties.ContainsKey("scope")) { token.scope = account.Properties ["scope"]; }
				if (account.Properties.ContainsKey("refresh_token")) { token.refreshToken = account.Properties ["refresh_token"]; }
				return token;
			}
		}

		public void clearTokens() {
			var accounts = AccountStore.Create().FindAccountsForService(context).ToList();
			accounts.ForEach(account => AccountStore.Create().Delete(account, context));
		}
	}
}

