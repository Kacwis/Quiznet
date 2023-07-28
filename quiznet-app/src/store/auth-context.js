import React from "react";

const AuthContext = React.createContext({
	isLoggedIn: false,
	loggedUser: null, // guest / username
	token: null,
	logIn: (logInInfo) => {},
	logOut: () => {},
});

export default AuthContext;
