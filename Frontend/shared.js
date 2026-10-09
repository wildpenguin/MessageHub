import { initializeApp } from "https://www.gstatic.com/firebasejs/13.0.0/firebase-app.js";
import { getAuth, signInWithEmailAndPassword, signOut } from "https://www.gstatic.com/firebasejs/13.0.0/firebase-auth.js";

const firebaseConfig = {
  apiKey: "AIzaSyCvmlflgZa6B7v4C68J4eVxgBy1cigRGrI",
  authDomain: "messagehub-aea2b.firebaseapp.com",
  projectId: "messagehub-aea2b",
  storageBucket: "messagehub-aea2b.firebasestorage.app",
  messagingSenderId: "655791855906",
  appId: "1:655791855906:web:85d34994f1cc46903fad7b"
};

export const auth = getAuth(initializeApp(firebaseConfig));

export const API_BASE = window.location.origin;
export const TOKEN_KEY = "messagehub.token";

// Storage

export function readSetting(key, fallback) {
  try { return localStorage.getItem(key) ?? fallback; } catch { return fallback; }
}

export function writeSetting(key, value) {
  try { localStorage.setItem(key, value); } catch { /* storage unavailable: keep in memory only */ }
}

// Auth

export async function login(email, password) {
  const credential = await signInWithEmailAndPassword(auth, email, password);
  return credential.user.getIdToken();
}

export async function logout() {
  await signOut(auth);
  writeSetting(TOKEN_KEY, "");
}

export function loginErrorMessage(err) {
  switch (err.code) {
    case "auth/invalid-email":       return "That email address isn't valid.";
    case "auth/invalid-credential":  return "Wrong email or password.";
    case "auth/too-many-requests":   return "Too many attempts. Try again in a few minutes.";
    default:
      console.error("Login error:", err);
      return "Login failed. Please try again.";
  }
}

// API

export async function apiRequest(token, path, options = {}) {
  const response = await fetch(`${API_BASE}${path}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
      ...options.headers,
    },
  });

  if (!response.ok) {
    const error = new Error(`${response.status} ${response.statusText}`);
    error.status = response.status;
    throw error;
  }
  return response.status === 204 ? null : response.json();
}

// Layout (React.createElement instead of JSX, since Babel only compiles the page's own script)

const h = React.createElement;

const PAGES = [
  { href: "index.html", label: "Events" },
  { href: "groups.html", label: "Groups" },
  { href: "clients.html", label: "Clients" },
];

export function Header() {
  const current = window.location.pathname.split("/").pop() || "index.html";

  return h("header", { className: "site-header" },
    h("div", { className: "container" },
      h("h1", { className: "brand" }, "Message Hub"),
      h("nav", null,
        PAGES.map(page => h("a", {
          key: page.href,
          href: page.href,
          "aria-current": page.href === current ? "page" : undefined,
        }, page.label))
      )
    )
  );
}

export function Footer() {
  return h("footer", { className: "site-footer" },
    h("div", { className: "container" },
      h("span", null, `© ${new Date().getFullYear()} Message Hub`),
      h("span", null, "Group messaging by email and SMS")
    )
  );
}
