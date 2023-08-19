import { useEffect, useRef, useState } from "react";
import style from "./Settings.module.css";
import EditButton from "./EditButton";

const Password = () => {
	const [isPasswordEditVisible, setIsPasswordEditVisible] = useState(false);
	const [newPassword, setNewPassword] = useState("");
	const [newRePassword, setNewRePassword] = useState("");
	const [arePasswordsEmpty, setArePasswordsEmpty] = useState(true);

	const passwordRef = useRef();
	const rePasswordRef = useRef();

	const passwordInputChangeHandler = () => {
		if (passwordRef.current) setNewPassword(passwordRef.current.value);
	};

	const rePasswordInputChangeHandler = () => {
		if (rePasswordRef.current) setNewRePassword(rePasswordRef.current.value);
	};

	const applyClickHandler = () => {};

	useEffect(() => {
		console.log(newPassword, newRePassword);
		if (newPassword.length > 0 && newRePassword.length > 0) {
			setArePasswordsEmpty(false);
		} else {
			setArePasswordsEmpty(true);
		}
	}, [newPassword, newRePassword]);

	return (
		<>
			<div className={style.password}>
				<label>**********</label>
				<EditButton
					clickHandler={() => setIsPasswordEditVisible(!isPasswordEditVisible)}
				/>
			</div>
			{isPasswordEditVisible && (
				<div className={style["password-edit"]}>
					<input
						type={"password"}
						ref={passwordRef}
						placeholder={"New password"}
						onChange={passwordInputChangeHandler}
					/>
					<input
						type={"password"}
						ref={rePasswordRef}
						placeholder={"Retype new password"}
						onChange={rePasswordInputChangeHandler}
					/>
					{newPassword !== newRePassword && <p>Passwords are not the same!</p>}
					{!arePasswordsEmpty && newPassword === newRePassword && (
						<button className={style["apply-btn"]} onClick={applyClickHandler}>
							Apply
						</button>
					)}
				</div>
			)}
		</>
	);
};

export default Password;
