
import React, { useState, useEffect } from 'react';
import { BrowserRouter, Routes, Route, Link, useNavigate } from "react-router-dom";
import {TaskList, ActiveTaskList} from "./Backlog";
import DependentTaskList from "./WaterfallBacklog";
import Calendario from "./Calendario";

export const HStack = ({ children, style, gap = '1rem', justifyContent = 'flex-start', alignItems = 'center' }) => {
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


export const VStack = ({ children, style, gap = '0.75rem', justifyContent = 'flex-start', alignItems = 'stretch' }) => {
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


function TopBarLayout({ children, gap = "1rem", style }) {
  const barHeight = 60;
  const navigate = useNavigate();
  const containerStyle = {
    display: "flex",
    flexDirection: "column", // actúa como VStack
    gap,
    ...style,
  };

  const styles = {
    topbar: {
      width: "100%",
      height: `${barHeight}px`,
      backgroundColor: "#333",
      color: "white",
      display: "flex",
      alignItems: "center",
      padding: "0 20px",
      boxSizing: "border-box",
      position: "fixed",
      top: 0,
      left: 0,
      zIndex: 1000,
    },
    content: {
      paddingTop: `${barHeight}px`, // empuja el contenido hacia abajo
    },
  };

  return (
    <div style={containerStyle}>
      <div style={styles.topbar}>

         <HStack gap= '5vw'style={{ width: '100%' }}>

          <h1>
            Menu
          </h1>

          <h1>
            Proyecto Ejemplo
          </h1>

          <h1 style={{ marginLeft: "auto" }}>
          <button onClick={() => navigate(-1)}
                style={{
            all: "unset", // removes all default styles
            cursor: "pointer", // optional: show pointer on hover
            color: "inherit", // inherit h1 color
            fontSize: "inherit", // inherit h1 font size
            fontFamily: "inherit", // inherit font
          }}>
            
            GOJI
            
          </button> {/* Go back one step */}
          </h1>


         </HStack>
        

      </div>
      <div style={styles.content}>{children}</div>
    </div>
  );
}


const ScrumView = () => {

  
  return(

    

    <ZStack style={{ width: '100%' , paddingLeft : '50px'}}>
      
      <VStack style={{ width: '100%' }}>
        <TopBarLayout/>

        <HStack gap= '5vw'style={{ width: '100%' }}>

          <VStack style={{ width: '20%', boxShadow: '5px 5px 10px rgba(0, 0, 0, 0.5)' , borderRadius : '10px'
            , padding : '5px'
          }}>
            <h1>PRODUCT BACKLOG</h1>
            <TaskList />

          </VStack>
          
          
          <VStack style={{ width: '20%', boxShadow: '5px 5px 10px rgba(0, 0, 0, 0.5)' , borderRadius : '10px'
            , padding : '5px'
          }}>
            <h1>SPRINT BACKLOG</h1>
            <ActiveTaskList />

          </VStack>

        </HStack>

        <Calendario></Calendario>
      </VStack>
    

    </ZStack>

  );

};


const WaterfallView = () => {

  return(

    

    <ZStack style={{ width: '100%' , paddingLeft : '50px'}}>
      
      <VStack style={{ width: '100%' }}>
        <TopBarLayout/>

        <HStack gap= '5vw'style={{ width: '100%' }}>

          <VStack style={{ width: '20%', boxShadow: '5px 5px 10px rgba(0, 0, 0, 0.5)' , borderRadius : '10px'
            , padding : '5px'
          }}>
            <h1>PRODUCT BACKLOG</h1>
            <TaskList />

          </VStack>
          
          
          <VStack style={{ width: '60%', boxShadow: '5px 5px 10px rgba(0, 0, 0, 0.5)' , borderRadius : '10px'
            , padding : '5px'
          }}>
            <h1>Tareas</h1>
            <DependentTaskList></DependentTaskList>
          
            
          </VStack>

        </HStack>

            <Calendario></Calendario>
      </VStack>
    

    </ZStack>

  );

};

function ProjectsTable() {
  const [projects, setProjects] = useState([]);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetch("/Proyectos.json")
      .then((res) => {
        if (!res.ok) throw new Error("Error al cargar Proyectos.json");
        return res.json();
      })
      .then((data) => setProjects(data))
      .catch((err) => {
        console.error(err);
        setError("No se pudo cargar lista de proyectos");
      });
  }, []);

  // Handler to navigate based on tipo and projectId
  const handleRowClick = (proj) => {
    if (proj.Tipo === "Waterfall") {
      navigate(`/waterfall/${proj.id}`);
    } else if (proj.Tipo === "Scrum") {
      navigate(`/scrum/${proj.id}`);
    } else {
      console.warn("Tipo de proyecto desconocido:", proj.Tipo);
    }
  };

  return (
    <VStack style={{ padding: "20px", width: "100%" }} gap="1.5rem">
      <h1>Proyectos</h1>

      <table style={{ width: "100%", borderCollapse: "collapse" }}>
        <thead>
          <tr>
            <th>ID</th>
            <th>Tipo</th>
            <th>Nombre</th>
            <th>Descripcion</th>
          </tr>
        </thead>
        <tbody>
          {error ? (
            <tr>
              <td colSpan={4}>{error}</td>
            </tr>
          ) : (
            projects.map((proj) => (
              <tr
                key={proj.id}
                onClick={() => handleRowClick(proj)}
                style={{ cursor: "pointer" }}
              >
                <td>{proj.id}</td>
                <td>{proj.Tipo}</td>
                <td>{proj.Nombre}</td>
                <td>{proj.Descripcion}</td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </VStack>
  );
}
// Componente principal que renderiza el ejemplo
function HomeView() {
  const navigate = useNavigate();
  return (
    <div>
      
      <VStack>
        <HStack>
        <h1>Home Page</h1>
        <h3 style={{ marginLeft: "auto" }}>
          <button onClick={() => navigate(-1)}
                style={{
            all: "unset", // removes all default styles
            cursor: "pointer", // optional: show pointer on hover
            color: "inherit", // inherit h1 color
            fontSize: "inherit", // inherit h1 font size
            fontFamily: "inherit", // inherit font
          }}>
            
            Log Out
            
          </button> {/* Go back one step */}
          </h3>
          
        </HStack>
        
        <ProjectsTable />
      </VStack>
    </div>
  );
}


function LoginView() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleLogin = (e) => {
    e.preventDefault();

    // Mock validation
    if (username === 'admin' && password === '1234') {
      navigate('/home'); // redirect to home
    } else {
      setError('Invalid username or password');
    }
  };

  return (
    <VStack style = {{backgroundColor: '#f5f5f5'}}>
      <h1 style = {{textAlign : 'center'}}>GOJI</h1>
      <img src ="./logo512.png" style = {{width : '200px' , margin : 'auto', padding : '30px'}}></img>
    <div
      style={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        
      }}
    >
      <form
        onSubmit={handleLogin}
        style={{
          background: '#fff',
          padding: '30px',
          borderRadius: '8px',
          boxShadow: '0 4px 8px rgba(0,0,0,0.1)',
          width: '300px',
        }}
      >
        <h2 style={{ textAlign: 'center', marginBottom: '20px' }}>Login</h2>
        {error && <p style={{ color: 'red', textAlign: 'center' }}>{error}</p>}
        <div style={{ marginBottom: '15px' }}>
          <label>Username</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            style={{
              width: '100%',
              padding: '8px',
              marginTop: '5px',
              borderRadius: '4px',
              border: '1px solid #ccc',
            }}
          />
        </div>
        <div style={{ marginBottom: '20px' }}>
          <label>Password</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            style={{
              width: '100%',
              padding: '8px',
              marginTop: '5px',
              borderRadius: '4px',
              border: '1px solid #ccc',
            }}
          />
        </div>
        <button
          type="submit"
          style={{
            width: '100%',
            padding: '10px',
            backgroundColor: '#3498db',
            color: '#fff',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer',
          }}
        >
          Login
        </button>
      </form>
      
    </div>
    <p style={{
    textAlign: 'center',
    color: '#808080', // A shade of gray
    fontSize: '0.8em', // Adjust as needed for small size
  }}>
        RECOMENDACION : NUNCA pongas le pongas a tu 
        usuario 'admin' la contraseña '1234' (esa es la contraseña profe)</p></VStack>
  );
}


const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<LoginView />} />
        <Route path="/home" element={<HomeView />} />
        <Route path="/waterfall/:projectId" element={<WaterfallView />} />
        <Route path="/scrum/:projectId" element={<ScrumView />} />
      </Routes>
    </BrowserRouter>
  );
};


export default App;