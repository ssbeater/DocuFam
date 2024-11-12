import {
  Link,
  Links,
  Meta,
  Outlet,
  Scripts,
  ScrollRestoration,
} from "@remix-run/react";
import type { LinksFunction } from "@remix-run/node";

import "./tailwind.css";

export const links: LinksFunction = () => [
  { rel: "preconnect", href: "https://fonts.googleapis.com" },
  {
    rel: "preconnect",
    href: "https://fonts.gstatic.com",
    crossOrigin: "anonymous",
  },
  {
    rel: "stylesheet",
    href: "https://fonts.googleapis.com/css2?family=Inter:ital,opsz,wght@0,14..32,100..900;1,14..32,100..900&display=swap",
  },
];

export function Layout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <Meta />
        <Links />
      </head>
      <body>
        {children}
        <ScrollRestoration />
        <Scripts />
      </body>
    </html>
  );
}

export default function App() {
  return (
    <div className="min-h-screen bg-gray-100">
      <nav className="flex items-center justify-center gap-4 pt-4">
        <Link
          to="/cases"
          className="px-4 py-2 text-lg text-center hover:text-white bg-gray-200 rounded hover:bg-gray-700 min-w-40"
        >
          Casos
        </Link>
        <Link
          to="/people"
          className="px-4 py-2 text-lg text-center hover:text-white bg-gray-200 rounded hover:bg-gray-700 min-w-40"
        >
          Personas
        </Link>
        <Link
          to="/places"
          className="px-4 py-2 text-lg text-center hover:text-white bg-gray-200 rounded hover:bg-gray-700 min-w-40"
        >
          Lugares
        </Link>
      </nav>

      <main>
        <Outlet />
      </main>
    </div>
  );
}
