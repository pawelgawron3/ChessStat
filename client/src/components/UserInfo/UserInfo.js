import React, { useState } from "react";
import axios from "axios";
import "./UserInfo.css";

const ChessUserInfo = () => {
  const [username, setUsername] = useState("");
  const [users, setUsers] = useState([]);
  const [error, setError] = useState("");

  const toEmoji = (boolValue) => (boolValue ? "✅" : "❌");

  const fetchUserInfo = async () => {
    try {
      const response = await axios.get(
        `https://localhost:7281/api/Chess/${username}`
      );

      const newUser = response.data;

      setUsers((prevUsers) => {
        const exists = prevUsers.some((u) => u.username === newUser.username);
        if (!exists) {
          return [...prevUsers, newUser];
        } else {
          setError("This user already exists in the table!");
          return prevUsers;
        }
      });

      setError("");
      setUsername("");
    } catch (ex) {
      console.error(ex);
      setError("User not found or error occurred!");
    }
  };
  const RemoveUser = (username)=>{
    const players=users.filter(x=>x.username!==username);
    setUsers(players)
  }
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

      {users && (
        <div>
          <div className="userInfoHeader">
            <h1>User Info:</h1>
          </div>
          <div className="table-container">
            <table>
              <thead>
                <tr>
                  <th>Avatar</th>
                  <th>Username</th>
                  <th>Country</th>
                  <th>FIDE</th>
                  <th>Followers</th>
                  <th>Streamer</th>
                  <th>Verified</th>
                  <th>Rapid current rating</th>
                  <th>Remove</th>
                </tr>
              </thead>
              <tbody>
                {users.map((user) => (
                  <tr key={user.username}>
                    <td>
                      <img src={user.avatar} alt={user.username} width="50" />
                    </td>
                    <td>{user.username}</td>
                    <td>{user.country}</td>
                    <td>{user.fide}</td>
                    <td>{user.followers}</td>
                    <td>{toEmoji(user.streamer)}</td>
                    <td>{toEmoji(user.verified)}</td>
                    <td>{user.rapid.last.rating}</td>
                    <td>
                      <button onClick={()=>RemoveUser(user.username)}>X</button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};

export default ChessUserInfo;
