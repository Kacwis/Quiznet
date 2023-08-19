import { useContext } from "react";
import style from "./GameSession.module.css";

import PropTypes from "prop-types";
import AuthContext from "../../store/auth-context";
import GameContext from "../../store/game-context";
import { getPlayersScoreInGame } from "../../constants/Constants";

const PlayersInfo = ({ players }) => {
	const authContext = useContext(AuthContext);

	const { activeGame } = useContext(GameContext);

	console.log(activeGame);

	const player = players.find((p) => +p.id === +authContext.loggedUser.id);
	const opponent = players.find((p) => +p.id !== +authContext.loggedUser.id);

	const { playerScore, opponentScore } = getPlayersScoreInGame(
		activeGame,
		player
	);

	return (
		<div className={style["players-info"]}>
			<div className={style["player-info"]}>
				<h3>{player.user.username}</h3>
				<p>{playerScore}</p>
			</div>
			<div className={style["player-info"]}>
				<h3>{opponent.user.username}</h3>
				<p>{opponentScore}</p>
			</div>
		</div>
	);
};

PlayersInfo.propTypes = {
	players: PropTypes.array,
};

export default PlayersInfo;
