import style from "./GameSession.module.css";

import PropTypes from "prop-types";
import IndividualScore from "./IndividualScore";
import { useContext } from "react";
import LanguageContext from "../../store/language-context";
import GameContext from "../../store/game-context";
import AuthContext from "../../store/auth-context";

const GameSessionRound = ({ gameRound }) => {
	const { isStarting, startRound, activeRound, activeGame } =
		useContext(GameContext);
	const { activeLang } = useContext(LanguageContext);
	const { loggedUser } = useContext(AuthContext);

	const isAnswersEmpty = activeRound && activeRound.playerAnswers.length === 0;

	const isPlayerTurn =
		activeRound && activeRound.roundNumber % 2 === 1
			? isStarting ^ !isAnswersEmpty
			: !(isStarting ^ !isAnswersEmpty);

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
					Play
				</button>
			</div>
		) : (
			<IndividualScore answers={loggedPlayersAnswers} />
		);

		opponentScoreContent = !isPlayerTurn ? (
			<div className={style["play-or-wait-container"]}>
				<p>Wait for opponent</p>
			</div>
		) : (
			<IndividualScore answers={opponentAnswers} />
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
