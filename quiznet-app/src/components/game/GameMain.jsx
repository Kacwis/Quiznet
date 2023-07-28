import { useContext, useEffect, useState } from "react";
import style from "./Game.module.css";
import PropTypes from "prop-types";
import SecondHalf from "./SecondHalf";
import FirstHalf from "./FirstHalf";
import GameContext from "../../store/game-context";

const GameMain = () => {
	const [category, setCategory] = useState(null);
	const { setActiveRound, activeGame, activeRound } = useContext(GameContext);

	const roundNumber =
		activeGame.rounds.length === 0 ? 1 : activeGame.rounds.length + 1;

	useEffect(() => {
		if (category !== null) {
			setActiveRound({
				categoryId: category.id,
				playerAnswers: [],
				roundNumber: roundNumber,
			});
		}
	}, [category, roundNumber]);

	if (!activeRound) {
		return <FirstHalf setCategory={setCategory} category={category} />;
	}

	if (activeRound.roundNumber === 6 && activeRound.playerAnswers.length === 3) {
		return <SecondHalf />;
	}

	return (
		<>
			{activeRound.playerAnswers.length < 3 ? (
				<FirstHalf setCategory={setCategory} category={category} />
			) : (
				<SecondHalf />
			)}
		</>
	);
};

GameMain.propTypes = {
	isStarting: PropTypes.bool,
};

export default GameMain;
