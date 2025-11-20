import React from 'react';
import { useNavigate } from "react-router-dom";
import ClickItemRedirect from './ClickItemRedirect';


const IndividualComponent = ({ data, onClick }) => {
  const handleClick = (e) => {
    e.stopPropagation();
    onClick(data.id);
  };

  return (
    <div className="card" onClick={() => onClick(data.id)} style={{ cursor: 'pointer' }}>
      <h2>{data.title}</h2>
    </div>
  );
};

export default IndividualComponent;