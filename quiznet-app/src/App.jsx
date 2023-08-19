import { Route, Routes, useNavigate } from "react-router-dom";
import "./App.css";
import Layout from "./components/ui/Layout";
import StartPanel from "./components/main-page/StartPanel";
import LogInPanel from "./components/main-page/LogInPanel";
import Menu from "./components/menu/Menu";
import GameSessionMain from "./components/game-session/GameSessionMain";
import { useContext, useEffect } from "react";
import AuthContext from "./store/auth-context";
import GuestPanel from "./components/main-page/GuestPanel";
import Cookies from "js-cookie";
import SignUpPanel from "./components/main-page/SignUpPanel";
import Settings from "./components/account-settings/Settings";

function App() {
	const { setUserData } = useContext(AuthContext);

	const navigate = useNavigate();

	useEffect(() => {
		const userData = Cookies.get("quiznet-user-data");
		if (userData) {
			setUserData(JSON.parse(userData));
			navigate("/menu");
		}
	}, []);

	return (
		<Layout>
			<Routes>
				<Route path="/" element={<StartPanel />} />
				<Route path="log-in" element={<LogInPanel />} />
				<Route path="/guest" element={<GuestPanel />} />
				<Route path="/sign-up" element={<SignUpPanel />} />
				<Route path="menu" element={<Menu />} />
				<Route path="game/:gameId" element={<GameSessionMain />} />
			</Routes>
		</Layout>
	);
}

export default App;
