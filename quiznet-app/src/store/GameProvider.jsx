import { useContext, useEffect, useState } from "react";
import GameContext from "./game-context";

import PropTypes from "prop-types";
import AuthContext from "./auth-context";
import useHttp from "../hooks/use-http";
import { getGameById, savePlayedRound } from "../api";

const GameProvider = ({ children }) => {
	const [activeGame, setActiveGame] = useState(null);
	const [activeRound, setAcitveRound] = useState(null);
	const [isRoundInPlay, setIsRoundInPlay] = useState(false);

	const authContext = useContext(AuthContext);

	const {
		status: saveRoundStatus,
		error: saveRoundError,
		data: saveRoundData,
		sendRequest: saveRoundSendRequest,
	} = useHttp(savePlayedRound);

	useEffect(() => {
		if (saveRoundStatus === "completed" && saveRoundError) {
			setActiveGame(saveRoundData);
		}
	}, [saveRoundData, saveRoundSendRequest, saveRoundStatus, saveRoundError]);

	const {
		status: getGameStatus,
		error: getGameError,
		data: getGameData,
		sendRequest: getGameSendRequest,
	} = useHttp(getGameById);

	useEffect(() => {
		if (getGameStatus === "completed" && !getGameError) {
			setActiveGame(getGameData);
			if (!getGameData.activeRound) return;
			getGameData.activeRound.playerAnswers =
				getGameData.activeRound.playerAnswers.sort((a, b) => {
					return a.answerNumber < b.answerNumber;
				});
			setAcitveRound(getGameData.activeRound);
		}
	}, [getGameError, getGameStatus, getGameData]);

	const startRoundHandler = () => {
		setIsRoundInPlay(true);
	};

	const stopRoundHandler = () => {
		setIsRoundInPlay(false);
		getGameSendRequest(activeGame.id);
	};

	const savePlayedRoundHandler = () => {
		if (activeRound !== null) {
			if (activeRound.playerAnswers.length === 6) {
				saveRoundSendRequest({
					playerAnswers: activeRound.playerAnswers.slice(3),
					roundId: activeRound.id,
				});
				activeRound.playerAnswers = [];
				return;
			}
			saveRoundSendRequest({
				round: activeRound,
				gameId: activeGame.id,
			});
		}
	};

	const setActiveRoundHandler = (round) => {
		setAcitveRound(round);
	};

	const setGameByIdHandler = (gameId) => {
		getGameSendRequest(gameId);
	};

	const context = {
		activeGame,
		activeRound,
		isStarting:
			activeGame !== null &&
			authContext.loggedUser &&
			activeGame.startingPlayerId === authContext.loggedUser.id,
		isRoundInPlay,
		setActiveRound: setActiveRoundHandler,
		setGameById: setGameByIdHandler,
		startRound: startRoundHandler,
		stopRound: stopRoundHandler,
		savePlayedRound: savePlayedRoundHandler,
	};

	return (
		<GameContext.Provider value={context}>{children}</GameContext.Provider>
	);
};

GameProvider.propTypes = {
	children: PropTypes.node,
};

export default GameProvider;
