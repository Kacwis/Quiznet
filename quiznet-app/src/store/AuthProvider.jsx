import { useEffect, useState } from "react";
import AuthContext from "./auth-context";

import PropTypes from "prop-types";
import { getMenuData } from "../api";

import Cookies from "js-cookie";
import useHttp from "../hooks/use-http";

const MENU_REFRESH_TIME = 1000 * 30;
const DEFAULT_MENU_DATA_STATE = {
	activeGames: [],
	finishedGames: [],
	friends: [],
	isNewMessages: false,
};

const COOKIE_ITEM_NAME = "quiznet-user-data";

const AuthProvider = ({ children }) => {
	const [loggedUser, setLoggedUser] = useState(null);
	const [menuData, setMenuData] = useState(DEFAULT_MENU_DATA_STATE);
	const [isLoggedIn, setIsLoggedIn] = useState(false);
	const [token, setToken] = useState(null);

	const { status, error, data, sendRequest } = useHttp(getMenuData);

	const sendMenuRequest = () => {
		sendRequest({ id: loggedUser.id, token: token });
	};

	useEffect(() => {
		if (status === "completed" && !error) {
			if (data.token) {
				setToken(data.token);
			}
			configureMenuData(data.menuData);
		}
	}, [status, error, data]);

	const configureMenuData = (menuData) => {
		const currentMenuData = menuData;
		currentMenuData.friends.push(loggedUser);
		currentMenuData.friends = currentMenuData.friends.sort(
			(a, b) => +b.score - +a.score
		);
		updateUser(menuData.player);
		setMenuData(currentMenuData);
	};

	const updateUser = (user) => {
		setLoggedUser(user);
		addUserToCookies(user);
	};

	const addUserToCookies = (user) => {
		var userData = { loggedUser: user, token };
		Cookies.set(COOKIE_ITEM_NAME, JSON.stringify(userData), { expires: 1 });
	};

	useEffect(() => {
		let interval;
		if (loggedUser) {
			interval = setInterval(() => {
				sendRequest({ id: loggedUser.id, token: token });
			}, [MENU_REFRESH_TIME]);
		}
		return () => clearInterval(interval);
	}, [loggedUser]);

	const logInHandler = (logInInfo) => {
		setLoggedUser(logInInfo.user);
		setIsLoggedIn(true);
		setToken(logInInfo.token);
		addUserToCookies(logInInfo.user);
	};

	const logOutHandler = () => {
		setLoggedUser(null);
		setIsLoggedIn(false);
		setToken(null);
		setMenuData(DEFAULT_MENU_DATA_STATE);
		Cookies.remove(COOKIE_ITEM_NAME);
	};

	const setUserDataHandler = (userData) => {
		setLoggedUser(userData.loggedUser);
		setToken(userData.token);
		setIsLoggedIn(true);
	};


	const context = {
		loggedUser,
		isLoggedIn,
		token,
		menuData,
		logIn: logInHandler,
		logOut: logOutHandler,
		setUserData: setUserDataHandler,
		getMenuData: sendMenuRequest,
	};

	return (
		<AuthContext.Provider value={context}>{children}</AuthContext.Provider>
	);
};

AuthProvider.propTypes = {
	children: PropTypes.node,
};

export default AuthProvider;
