document.addEventListener("DOMContentLoaded", () => {
    const tableBody = document.querySelector("#backlog-table tbody");

    // Cargar el JSON con fetch
    fetch("backlog.json")
        .then((response) => {
            if (!response.ok) throw new Error("Error al cargar backlog.json");
            return response.json();
        })
        .then((data) => {
            // Recorrer cada elemento del backlog y crear filas
            data.forEach((item) => {
                const row = document.createElement("tr");

                row.innerHTML = `
          <td>${item.id}</td>
          <td>${item.titulo}</td>
          <td>${item.prioridad}</td>
          <td>${item.estado}</td>
          <td>${item.responsable}</td>
        `;

                tableBody.appendChild(row);
            });
        })
        //caso de error
        .catch((error) => {
            console.error("Error:", error);
            const row = document.createElement("tr");
            row.innerHTML = `<td colspan="5">No se pudo cargar el backlog</td>`;
            tableBody.appendChild(row);
        });
});
