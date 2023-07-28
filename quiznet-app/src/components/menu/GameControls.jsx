import { useContext, useEffect, useState } from "react";
import AuthContext from "../../store/auth-context";
import style from "./Menu.module.css";
import LanguageContext from "../../store/language-context";

import GameControlsButton from "./GameControlsButton";
import { startGameWithRandom } from "../../api";
import useHttp from "../../hooks/use-http";
import { useNavigate } from "react-router-dom";

const GameControls = () => {
	const [gameId, setGameId] = useState(null);

	const { dictionary } = useContext(LanguageContext);

	const { status, error, data, sendRequest } = useHttp(startGameWithRandom);

	const authContext = useContext(AuthContext);

	const navigate = useNavigate();

	const randomGameClickHandler = () => {
		sendRequest(authContext.loggedUser.id);
	};

	const playWithFriendClickHandler = () => {};

	useEffect(() => {
		if (status === "completed" && !error) {
			setGameId(data);
		}
	}, [status, error, data, setGameId]);

	useEffect(() => {
		if (gameId !== null) {
			navigate(`/game/${gameId}`);
		}
	}, [gameId, navigate]);

	return (
		<div className={style["game-controls"]}>
			<div className={style["logged-options"]}>
				<GameControlsButton
					text={dictionary.searchRandom}
					clickHandler={randomGameClickHandler}
				/>
				<GameControlsButton
					text={dictionary.playWithFriend}
					clickHandler={playWithFriendClickHandler}
				/>
			</div>
		</div>
	);
};

export default GameControls;
