import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";



const HStack = ({ children, style, gap = '1rem', justifyContent = 'flex-start', alignItems = 'center' }) => {
  const hStackStyle = {
    display: 'flex',
    flexDirection: 'row',
    gap,
    justifyContent,
    alignItems,
    ...style, // Allows user to override or add styles.
  };
  return <div style={hStackStyle}>{children}</div>;
};


const VStack = ({ children, style, gap = '0.75rem', justifyContent = 'flex-start', alignItems = 'stretch' }) => {
  const vStackStyle = {
    display: 'flex',
    flexDirection: 'column',
    gap,
    justifyContent,
    alignItems,
    ...style,
  };
  return <div style={vStackStyle}>{children}</div>;
};

const ZStack = ({ children, style }) => {
  // El estilo del contenedor padre
  const zStackContainerStyle = {
    position: 'relative',
    ...style, 
  };

  
  const stackedChildren = React.Children.map(children, (child) => {
    if (!React.isValidElement(child)) {
      return child;
    }

    // Agrega absolute
    const childStyleWithPosition = {
      ...child.props.style, // Preserva los estilos del hijo 
      position: 'absolute', 
    };

    
    return React.cloneElement(child, { style: childStyleWithPosition });
  });
return (
    <div style={zStackContainerStyle}>
      {stackedChildren}
    </div>
  );
};



/////////////////////////////
export default function DependentTaskList() {
  const [tasks, setTasks] = useState([]);
  
const { projectId } = useParams();

  useEffect(() => {
    fetch(`https://localhost:7286/api/Data/tasks?projectId=${projectId}`)
      .then((res) => res.json())
      .then((data) => setTasks(data))
      .catch((err) => console.error("Error loading data:", err));
  }, [projectId]);

  // Build levels based on dependencies
  const buildLevels = (tasks) => {
    const map = {};
    tasks.forEach((t) => (map[t.id] = { ...t }));
    const levels = [];
    const added = new Set();
    const levelMap = {}; // Track which level each task is in
    
    while (added.size < tasks.length) {
      const level = tasks.filter((t) => {
        if (added.has(t.id)) return false;
        if (!t.dependsOn) return true;
        // Handle both single dependency and array of dependencies
        const deps = Array.isArray(t.dependsOn) ? t.dependsOn : [t.dependsOn];
        return deps.every(dep => added.has(dep));
      });
      if (level.length === 0) break; // prevent infinite loop on broken deps
      level.forEach((t) => {
        added.add(t.id);
        levelMap[t.id] = levels.length;
      });
      levels.push(level);
    }
    
    // Find lowest level task with multiple dependencies
    let lowestMultiDepTask = null;
    let lowestLevel = -1;
    
    tasks.forEach(task => {
      const deps = task.dependsOn ? (Array.isArray(task.dependsOn) ? task.dependsOn : [task.dependsOn]) : [];
      if (deps.length > 1) {
        const taskLevel = levelMap[task.id];
        if (taskLevel > lowestLevel) {
          lowestLevel = taskLevel;
          lowestMultiDepTask = task.id;
        }
      }
    });
    
    return { levels, lowestMultiDepTask };
  };

  const { levels, lowestMultiDepTask } = buildLevels(tasks);

  return (
    <div style={{ overflowX: "auto", padding: "10px", height: "300px", backgroundColor: "#f5f5f5", borderRadius: "10px" }}>
      {tasks.length === 0 ? (
        <p style={{ textAlign: "center", color: "#666" }}>No tasks found.</p>
      ) : (
        <HStack gap="2rem" style={{ alignItems: "flex-start" }}>
          {levels.map((levelTasks, idx) => (
            <VStack key={idx} gap="1rem" style={{ alignItems: "flex-start" }}>
              <p style = {{marginTop : 'auto'}}>Nivel de dependencia : {idx}</p>
              {levelTasks.map((task) => (
                <div
                  key={task.id}
                  style={{
                    background: "#e0f7fa",
                    border:  "1px solid #ddd",
                    borderRadius: "8px",
                    padding: "12px",
                    minWidth: "150px",
                    boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
                  }}
                >
                  <h3 style={{ margin: 0, fontSize: "16px" }}>
                    {task.title}
                    
                  </h3>
                  <p style={{ margin: "4px 0 0", fontSize: "14px", color: "#555" }}>{task.description}</p>
                  <div style={{ fontSize: "13px", color: "#666", marginTop: "4px" }}>
                    <span>Priority: {task.priority}</span> | <span>Type: {task.type}</span> <br></br>
                    <p>Depends on : {task.dependsOn.length === 0 ? "Nothing" : task.dependsOn.join(", ") }</p>

                  </div>
                </div>
              ))}
            </VStack>
          ))}
        </HStack>
      )}
    </div>
  );
}