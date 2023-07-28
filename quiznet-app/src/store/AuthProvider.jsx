import { useState } from "react";
import AuthContext from "./auth-context";

import PropTypes from "prop-types";

const AuthProvider = ({ children }) => {
	const [loggedUser, setLoggedUser] = useState(null);
	const [isLoggedIn, setIsLoggedIn] = useState(false);
	const [token, setToken] = useState(null);

	const logInHandler = (logInInfo) => {
		setLoggedUser(logInInfo.user);
		setIsLoggedIn(true);
		setToken(logInInfo.token);
	};

	const logOutHandler = () => {
		setLoggedUser(null);
		setIsLoggedIn(false);
		setToken(null);
	};

	const context = {
		loggedUser,
		isLoggedIn,
		token,
		logIn: logInHandler,
		logOut: logOutHandler,
	};

	return (
		<AuthContext.Provider value={context}>{children}</AuthContext.Provider>
	);
};

AuthProvider.propTypes = {
	children: PropTypes.node,
};

export default AuthProvider;
