import style from "./GameSession.module.css";
import PropTypes from "prop-types";
import PlayerAnswerDisplay from "./PlayerAnswerDisplay";
import { useState } from "react";

const IndividualScore = ({ answers, areAnswersBlocked }) => {
	const [isAnswerDisplayVisible, setIsAnswerDisplayVisible] = useState(false);
	const [currentAnswer, setCurrentAnswer] = useState(null);

	const answerClickHandler = (answer) => {
		setIsAnswerDisplayVisible(true);
		setCurrentAnswer(answer);
	};

	const hidePlayerAnswerDisplay = () => {
		setIsAnswerDisplayVisible(false);
		setCurrentAnswer(null);
	};

	const questionMarkersContent = answers.map((answer, index) => {
		let className = style["question-marker"];
		if (areAnswersBlocked) {
			className = `${className} ${style.blocked}`;
		} else {
			answer.isCorrect
				? (className = `${className} ${style.correct}`)
				: (className = `${className} ${style.incorrect}`);
		}

		return (
			<div
				key={index}
				className={className}
				onClick={() => answerClickHandler(answer)}
			/>
		);
	});

	return (
		<>
			<div className={style["individual-score"]}>{questionMarkersContent}</div>
			{isAnswerDisplayVisible && (
				<PlayerAnswerDisplay
					currentAnswer={currentAnswer}
					hide={hidePlayerAnswerDisplay}
				/>
			)}
		</>
	);
};

IndividualScore.propTypes = {
	answers: PropTypes.array,
	areAnswersBlocked: PropTypes.bool,
};

export default IndividualScore;
