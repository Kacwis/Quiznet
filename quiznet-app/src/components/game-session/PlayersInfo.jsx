import { useContext } from "react";
import style from "./GameSession.module.css";

import PropTypes from "prop-types";
import AuthContext from "../../store/auth-context";

const PlayersInfo = ({ players }) => {
	const authContext = useContext(AuthContext);

	const player = players.find((p) => +p.id === +authContext.loggedUser.id);
	const opponent = players.find((p) => +p.id !== +authContext.loggedUser.id);

	return (
		<div className={style["players-info"]}>
			<div className={style["player-info"]}>
				<h3>{player.user.username}</h3>
				<p>{player.score}</p>
			</div>
			<div className={style["player-info"]}>
				<h3>{opponent.user.username}</h3>
				<p>{opponent.score}</p>
			</div>
		</div>
	);
};

PlayersInfo.propTypes = {
	players: PropTypes.array,
};

export default PlayersInfo;
