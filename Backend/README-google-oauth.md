# Google OAuth Setup (Backend)

This document explains how to create Google OAuth credentials and configure the backend to verify Google ID tokens.

## 1. Create credentials in Google Cloud Console

1. Go to the Google Cloud Console: https://console.cloud.google.com/
2. Select your project or create a new one.
3. Navigate to APIs & Services → Credentials.
4. Click "Create Credentials" → "OAuth client ID".
5. Choose the **Application type**: **Web application** (OAuth 2.0 Client IDs).
6. Fill in a name (e.g., `ProjectName - Web Client`).
7. Configure **Authorized JavaScript origins** and **Authorized redirect URIs**:
   - Authorized JavaScript origins: add the client origin used in development, e.g. `http://localhost:5173` (Vite dev server).
   - Authorized redirect URIs: if your app performs a server-side OAuth exchange, add the backend callback URL, e.g. `https://localhost:5001/api/auth/google/callback` or `http://localhost:5000/api/auth/google/callback` depending on your environment.

Notes:

- If your frontend obtains an ID token directly (Google Identity Services) and sends it to the backend for verification, you should still set the Authorized JavaScript origins to include your frontend origin.
- Keep the generated **Client ID** and **Client secret** safe. The Client Secret is for server-side use only.

## 2. Configure Backend with dotnet user-secrets

We recommend storing sensitive values (Client ID / Client Secret) with `dotnet user-secrets` during development.

From the Backend project directory (where the .csproj is located):

```powershell
cd Backend
# Initialize user-secrets (only needed once per project)
dotnet user-secrets init

# Set the Google Client ID and Client Secret
# Use keys compatible with your configuration. Common keys:
# Google:ClientId and Google:ClientSecret

dotnet user-secrets set "Google:ClientId" "YOUR_GOOGLE_CLIENT_ID"
dotnet user-secrets set "Google:ClientSecret" "YOUR_GOOGLE_CLIENT_SECRET"
```

If your app expects the keys under a different section (for example `Authentication:Google:ClientId`), set them accordingly:

```powershell
dotnet user-secrets set "Authentication:Google:ClientId" "YOUR_GOOGLE_CLIENT_ID"
dotnet user-secrets set "Authentication:Google:ClientSecret" "YOUR_GOOGLE_CLIENT_SECRET"
```

After setting secrets, the backend will pick them up in development when run locally.

## 3. Backend endpoint contract

POST /api/auth/google

Request body (JSON):

```json
{ "idToken": "<Google ID token from client>" }
```

Response (JSON):

```json
{ "token": "<app JWT or session token>" }
```

Expected status codes:

- 200 OK — token issued successfully
- 400 Bad Request — missing or malformed request body (e.g., no idToken)
- 401 Unauthorized — invalid or expired Google ID token
- 500 Internal Server Error — unexpected server error

Make sure your backend verifies the ID token with Google's token verification endpoint or a verified JWT library.
