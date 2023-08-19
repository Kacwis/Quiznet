import style from "./GameSession.module.css";

import PropTypes from "prop-types";
import IndividualScore from "./IndividualScore";
import { useContext } from "react";
import LanguageContext from "../../store/language-context";
import GameContext from "../../store/game-context";
import AuthContext from "../../store/auth-context";
import { getCurrentTurn } from "../../constants/Constants";

const GameSessionRound = ({ gameRound }) => {
	const { isStarting, startRound, activeRound, activeGame } =
		useContext(GameContext);

	const { activeLang, dictionary } = useContext(LanguageContext);
	const { loggedUser } = useContext(AuthContext);

	const isPlayerTurn = getCurrentTurn(activeRound, isStarting);

	const loggedPlayersAnswers = [];
	const opponentAnswers = [];
	gameRound.playerAnswers.forEach((a) => {
		if (a.player.id === loggedUser.id) {
			loggedPlayersAnswers.push(a);
		} else {
			opponentAnswers.push(a);
		}
	});

	let opponentScoreContent;
	let loggedPlayerScoreContent;

	const playClickHandler = () => {
		startRound();
	};

	if (
		gameRound.playerAnswers.length === 6 ||
		activeGame.status === "FINISHED"
	) {
		opponentScoreContent = <IndividualScore answers={opponentAnswers} />;
		loggedPlayerScoreContent = (
			<IndividualScore answers={loggedPlayersAnswers} />
		);
	} else {
		loggedPlayerScoreContent = isPlayerTurn ? (
			<div className={style["play-or-wait-container"]}>
				<button onClick={playClickHandler} className={style["play-btn"]}>
					{dictionary.play}
				</button>
			</div>
		) : (
			<IndividualScore answers={loggedPlayersAnswers} />
		);

		opponentScoreContent = !isPlayerTurn ? (
			<div className={style["play-or-wait-container"]}>
				<p>{dictionary.waitForOpponent}</p>
			</div>
		) : (
			<IndividualScore answers={opponentAnswers} areAnswersBlocked={true} />
		);
	}

	return (
		<div className={style["game-round"]}>
			<div className={style["round-upper-container"]}>
				<p>
					{activeLang === "pl"
						? gameRound.category.namePl
						: gameRound.category.name}
				</p>
			</div>
			<div className={style["round-down-container"]}>
				{loggedPlayerScoreContent}
				{opponentScoreContent}
			</div>
		</div>
	);
};

GameSessionRound.propTypes = {
	gameRound: PropTypes.object,
};

export default GameSessionRound;
