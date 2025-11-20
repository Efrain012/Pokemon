# HetplatProyecto

Goji es una aplicación para el manejo de proyectos que utilizan las metodologias de SCRUM y de Waterfall.

## Progreso

[✅] Vista de proyectos SCRUM y Waterfall
- Detalles de tarea
- Calendario de tarea
- Visualización de proyectos
- Interfaz de inicio de sesion

[❗] Implementación de API
- Ya se tiene una API rudimentaria que utiliza JSON para guardar los archivos, el siguiente paso es implementar una base de datos donde se guarde todo, y de esa base de datos vamos a sacar y convertir en json las tareas y proyectos.

De momento no se implementa la funcion de añadir tareas o proyectos ya que seria agregar complejidad inecesaria, pues las nuevas tareas se van a crear en base de datos, por lo tanto se tomo la decision de esperar al siguiente avance donde vamos a tener una capa de persistencia mejor definida para implementar esa funcionalidad.

## Como ejecutar

Es importante tener instalado ASP.NET, visual studio, y npm. 
npm : https://docs.npmjs.com/downloading-and-installing-node-js-and-npm

Los siguientes paquetes de npm son necesarios para la correcta ejecución del proyecto.

Para poder ejecutar un proyecto de React.
`npm install react`
`npm install react react-dom`

Para poder compilar el enrutador de vistas
`npm install react-router-dom`

Para poder compilar la funcion de calendario
`npm install @fullcalendar/react @fullcalendar/daygrid @fullcalendar/timegrid @fullcalendar/interaction`

(si falto algo una disculpa profe ;-; tenga piedad)

### Iniciar la API
Una vez se tengan todos los paquetes instalados, abra el archivo de VisualStudio en el directorio de API_PROYECTO y ejecute el proyecto para iniciar el servicio de la API en https://localhost:7286 (por lo tanto es importante tener libre ese puerto a la hora de ejecutar!)

### Iniciar la SPA
Dentro del directorio de hetplat/src abra una linea de comando (cmd) y ejecute el comando ``npm start``

En automatico se abre una pagina de swagger para probar que la API funciona correctamente, y se abre la pagina de Login de la APP.
