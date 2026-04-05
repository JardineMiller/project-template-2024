# Google OAuth Setup (Client)

This document explains how to configure the frontend (Vite) with the Google Client ID used for authentication.

## 1. Choose credentials type in Google Cloud Console

When creating credentials in the Google Cloud Console, select **Web application** (OAuth 2.0 Client IDs). Add the frontend origin to **Authorized JavaScript origins**, for example `http://localhost:5173` for local Vite development.

## 2. Configure Vite (.env.development)

Create a `.env.development` file in the Client project root (do not check this file into source control):

```
VITE_GOOGLE_CLIENT_ID=YOUR_GOOGLE_CLIENT_ID
```

Notes:

- Vite exposes env variables prefixed with `VITE_` to client-side code. Access it in your code via `import.meta.env.VITE_GOOGLE_CLIENT_ID`.
- Never commit client secrets or .env files with credentials. Add `.env*` to your .gitignore if not already ignored.

## 3. Client-to-backend flow

Typical flow:

1. The client obtains a Google ID token (for example using Google Identity Services).
2. The client sends the ID token to the backend: POST /api/auth/google with JSON `{ idToken }`.
3. The backend verifies the ID token and issues an application token (e.g., JWT) returned as `{ token }`.

Refer to the Backend README for the exact endpoint contract and status codes.
