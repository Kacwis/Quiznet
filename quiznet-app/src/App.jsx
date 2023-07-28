import { Route, Routes } from "react-router-dom";
import "./App.css";
import Layout from "./components/ui/Layout";
import StartPanel from "./components/main-page/StartPanel";
import LogInPanel from "./components/main-page/LogInPanel";
import Menu from "./components/menu/Menu";
import GameSessionMain from "./components/game-session/GameSessionMain";

function App() {
	return (
		<Layout>
			<Routes>
				<Route path="/" element={<StartPanel />} />
				<Route path="log-in" element={<LogInPanel />} />
				{/* <Route path="sign-up" element={<AuthenticationPanel />} /> */}
				<Route path="menu" element={<Menu />} />

				<Route path="game/:gameId" element={<GameSessionMain />} />
			</Routes>
		</Layout>
	);
}

export default App;
