import React, { useState, useEffect } from 'react';
import AllComponent from './AllComponent';
import ClickItemRedirect from './ClickItemRedirect';

const ListsComponents = () => {
  const [jsonData, setJsonData] = useState([]);
  const [selectedId, setSelectedId] = useState(null);

  useEffect(() => {
    fetch('/data.json')
      .then((response) => response.json())
      .then((data) => setJsonData(data))
      .catch((error) => console.error('Error fetching data: ', error));
  }, []);

  return (
    <div className="ListsComponentsMain">
      <h1>Tasks</h1>
      <div className="ListsComponentsGrid" style={{ pointerEvents: selectedId ? 'none' : 'auto' }}>
        <div className="ListsComponents">
          <h2 className="section-title">TO DO</h2>
          <AllComponent 
            jsonData={jsonData} 
            filter={"Description for Card 1"} 
            onItemClick={setSelectedId}
          />
        </div>
        <div className="ListsComponents">
          <h2 className="section-title">IN PROGRESS</h2>
          <AllComponent 
            jsonData={jsonData} 
            filter={"Description for Card 2"}
            onItemClick={setSelectedId}
          />
        </div>
        <div className="ListsComponents">
          <h2 className="section-title">CODE REVIEW</h2>
          <AllComponent 
            jsonData={jsonData} 
            filter={"Description for Card 3"}
            onItemClick={setSelectedId}
          />
        </div>
        <div className="ListsComponents">
          <h2 className="section-title">DONE</h2>
          <AllComponent 
            jsonData={jsonData} 
            filter={"Description for Card 4"}
            onItemClick={setSelectedId}
          />
        </div>
      </div>

      {selectedId && (
        <ClickItemRedirect 
          id={selectedId} 
          onClose={() => setSelectedId(null)} 
        />
      )}
    </div>
  );
};

export default ListsComponents;