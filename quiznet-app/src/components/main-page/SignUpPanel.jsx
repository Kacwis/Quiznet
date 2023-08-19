import { useContext, useEffect, useRef, useState } from "react";
import style from "./MainPage.module.css";
import useHttp from "../../hooks/use-http";
import CentralPanel from "../ui/CentralPanel";
import { useNavigate } from "react-router-dom";
import { signUp } from "../../api";
import LanguageContext from "../../store/language-context";

const SignUpPanel = () => {
	const [arePasswordsValid, setArePasswordsValid] = useState(true);
	const [areInputsEmpty, setAreInputsEmpty] = useState(false);

	const { dictionary } = useContext(LanguageContext);

	const usernameRef = useRef();
	const emailRef = useRef();
	const passwordRef = useRef();
	const rePasswordRef = useRef();

	const navigate = useNavigate();

	const { status, error, data, sendRequest } = useHttp(signUp);

	const submitHandler = (e) => {
		e.preventDefault();
		setArePasswordsValid(true);
		setAreInputsEmpty(false);
		if (!validateData()) {
			return;
		}
		const username = usernameRef.current.value;
		const email = emailRef.current.value;
		const password = passwordRef.current.value;
		const requestData = {
			username,
			email,
			password,
		};
		sendRequest(requestData);
	};

	useEffect(() => {
		if (status === "completed" && !error) {
			if (data.isSuccess) {
				navigate("/log-in");
			}
		}
	}, [status, error, data, navigate]);

	const validateData = () => {
		const username = usernameRef.current.value;
		const email = emailRef.current.value;
		const password = passwordRef.current.value;
		const rePassword = rePasswordRef.current.value;
		if (username.trim(" ").length === 0 || email.trim(" ").length === 0) {
			setAreInputsEmpty(true);
			return false;
		}
		if (password !== rePassword) {
			setArePasswordsValid(false);
			return false;
		}
		return true;
	};

	return (
		<CentralPanel className={style["sign-up-panel"]}>
			<h2>{dictionary.signUp}</h2>
			<form onSubmit={submitHandler}>
				{areInputsEmpty && <p>{dictionary.usernameEmailBlankError}</p>}
				<input
					type={"text"}
					ref={usernameRef}
					className={style["input"]}
					placeholder={dictionary.username}
				/>
				<input
					type={"text"}
					ref={emailRef}
					className={style["input"]}
					placeholder={"Email"}
				/>
				{!arePasswordsValid && <p>{dictionary.passwordsError}</p>}
				<input
					type={"password"}
					ref={passwordRef}
					className={style["input"]}
					placeholder={dictionary.password}
				/>
				<input
					type={"password"}
					ref={rePasswordRef}
					className={style["input"]}
					placeholder={dictionary.rePassword}
				/>

				<div className={style["auth-buttons"]}>
					<button type={"submit"}>{dictionary.signUp}</button>
					<button type={"reset"} onClick={() => navigate("/")}>
						{dictionary.cancel}
					</button>
				</div>
			</form>
		</CentralPanel>
	);
};

export default SignUpPanel;
