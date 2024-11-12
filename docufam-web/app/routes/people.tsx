// app/routes/personas.tsx
import { redirect, json, ActionFunctionArgs } from "@remix-run/node";
import { useLoaderData, Form } from "@remix-run/react";
import { useState } from "react";
import { Person } from "~/types/Person";

// Loader para obtener todas las personas
export async function loader() {
  const response = await fetch(
    `${process.env.API_GW_URI}/people-ms/people`,
    {
      headers: { Accept: "application/json" },
    }
  );

  if (!response.ok) {
    throw new Error("Error fetching people");
  }

  const people: Person[] = await response.json(); // Ahora people tiene tipo Person[]
  return json(people);
}

export async function action({ request }: ActionFunctionArgs) {
  const formData = await request.formData();
  const method = formData.get("_method");

  const id = formData.get("id");
  const firstName = formData.get("firstName");
  const lastName = formData.get("lastName");
  const dateOfBirth = formData.get("dateOfBirth");
  const email = formData.get("email");
  const phoneNumber = formData.get("phoneNumber");
  const address = formData.get("address");

  if (method === "POST") {
    const response = await fetch(
      `${process.env.API_GW_URI}/people-ms/people`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          firstName,
          lastName,
          dateOfBirth,
          email,
          phoneNumber,
          address,
        }),
      }
    );
    if (!response.ok) throw new Error(`Error with ${method} request`);
  } else if (method === "PUT") {
    const response = await fetch(
      `${process.env.API_GW_URI}/people-ms/people/${id}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          id,
          firstName,
          lastName,
          dateOfBirth,
          email,
          phoneNumber,
          address,
        }),
      }
    );
    if (!response.ok) throw new Error(`Error with ${method} request`);
  } else if (method === "DELETE") {
    const response = await fetch(
      `${process.env.API_GW_URI}/people-ms/people/${id}`,
      {
        method: "DELETE",
      }
    );
    if (!response.ok) throw new Error(`Error with ${method} request`);
  }

  return redirect("/people");
}

// Componente de React
export default function Personas() {
  const people = useLoaderData<Person[]>(); // Definimos el tipo de people como Person[]

  const [person, setPerson] = useState<Person>({
    id: "",
    firstName: "",
    lastName: "",
    dateOfBirth: "",
    email: "",
    phoneNumber: "",
    address: "",
  });

  // Función para formatear la fecha en 'YYYY-MM-DD'
  const formatDateForInput = (dateString: string) => {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0"); // Mes en dos dígitos
    const day = String(date.getDate()).padStart(2, "0"); // Día en dos dígitos
    return `${year}-${month}-${day}`;
  };

  const handleEdit = (p: Person) => {
    setPerson({
      ...p,
      dateOfBirth: formatDateForInput(p.dateOfBirth), // Formateamos la fecha aquí
    });
  };

  return (
    <div className="p-8">
      <h1 className="text-3xl font-extrabold mb-6 text-gray-800">
        Gestión de Personas
      </h1>

      <Form
        method="post"
        className="mb-10 p-6 bg-white rounded-lg shadow-md space-y-4"
      >
        <input
          type="hidden"
          name="_method"
          value={person.id ? "PUT" : "POST"}
        />
        <input type="hidden" name="id" value={person.id} />

        <input
          type="text"
          name="firstName"
          placeholder="First Name"
          defaultValue={person.firstName}
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
          required
        />
        <input
          type="text"
          name="lastName"
          placeholder="Last Name"
          defaultValue={person.lastName}
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
          required
        />
        <input
          type="date"
          name="dateOfBirth"
          defaultValue={person.dateOfBirth}
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
          required
        />
        <input
          type="email"
          name="email"
          placeholder="Email"
          defaultValue={person.email}
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
          required
        />
        <input
          type="text"
          name="phoneNumber"
          placeholder="Phone Number"
          defaultValue={person.phoneNumber}
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
          required
        />
        <input
          type="text"
          name="address"
          placeholder="Address"
          defaultValue={person.address}
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
          required
        />

        <button
          type="submit"
          className="w-full py-2 px-4 bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 transition-colors"
        >
          {person.id ? "Actualizar" : "Crear"}
        </button>
      </Form>

      <ul className="space-y-4">
        {people.map((p) => (
          <li
            key={p.id}
            className="p-4 bg-gray-50 rounded-lg shadow flex items-center justify-between"
          >
            <span className="text-gray-800 font-medium">
              {p.firstName} {p.lastName} - {p.email}
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
