import { useContext } from "react";
import style from "./Menu.module.css";
import AuthContext from "../../store/auth-context";

import PropTypes from "prop-types";
import {
	getCurrentTurn,
	getPlayersScoreInGame,
} from "../../constants/Constants";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faClock } from "@fortawesome/free-regular-svg-icons";
import { faPlay } from "@fortawesome/free-solid-svg-icons";
import LanguageContext from "../../store/language-context";

const GameListItem = ({ game }) => {
	const { loggedUser } = useContext(AuthContext);

	const { dictionary } = useContext(LanguageContext);

	const opponentPlayer = game.players.find((p) => p.id !== loggedUser.id);

	const { playerScore, opponentScore } = getPlayersScoreInGame(
		game,
		loggedUser
	);

	const isStarting = game.startingPlayerId === loggedUser.id;

	const isPlayerTurn = getCurrentTurn(
		game.rounds[game.rounds.length - 1],
		isStarting
	);

	return (
		<div className={style["game-list-item"]}>
			<div className={style.opponent}>
				<p>{dictionary.you}</p>
				<p>vs</p>
				<p>{opponentPlayer.user.username}</p>
			</div>
			<div className={style["game-score"]}>
				<p>{playerScore}</p>
				<p>:</p>
				<p>{opponentScore}</p>
			</div>
			{game.status === "IN_PROGRESS" && (
				<div className={style["whose-turn"]}>
					{isPlayerTurn ? (
						<FontAwesomeIcon
							icon={faPlay}
							size="xl"
							style={{ color: "var(--dark-pink-color" }}
						/>
					) : (
						<FontAwesomeIcon icon={faClock} size="xl" />
					)}
				</div>
			)}
		</div>
	);
};

GameListItem.propTypes = {
	game: PropTypes.object,
};

export default GameListItem;
