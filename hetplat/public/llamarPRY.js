document.addEventListener("DOMContentLoaded", () => {
    const tableBody = document.querySelector("#Proyectos-table tbody");

    // Cargar el JSON con fetch
    fetch("Proyectos.json")
        .then((response) => {
            if (!response.ok) throw new Error("Error al cargar Proyectos.json");
            return response.json();
        })
        .then((data) => {
            // Recorrer cada elemento del Proyectos y crear filas
            data.forEach((item) => {
                const row = document.createElement("tr");

            //item path es la direccion de la vista del proyecto
                row.innerHTML = `
          <td><a href="${item.path}" target="_self">${item.id}</a></td>
          <td>${item.Nombre}</td>
          <td>${item.Descripcion}</td>
        `;

                tableBody.appendChild(row);
            });
        })
        //caso de error
        .catch((error) => {
            console.error("Error:", error);
            const row = document.createElement("tr");
            row.innerHTML = `<td colspan="5">No se pudo cargar lista de proyectos</td>`;
            tableBody.appendChild(row);
        });
});
