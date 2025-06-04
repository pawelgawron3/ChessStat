import React, { useEffect, useState } from "react";
import axios from "axios";
import "./UserInfo.css";

const ChessUserInfo = () => {
  const [username, setUsername] = useState("");
  const [users, setUsers] = useState(() => {
    try {
      const stored = localStorage.getItem("users");
      const parsed = stored ? JSON.parse(stored) : [];
      return parsed;
    } catch {
      return [];
    }
  });
  const [error, setError] = useState("");

  const toEmoji = (boolValue) => (boolValue ? "✅" : "❌");

  const clearErrorMessage = () => {
    setTimeout(() => {
      setError("");
    }, 3000);
  };

  const fetchUserInfo = async () => {
    if (!username.trim()) {
      setError("Please enter a username!");
      clearErrorMessage();
      return;
    }

    try {
      const response = await axios.get(
        `https://localhost:7281/api/Chess/${username}`
      );

      const newUser = response.data;

      if (users.some((u) => u.username === newUser.username)) {
        setError("This user already exists in the table!");
        clearErrorMessage();
        return;
      }

      setUsers([...users, newUser]);
      setError("");
      setUsername("");
    } catch (ex) {
      console.error(ex);
      setError("User not found or error occurred!");
      clearErrorMessage();
    }
  };

  const RemoveUser = (username) => {
    const players = users.filter((x) => x.username !== username);
    setUsers(players);
  };

  useEffect(() => {
    localStorage.setItem("users", JSON.stringify(users));
  }, [users]);

  const handleKeyPress = (e) => {
    if (e.key === "Enter") {
      fetchUserInfo();
    }
  };

  return (
    <div className="app-main">
      <h1>Chess User Information</h1>

      <div>
        <input
          type="text"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          onKeyDown={handleKeyPress}
          placeholder="Enter username"
        />
        <button className="usernameButton" onClick={fetchUserInfo}>
          Get info
        </button>
        {error && <p style={{ color: "red" }}>{error}</p>}
      </div>

      {users.length > 0 && (
        <>
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
                  <th>Rapid rating</th>
                  <th>Blitz rating</th>
                  <th>Bullet rating</th>
                  <th>Remove</th>
                </tr>
              </thead>
              <tbody>
                {users.map((user) => (
                  <tr key={user.username}>
                    <td>
                      {user.avatar ? (
                        <img src={user.avatar} alt={user.username} />
                      ) : (
                        "❌"
                      )}
                    </td>
                    <td>{user.username}</td>
                    <td>{user.country}</td>
                    <td>{user.fide}</td>
                    <td>{user.followers}</td>
                    <td>{toEmoji(user.streamer)}</td>
                    <td>{toEmoji(user.verified)}</td>
                    <td>{user.rapid.last.rating}</td>
                    <td>{user.blitz.last.rating}</td>
                    <td>{user.bullet.last.rating}</td>
                    <td>
                      <button onClick={() => RemoveUser(user.username)}>
                        <span className="knight">&#9816;</span>
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </>
      )}
    </div>
  );
};

export default ChessUserInfo;
