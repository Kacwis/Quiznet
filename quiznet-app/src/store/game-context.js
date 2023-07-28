import React from "react";

const GameContext = React.createContext({
	activeGame: null,
	activeRound: null,
	roundNumber: 1,
	isStarting: true,
	isRoundInPlay: false,
	setActiveRound: () => {},
	startRound: () => {},
	stopRound: () => {},
	savePlayedRound: () => {},
	setGameById: () => {},
});

export default GameContext;
