import { ActionFunctionArgs, json, redirect } from "@remix-run/node";
import { useLoaderData, Form } from "@remix-run/react";
import { useState } from "react";
import { v4 as uuidv4 } from "uuid"; // Importar el generador de UUID

type Person = {
  id: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  email: string;
  phoneNumber: string;
  address: string;
};

type Case = {
  id: string;
  title: string;
  patientId: string;
  offenderId: string;
  dateCreated?: string;
  dateUpdated?: string;
  openCase: boolean;
};

export async function loader() {
  const casesResponse = await fetch(
    `${process.env.API_GW_URI}/docs-ms/ComisaryCase`,
    {
      headers: { Accept: "application/json" },
    }
  );
  const peopleResponse = await fetch(
    `${process.env.API_GW_URI}/people-ms/people`,
    {
      headers: { Accept: "application/json" },
    }
  );

  if (!casesResponse.ok || !peopleResponse.ok) {
    throw new Error("Error fetching data");
  }

  const cases: Case[] = await casesResponse.json();
  const people: Person[] = await peopleResponse.json();

  // Crear un objeto de búsqueda rápida para las personas por su ID
  const peopleById = people.reduce((acc: { [key: string]: Person }, person) => {
    acc[person.id] = person;
    return acc;
  }, {});

  return json({ cases, peopleById });
}

export async function action({ request }: ActionFunctionArgs) {
  const formData = await request.formData();
  const method = formData.get("_method") || "POST";
  const id = formData.get("id");

  let url = `${process.env.API_GW_URI}/docs-ms/ComisaryCase`;
  if (method === "PUT" || method === "DELETE") {
    url += `/${id}`;
  }

  // Generar el ID si es un nuevo caso (cuando es POST)
  const caseId = method === "POST" ? uuidv4() : formData.get("id");

  const payload =
    method === "DELETE"
      ? {}
      : {
          id: caseId, // Usar el ID generado para un nuevo caso
          title: formData.get("title"),
          patientId: formData.get("patientId"),
          offenderId: formData.get("offenderId"),
        };

  await fetch(url, {
    method: method as string,
    headers: { "Content-Type": "application/json" },
    body: method === "DELETE" ? undefined : JSON.stringify(payload),
  });

  return redirect("/cases");
}

export default function Cases() {
  const { cases, peopleById } = useLoaderData<{
    cases: Case[];
    peopleById: { [key: string]: Person };
  }>();
  const [selectedCase, setSelectedCase] = useState<Case | null>(null);

  const handleViewCase = (c: Case) => {
    setSelectedCase(c);
  };

  return (
    <div className="p-8">
      <h1 className="text-3xl font-extrabold mb-6 text-gray-800">
        Gestión de Casos
      </h1>

      {/* Formulario de Creación de Caso */}
      <Form
        method="post"
        className="mb-10 p-6 bg-white rounded-lg shadow-md space-y-4"
      >
        <input type="hidden" name="_method" value="POST" />

        <input
          type="text"
          name="title"
          placeholder="Título del Caso"
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
        />

        {/* Combo box para seleccionar paciente */}
        <select
          name="patientId"
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
        >
          <option value="">Seleccionar Paciente</option>
          {Object.values(peopleById).map((p) => (
            <option key={p.id} value={p.id}>
              {p.firstName} {p.lastName}
            </option>
          ))}
        </select>

        {/* Combo box para seleccionar ofensor */}
        <select
          name="offenderId"
          className="w-full p-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
        >
          <option value="">Seleccionar Ofensor</option>
          {Object.values(peopleById).map((p) => (
            <option key={p.id} value={p.id}>
              {p.firstName} {p.lastName}
            </option>
          ))}
        </select>

        <button
          type="submit"
          className="w-full py-2 px-4 bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 transition-colors"
        >
          Crear Caso
        </button>
      </Form>

      {/* Lista de Casos */}
      <ul className="space-y-4">
        {cases.map((c) => (
          <li
            key={c.id}
            className="p-4 bg-gray-50 rounded-lg shadow cursor-pointer flex items-center justify-between"
            onClick={() => handleViewCase(c)} // Mostrar detalles al hacer clic
          >
            <span className="text-gray-800 font-medium">{c.title}</span>
          </li>
        ))}
      </ul>

      {/* Mostrar detalles del caso seleccionado */}
      {selectedCase && (
        <div className="mt-8 p-6 bg-white rounded-lg shadow-md">
          <h2 className="text-2xl font-bold text-gray-800 mb-4">
            Detalles del Caso
          </h2>
          <p>
            <strong>ID:</strong> {selectedCase.id}
          </p>
          <p>
            <strong>Título:</strong> {selectedCase.title}
          </p>

          {/* Datos del paciente */}
          {selectedCase.patientId && peopleById[selectedCase.patientId] && (
            <div>
              <h3 className="text-lg font-bold text-gray-700">Paciente:</h3>
              <p>
                {peopleById[selectedCase.patientId].firstName}{" "}
                {peopleById[selectedCase.patientId].lastName}
              </p>
              <p>{peopleById[selectedCase.patientId].email}</p>
              <p>{peopleById[selectedCase.patientId].phoneNumber}</p>
            </div>
          )}

          {/* Datos del ofensor */}
          {selectedCase.offenderId && peopleById[selectedCase.offenderId] && (
            <div className="mt-4 mb-4">
              <h3 className="text-lg font-bold text-gray-700">Ofensor:</h3>
              <p>
                {peopleById[selectedCase.offenderId].firstName}{" "}
                {peopleById[selectedCase.offenderId].lastName}
              </p>
              <p>{peopleById[selectedCase.offenderId].email}</p>
              <p>{peopleById[selectedCase.offenderId].phoneNumber}</p>
            </div>
          )}

          <p>
            <strong>Fecha Creación:</strong> {selectedCase.dateCreated}
          </p>
          <p>
            <strong>Fecha Actualización:</strong> {selectedCase.dateUpdated}
          </p>
          <p>
            <strong>Estado:</strong>{" "}
            {selectedCase.openCase ? "Abierto" : "Cerrado"}
          </p>
        </div>
      )}
    </div>
  );
}
