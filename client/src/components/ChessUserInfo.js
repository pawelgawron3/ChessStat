import React, { useState } from "react";
import axios from "axios";

const ChessUserInfo = () => {
  const [username, setUsername] = useState("");
  const [userInfo, setUserInfo] = useState(null);
  const [error, setError] = useState("");

  const fetchUserInfo = async () => {
    try {
      const response = await axios.get(
        `https://localhost:7281/api/Chess/${username}`
      );
      console.log(response);
      setUserInfo(response.data);
      setError("");
    } catch (ex) {
      console.error(ex);
      setError("User not found or error occurred!");
      setUserInfo(null);
    }
  };

  return (
    <div>
      <h1>Chess User Information</h1>
      <input
        type="text"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        placeholder="Enter username"
      />
      <button onClick={fetchUserInfo}>Get info</button>

      {error && <p style={{ color: "red" }}>{error}</p>}

      {userInfo && (
        <div>
          <h2>User Info:</h2>
          <p>
            <strong>Chess.com account:</strong> {userInfo.url}
          </p>
          <p>
            <strong>Username:</strong> {userInfo.username}
          </p>
          <p>
            <strong>Country:</strong> {userInfo.country}
          </p>
          <p>
            <strong>Followers:</strong> {userInfo.followers}
          </p>
          <img src={userInfo.avatar} alt={userInfo.username} width="100" />
        </div>
      )}
    </div>
  );
};

export default ChessUserInfo;
