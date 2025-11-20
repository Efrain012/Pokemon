import React, { useState, useEffect } from 'react';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import './Calendario.css';
import { useParams } from 'react-router-dom';

const Calendario = () => {
  const [eventos, setEventos] = useState([]);
  const { projectId } = useParams(); // get projectId from route

  useEffect(() => {
    if (!projectId) return;

    fetch(`https://localhost:7286/api/Data/tasks?projectId=${projectId}`)
      .then((res) => res.json())
      .then((data) => {
        const eventosFormateados = data.map((task) => ({
          id: task.id,
          title: task.title,
          start: task.start,      // from API
          end: task.deadline,     // from API
          backgroundColor: getColor(task.status),
        }));
        setEventos(eventosFormateados);
      })
      .catch((err) => console.error('Error cargando tareas:', err));
  }, [projectId]);

  const getColor = (status) => {
    if (status === 'active') return '#3498db';       // blue for active
    if (status === 'inactive') return '#95a5a6';     // gray for inactive
    return '#2ecc71';                                // green default
  };

  return (
    
    <div className="calendario-container">
  

  <div className="calendario-content" style={{ height: 'calc(100vh - 80px)' }}>
    {/* 80px = approximate header height */}
    <div className="calendario-header">
      <h1>Calendario</h1>
    </div>

    <div className="calendario-main" style={{ height: '100%' }}>
      <FullCalendar
        plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
        initialView="dayGridMonth"
        headerToolbar={{
          left: 'prev,next today',
          center: 'title',
          right: 'dayGridMonth,timeGridWeek',
        }}
        events={eventos}
        locale="es"
        height="100%"           // <-- fill the parent container
      />
    </div>
  </div>
</div>

  );
};

export default Calendario;
