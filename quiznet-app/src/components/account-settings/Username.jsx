import { useContext, useState, useRef, useEffect } from "react";
import style from "./Settings.module.css";
import AuthContext from "../../store/auth-context";
import EditButton from "./EditButton";
import useHttp from "../../hooks/use-http";
import { getMenuData, updateUsername } from "../../api";

const Username = () => {
	const [isUsernameEditVisible, setIsUsernameEditVisible] = useState(false);
	const [newUsername, setNewUsername] = useState("");
	const [isUsernameUsed, setIsUsernameUsed] = useState(false);

	const { loggedUser, token, getMenuData } = useContext(AuthContext);

	const usernameRef = useRef();

	const { status, error, data, sendRequest } = useHttp(updateUsername);

	const inputChangeHandler = () => {
		setNewUsername(usernameRef.current.value);
	};

	const applyClickHandler = () => {
		setIsUsernameUsed(false);
		const requestData = {
			token,
			body: {
				playerId: loggedUser.id,
				username: newUsername,
			},
		};
		sendRequest(requestData);
	};

	const resetEditPanel = () => {
		setNewUsername("");
		usernameRef.current.value = "";
	};

	useEffect(() => {
		if (status === "completed" && !error) {
			if (!data.isSuccess) {
				setIsUsernameUsed(true);
			} else {
				getMenuData();
			}
		}
	}, [status, data, error]);

	useEffect(() => {
		if (isUsernameUsed) {
			resetEditPanel();
		}
	}, [isUsernameUsed]);

	return (
		<>
			<div className={style.username}>
				<h3>{loggedUser.username}</h3>
				<EditButton
					clickHandler={() => setIsUsernameEditVisible(!isUsernameEditVisible)}
				/>
			</div>
			{isUsernameEditVisible && (
				<div className={style["username-edit"]}>
					<input
						type={"text"}
						placeholder={"new username"}
						ref={usernameRef}
						onChange={inputChangeHandler}
					/>
					{isUsernameUsed && (
						<p style={{ color: "red" }}>Username already exists. Try other</p>
					)}
					{usernameRef.current &&
						usernameRef.current.value.trim().length !== 0 && (
							<button
								onClick={applyClickHandler}
								className={style["apply-btn"]}
							>
								Apply
							</button>
						)}
				</div>
			)}
		</>
	);
};

export default Username;
