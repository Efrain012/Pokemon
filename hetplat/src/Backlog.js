import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import ClickItemRedirect from './ClickItemRedirect';

export function TaskList() {
  const [tasks, setTasks] = useState([]);
  const [selectedTask, setSelectedTask] = useState(null);
  const { projectId } = useParams();

  useEffect(() => {
    fetch(`https://localhost:7286/api/Data/tasks?projectId=${projectId}`)
      .then((res) => res.json())
      .then((data) => setTasks(data))
      .catch((err) => console.error("Error loading tasks:", err));
  }, [projectId]);

  return (
    <div>
      {tasks.length === 0 ? (
        <p style={{ color: "#666", textAlign: "center" }}>
          No tasks found.
        </p>
      ) : (
        <div
          style={{
            height: "300px",
            overflowY: "auto",
            padding: "10px",
            backgroundColor: "#f5f5f5",
            borderRadius: "10px",
            boxShadow: "inset 0 0 5px rgba(0,0,0,0.1)",
          }}
        >
          {tasks.map((task) => (
            <div
              key={task.id}
              onClick={() => setSelectedTask(task.id)} // open popup
              style={{
                background: "#fff",
                border: "1px solid #ddd",
                borderRadius: "8px",
                padding: "12px",
                marginBottom: "10px",
                cursor: 'pointer',
                boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
              }}
            >
              <h3 style={{ margin: "0 0 5px", fontSize: "16px" }}>{task.title}</h3>
              <p style={{ margin: "0 0 8px", color: "#555", fontSize: "14px" }}>
                Click for details
              </p>
              <div
                style={{
                  display: "flex",
                  justifyContent: "space-between",
                  fontSize: "13px",
                  color: "#666",
                }}
              >
                <span>Priority: {task.priority}</span>
                <span>Type: {task.type}</span>
              </div>
            </div>
          ))}
        </div>
      )}

      {selectedTask && (
        <ClickItemRedirect
          id={selectedTask}
          onClose={() => setSelectedTask(null)}
        />
      )}
    </div>
  );
}


export function ActiveTaskList() {
  const [tasks, setTasks] = useState([]);
const { projectId } = useParams();

  useEffect(() => {
  fetch(`https://localhost:7286/api/Data/tasks?projectId=${projectId}`)
    .then((res) => res.json())
    .then((data) => {
      const activeTasks = data.filter(task => task.status === "active");
      setTasks(activeTasks);
    })
    .catch((err) => console.error("Error loading data:", err));
}, [projectId]);


  

  return (
    <div
      style={{
        height: "300px",
        overflowY: "auto",
        padding: "10px",
        backgroundColor: "#f5f5f5",
        borderRadius: "10px",
        boxShadow: "inset 0 0 5px rgba(0,0,0,0.1)",
      }}
    >
      {tasks.length === 0 ? (
        <p style={{ color: "#666", textAlign: "center" }}>
          No active tasks found.
        </p>
      ) : (
        tasks.map((task) => (
          <div
            key={task.id}
            style={{
              background: "#fff",
              border: "1px solid #ddd",
              borderRadius: "8px",
              padding: "12px",
              marginBottom: "10px",
              boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
            }}
          >
            <h3 style={{ margin: "0 0 5px", fontSize: "16px" }}>{task.title}</h3>
            <p style={{ margin: "0 0 8px", color: "#555", fontSize: "14px" }}>
              {task.description}
            </p>
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                fontSize: "13px",
                color: "#666",
              }}
            >
              <span>Priority: {task.priority}</span>
              <span>Type: {task.type}</span>
            </div>
          </div>
        ))
      )}
    </div>
  );
}
