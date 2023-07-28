import { useCallback, useContext, useEffect, useRef, useState } from "react";
import style from "./Game.module.css";

import PropTypes from "prop-types";
import LanguageContext from "../../store/language-context";
import AuthContext from "../../store/auth-context";
import GameContext from "../../store/game-context";
import Answers from "./Answers";

const CORRECT_ANSWERS_VIEW_TIME = 2000;
const TIME_FOR_ANSWER = 10000;

const Question = ({ currentQuestion, nextQuestion, questionCounter }) => {	
	const [question, setQuestion] = useState(currentQuestion);
	const [timerClassName, setTimerClassName] = useState(style.timer);
	const [isTimeOver, setIsTimeOver] = useState(false);
	const [animationKey, setAnimationKey] = useState(0);

	const { activeLang } = useContext(LanguageContext);
	const { loggedUser } = useContext(AuthContext);
	const gameContext = useContext(GameContext);

	const timerSlideRef = useRef();

	let timerTimeoutId;

	const startTimerAnimation = useCallback(() => {
		if (timerSlideRef.current.style.animationPlayState === "paused") {
			setAnimationKey(animationKey + 1);
		}
		setTimerClassName(`${style.timer} ${style["timer-start"]}`);
	}, [timerSlideRef, animationKey]);

	const pauseTimerAnimation = () => {
		timerSlideRef.current.style.animationPlayState = "paused";
	};

	useEffect(() => {
		setQuestion(currentQuestion);
		startTimerAnimation();
		return () => {
			timerTimeoutId = setTimeout(() => {
				setIsTimeOver(true);
			}, [TIME_FOR_ANSWER]);
		};
	}, [currentQuestion]);

	useEffect(() => {
		if (isTimeOver) {
			setTimerClassName(style.timer);
			return;
		}
	}, [isTimeOver]);

	const confirmAnswer = (answer) => {
		clearTimeout(timerTimeoutId);
		pauseTimerAnimation();
		setTimeout(() => {
			nextQuestion({
				questionId: question.id,
				playerId: loggedUser.id,
				selectedAnswer: answer.text,
				answerNumber:
					gameContext.activeRound.playerAnswers.length >= 3
						? questionCounter + 4
						: questionCounter + 1,
			});
		}, [CORRECT_ANSWERS_VIEW_TIME]);
	};

	const questionText = activeLang === "pl" ? question.textPl : question.text;

	return (
		<div className={style.question}>
			<div className={style["question-content"]}>
				<p className={style["question-text"]}>{questionText}</p>
			</div>
			<div
				key={animationKey}
				className={timerClassName}
				ref={timerSlideRef}
			></div>
			<Answers
				answers={question.answers}
				confirmAnswer={confirmAnswer}
				correctAnswerId={question.correctAnswerId}
			/>
		</div>
	);
};

Question.propTypes = {
	currentQuestion: PropTypes.object,
	nextQuestion: PropTypes.func,
	questionCounter: PropTypes.number,
};

export default Question;
