import React, { useState } from "react";
import axios from "axios";
import "./UserInfo.css";

const ChessUserInfo = () => {
  const [username, setUsername] = useState("");
  const [userInfo, setUserInfo] = useState(null);
  const [error, setError] = useState("");

  const fetchUserInfo = async () => {
    try {
      const response = await axios.get(
        `https://localhost:7281/api/Chess/${username}`
      );
      setUserInfo(response.data);
      setError("");
    } catch (ex) {
      console.error(ex);
      setError("User not found or error occurred!");
      setUserInfo(null);
    }
  };

  return (
    <div className="temporary">
      <h1>Chess User Information</h1>
      <input
        type="text"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        placeholder="Enter username"
      />
      <button className="usernameButton" onClick={fetchUserInfo}>
        Get info
      </button>

      {error && <p style={{ color: "red" }}>{error}</p>}

      {userInfo && (
        <div>
          <div className="userInfoHeader">
            <h1>User Info:</h1>
          </div>
          <div className="table-container">
            <table>
              <thead>
                <tr>
                  <th>Image</th>
                  <th>Username</th>
                  <th>Country</th>
                  <th>FIDE</th>
                  <th>Followers</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>
                    <img
                      src={userInfo.avatar}
                      alt={userInfo.username}
                      width="50"
                    />
                  </td>
                  <td>{userInfo.username}</td>
                  <td>{userInfo.country}</td>
                  <td>{userInfo.fide}</td>
                  <td>{userInfo.followers}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};

export default ChessUserInfo;
