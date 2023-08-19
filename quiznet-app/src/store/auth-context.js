import React from "react";

const AuthContext = React.createContext({
	isLoggedIn: false,
	loggedUser: null,
	token: null,
	menuData: { activeGames: [], finishedGames: [], friends: [] },
	logIn: () => {},
	logOut: () => {},
	setUserData: () => {},
	getMenuData: () => {},
});

export default AuthContext;
