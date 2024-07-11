import React, { useEffect, useState, useRef } from "react";
import "./UploadFile.css";
import FileService from "../../Services/FileService";

function UploadFile() {
  const [containers, setContainers] = useState<any[]>([]);
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    const fetchLanguages = async () => {
      try {
        const languageDatas = await FileService.fetchAllContainerAndBlobs();
        setContainers(languageDatas);
      } catch (error) {
        console.error('Error fetching containers and blobs:', error);
      }
    };
    fetchLanguages();
  }, []);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setSelectedFile(event.target.files[0]);
    }
  };

  const handleUpload = async () => {
    if (selectedFile) {
      try {
        const result = await FileService.uploadBlob(selectedFile);
        if (result) {
          alert('File uploaded successfully!');
          setSelectedFile(null);
          if (fileInputRef.current) {
            fileInputRef.current.value = '';
          }
          const languageDatas = await FileService.fetchAllContainerAndBlobs();
          setContainers(languageDatas);
        } else {
          alert('Failed to upload file.');
        }
      } catch (error) {
        alert('Error uploading file.');
      }
    }
  };

  return (
    <div className="upload-container">
    <div className="list">
    {Object.keys(containers).map((containerName: any) => (
        <div key={containerName} className="list-container">
          <h2>{containerName}</h2>
          <ul>
            {containers[containerName].map((blob: string) => (
              <li key={blob}>{blob}</li>
            ))}
          </ul>
        </div>
      ))}
    </div>
      
      <div className="upload-controls">
        <input
          type="file"
          className="file-input"
          onChange={handleFileChange}
          ref={fileInputRef}
        />
        <button className="upload-button" onClick={handleUpload}>
          Upload
        </button>
      </div>
    </div>
  );
}

export default UploadFile;
