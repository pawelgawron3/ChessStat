import React from "react";
import UserInfo from "./components/UserInfo/UserInfo";
import "./App.css";

function ChessFigures() {
  return <span className="title-figures">&#9813;&#9812;</span>;
}

function App() {
  return (
    <div className="App">
      <h1 className="app-header">
        <ChessFigures />
        Welcome to Chess Info App
        <ChessFigures />
      </h1>
      <UserInfo />
    </div>
  );
}

export default App;
