import { useContext, useEffect, useState } from "react";
import style from "./Menu.module.css";
import AuthContext from "../../store/auth-context";
import CentralPanel from "../ui/CentralPanel";
import GameControls from "./GameControls";
import AccountInfo from "./AccountInfo";
import GamesList from "./GamesList";
import GameContext from "../../store/game-context";
import FriendsRanking from "./FriendsRanking";
import useHttp from "../../hooks/use-http";
import { getMenuData } from "../../api";
import LoadingSpinner from "../ui/LoadingSpinner";

const Menu = () => {
	const [menuData, setMenuData] = useState({
		activeGames: [],
		finishedGames: [],
	});

	const authContext = useContext(AuthContext);
	const { setActiveRound } = useContext(GameContext);

	const { status, error, data, sendRequest } = useHttp(getMenuData, true);

	useEffect(() => {
		if (status === "completed" && !error) {
			console.log(data);
			setMenuData(data);
		}
	}, [status, error, data, setMenuData]);

	useEffect(() => {
		sendRequest({
			playerId: authContext.loggedUser.id,
			token: authContext.token,
		});
		setActiveRound(null);
	}, []);

	if (status === "pending") {
		return <LoadingSpinner />;
	}

	if (error) {
		return <h1>{error}</h1>;
	}

	return (
		<CentralPanel className={style.menu}>
			<div className={style["controls-account"]}>
				{authContext.isLoggedIn && <AccountInfo />}
				<GameControls />
			</div>
			<div className={style["ranking-games"]}>
				<GamesList title={"Active games"} games={menuData.activeGames} />
				<FriendsRanking />
				<GamesList title={"Finished games"} games={menuData.finishedGames} />
			</div>
		</CentralPanel>
	);
};

export default Menu;
