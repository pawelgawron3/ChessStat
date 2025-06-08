import React from "react";
import UserInfo from "./components/UserInfo/UserInfo";
import "./App.css";

function App() {
  return (
    <div className="App">
      <h1 className="app-header">Welcome to Chess Info App</h1>
      <UserInfo />
    </div>
  );
}

export default App;
