import { useContext, useState } from "react";
import style from "./Game.module.css";
import Question from "./Question";

import PropTypes from "prop-types";
import GameContext from "../../store/game-context";

const Questions = ({ questions, saveAnswers }) => {
	const [questionIndex, setQuestionIndex] = useState(0);

	const gameContext = useContext(GameContext);

	const nextQuestion = (previousQuestionAnswer) => {
		gameContext.activeRound.playerAnswers.push(previousQuestionAnswer);
		
		const currentQuestionCounter = questionIndex + 1;
		if (currentQuestionCounter > 2) {
			saveAnswers();
			return;
		}
		setQuestionIndex(currentQuestionCounter);
	};

	return (
		<div className={style.questions}>
			<Question
				currentQuestion={questions[questionIndex]}
				nextQuestion={nextQuestion}
				questionCounter={questionIndex}
			/>
		</div>
	);
};

Questions.propTypes = {
	questions: PropTypes.array,
	isBackToMenu: PropTypes.bool,
	saveAnswers: PropTypes.func,
};

export default Questions;
