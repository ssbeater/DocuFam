// app/routes/places.tsx
import { ActionFunctionArgs, json, redirect } from "@remix-run/node";
import { useLoaderData, Form } from "@remix-run/react";
import { useState } from "react";

type Place = {
  id: number;
  name: string;
  description: string;
  location: string;
};

// Fetch places on load
export async function loader() {
  const response = await fetch(
    `${process.env.API_GW_URI}/place-ms/places`,
    {
      headers: { Accept: "application/json" },
    }
  );

  if (!response.ok) {
    throw new Error("Error fetching places");
  }

  const places: Place[] = await response.json();
  return json(places);
}

// Handle CRUD actions
export async function action({ request }: ActionFunctionArgs) {
  const formData = await request.formData();
  const method = formData.get("_method") || "POST";

  let url = `${process.env.API_GW_URI}/place-ms/places`;
  const id = formData.get("id");

  if (method === "PUT" || method === "DELETE") {
    url += `/${id}`;
  }

  const payload =
    method === "DELETE"
      ? {}
      : {
          name: formData.get("name"),
          description: formData.get("description"),
          location: formData.get("location"),
        };

  await fetch(url, {
    method: method as string,
    headers: { "Content-Type": "application/json" },
    body: method === "DELETE" ? undefined : JSON.stringify(payload),
  });

  return redirect("/places");
}

export default function Places() {
  const places = useLoaderData<Place[]>();

  const [place, setPlace] = useState<Place>({
    id: 0,
    name: "",
    description: "",
    location: "",
  });

  const handleEdit = (p: Place) => {
    setPlace(p);
  };

  return (
    <div className="p-8">
      <h1 className="text-3xl font-extrabold mb-6 text-gray-800">
        Gestión de Lugares
      </h1>

      <Form
        method="post"
        className="mb-10 p-6 bg-white rounded-lg shadow-md space-y-4"
      >
        <input type="hidden" name="_method" value={place.id ? "PUT" : "POST"} />
        <input type="hidden" name="id" value={place.id} />

        <input
          type="text"
          name="name"
          placeholder="Name"
          defaultValue={place.name}
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
        />
        <input
          type="text"
          name="description"
          placeholder="Description"
          defaultValue={place.description}
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
        />
        <input
          type="text"
          name="location"
          placeholder="Location"
          defaultValue={place.location}
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
        />

        <button
          type="submit"
          className="w-full py-2 px-4 bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 transition-colors"
        >
          {place.id ? "Actualizar" : "Crear"}
        </button>
      </Form>

      <ul className="space-y-4">
        {places.map((p) => (
          <li
            key={p.id}
            className="p-4 bg-gray-50 rounded-lg shadow flex items-center justify-between"
          >
            <span className="text-gray-800 font-medium">
              {p.name} - {p.location}
            </span>
            <div className="flex space-x-2">
              <button
                onClick={() => handleEdit(p)}
                className="px-4 py-2 bg-yellow-500 text-white rounded-lg font-semibold hover:bg-yellow-600 transition-colors"
              >
                Editar
              </button>
              <Form method="post">
                <input type="hidden" name="_method" value="DELETE" />
                <input type="hidden" name="id" value={p.id} />
                <button
                  type="submit"
                  className="px-4 py-2 bg-red-500 text-white rounded-lg font-semibold hover:bg-red-600 transition-colors"
                >
                  Eliminar
                </button>
              </Form>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}
