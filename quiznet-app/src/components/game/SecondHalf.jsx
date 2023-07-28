import { useContext } from "react";
import style from "./Game.module.css";
import GameContext from "../../store/game-context";
import Questions from "./Questions";
import { useNavigate } from "react-router-dom";

const SecondHalf = () => {
	const { activeRound, savePlayedRound, stopRound, activeGame } =
		useContext(GameContext);

	const navigate = useNavigate();

	let questions = [];

	if (activeRound) {
		activeRound.playerAnswers.forEach((answer) => {
			questions.push(answer.question);
		});
		questions = questions.sort((a, b) => {
			return a.answerNumber > b.answerNumber;
		});
	}

	const saveAnswers = () => {
		console.log("saving second");
		console.log(activeRound, activeGame);
		savePlayedRound();
		if (activeGame.rounds.length === 5) {
			stopRound();
			return;
		}
		activeRound.playerAnswers = [];
	};

	return (
		<div className={style["round-half"]}>
			<Questions questions={questions} saveAnswers={saveAnswers} />
		</div>
	);
};

export default SecondHalf;
