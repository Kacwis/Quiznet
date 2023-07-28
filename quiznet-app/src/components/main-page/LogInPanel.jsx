import { useContext, useEffect, useRef, useState } from "react";
import CentralPanel from "../ui/CentralPanel";
import style from "./MainPage.module.css";
import LanguageContext from "../../store/language-context";
import useHttp from "../../hooks/use-http";
import { logIn } from "../../api";
import AuthContext from "../../store/auth-context";
import { useNavigate } from "react-router-dom";
import LoadingSpinner from "../ui/LoadingSpinner";

const LogInPanel = () => {
	const [logInResponse, setLogInResponse] = useState(null);

	const { dictionary } = useContext(LanguageContext);

	const authContext = useContext(AuthContext);

	const navigate = useNavigate();

	const usernameRef = useRef();
	const passwordRef = useRef();

	const { status, error, data, sendRequest } = useHttp(logIn);

	const submitHandler = (e) => {
		const username = usernameRef.current.value;
		const password = passwordRef.current.value;
		e.preventDefault();
		sendRequest({
			username,
			password,
		});
	};

	useEffect(() => {
		if (status === "completed" && !error) {
			setLogInResponse(data);
		}
	}, [status, error, data]);

	useEffect(() => {
		if (logInResponse) {
			authContext.logIn(logInResponse);
		}
	}, [logInResponse, authContext]);

	useEffect(() => {
		if (authContext.isLoggedIn) {
			navigate("/menu");
		}
	}, [authContext.isLoggedIn, navigate]);

	if (status === "pending") {
		return (
			<CentralPanel className={style["log-in-panel"]}>
				<LoadingSpinner />
			</CentralPanel>
		);
	}

	return (
		<CentralPanel className={style["log-in-panel"]}>
			<h2>{dictionary.logIn}</h2>
			{error && <p>{dictionary.wrongLoginError}</p>}
			<form onSubmit={submitHandler}>
				<div className={style["log-in-inputs"]}>
					<input
						type="text"
						placeholder={dictionary.username}
						className={style.input}
						ref={usernameRef}
					/>
					<input
						type="password"
						placeholder={dictionary.password}
						className={style.input}
						ref={passwordRef}
					/>
				</div>
				<div className={style["auth-buttons"]}>
					<button type="submit">{dictionary.logIn}</button>
					<button>{dictionary.cancel}</button>
				</div>
			</form>
		</CentralPanel>
	);
};

export default LogInPanel;
