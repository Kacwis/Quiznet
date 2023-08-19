import { useContext, useEffect, useState } from "react";
import style from "./Menu.module.css";
import AuthContext from "../../store/auth-context";
import CentralPanel from "../ui/CentralPanel";
import GameControls from "./GameControls";
import GamesList from "./GamesList";
import GameContext from "../../store/game-context";
import Ranking from "./Ranking";
import AccountControls from "./AccountControls";
import Inbox from "../inbox/Inbox";
import Cookies from "js-cookie";
import LanguageContext from "../../store/language-context";

const Menu = () => {
	const [isInboxVisible, setIsInboxVisible] = useState(false);
	const [chatPlayerId, setChatPlayerId] = useState(null);

	const { menuData, getMenuData, setUserData } = useContext(AuthContext);
	const { setActiveRound } = useContext(GameContext);
	const { dictionary } = useContext(LanguageContext);

	useEffect(() => {
		setActiveRound(null);
		getMenuData();
		const userData = Cookies.get("quiznet-user-data");
		if (userData) {
			setUserData(JSON.parse(userData));
		}
	}, []);

	const openInboxHandler = (chatPlayerId) => {
		setChatPlayerId(null);
		if (typeof chatPlayerId === "number") {
			setChatPlayerId(chatPlayerId);
			setIsInboxVisible(true);
			return;
		}
		setIsInboxVisible(true);
	};

	return (
		<>
			<CentralPanel className={style.menu}>
				<div className={style["controls-account"]}>
					<AccountControls openInbox={openInboxHandler} />
					<GameControls />
				</div>
				<div className={style["ranking-games"]}>
					<GamesList
						title={dictionary.activeGames}
						games={menuData.activeGames}
					/>
					<Ranking openInbox={openInboxHandler} />
					<GamesList
						title={dictionary.finishedGames}
						games={menuData.finishedGames}
					/>
				</div>
			</CentralPanel>
			{isInboxVisible && (
				<Inbox
					closeInbox={() => {
						setIsInboxVisible(false);
					}}
					chatPlayerId={chatPlayerId}
				/>
			)}
		</>
	);
};

export default Menu;
