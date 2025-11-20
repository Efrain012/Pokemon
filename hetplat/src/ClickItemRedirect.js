import React, { useState, useEffect } from 'react';

const ClickItemRedirect = ({ id, onClose }) => {
  const [data, setData] = useState(null);
  const [notes, setNotes] = useState('');

  useEffect(() => {
    if (!id) return;

    fetch(`https://localhost:7286/api/Data/task/${id}`) // fetch single task by ID
      .then((response) => {
        if (!response.ok) throw new Error("Task not found");
        return response.json();
      })
      .then((task) => setData(task))
      .catch((error) => console.error('Error fetching task: ', error));
  }, [id]);

  const handleClose = () => {
    onClose();
  };

  if (!data) return null;

  return (
    <div style={{ position: 'fixed', inset: 0, zIndex: 9999 }}>
      <div
        className="popup-overlay"
        onClick={handleClose}
        style={{
          position: 'absolute',
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          backgroundColor: 'rgba(44, 44, 40, 0.85)',
          cursor: 'pointer'
        }}
      />

      <div
        className="card popup-container"
        onClick={(e) => e.stopPropagation()}
        style={{
          position: 'absolute',
          top: '50%',
          left: '50%',
          transform: 'translate(-50%, -50%)',
          zIndex: 10000,
          backgroundColor: '#fff',
          padding: '20px',
          borderRadius: '8px',
          width: '300px'
        }}
      >
        <button
          className="popup-close-btn"
          onClick={handleClose}
          style={{
            float: 'right',
            fontSize: '18px',
            border: 'none',
            background: 'transparent',
            cursor: 'pointer'
          }}
        >
          ×
        </button>

        <h1>{data.title}</h1>
        <p>{data.description}</p>

        <div className="popup-content-label">Priority</div>
        <div className="popup-content-value">{data.priority}</div>

        <div className="popup-content-label">Type</div>
        <div className="popup-content-value">{data.type}</div>

        <div className="popup-content-label">Notes</div>
        <textarea
          className="popup-textarea"
          placeholder="Add your notes here..."
          value={notes}
          onChange={(e) => setNotes(e.target.value)}
          style={{ width: '100%', height: '60px', marginTop: '5px' }}
        />
      </div>
    </div>
  );
};

export default ClickItemRedirect;
