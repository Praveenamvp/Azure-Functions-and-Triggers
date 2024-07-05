import "./App.css";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import UploadFile from "./Components/UploadFile/UploadFile";

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<UploadFile />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
