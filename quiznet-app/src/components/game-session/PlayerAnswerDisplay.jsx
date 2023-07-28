import { useContext, useEffect, useState } from "react";
import Modal from "../ui/Modal";
import style from "./GameSession.module.css";

import pt from "prop-types";
import LanguageContext from "../../store/language-context";

const PlayerAnswerDisplay = ({ currentAnswer, hide }) => {
	const [playerAnswer, setPlayerAnswer] = useState(currentAnswer);

	const { activeLang } = useContext(LanguageContext);

	const isPolishActive = activeLang === "pl";

	console.log(playerAnswer);

	useEffect(() => {
		setPlayerAnswer(currentAnswer);
	}, [currentAnswer]);

	const answersListContent = playerAnswer.question.answers.map((answer) => {
		let classname = `${style.answer}`;
		if (playerAnswer.selectedAnswer === answer.text) {
			classname = `${classname} ${style["player-answer"]}`;
		}
		classname =
			answer.id === playerAnswer.question.correctAnswerId
				? `${classname} ${style.correct}`
				: `${classname} ${style.incorrect}`;
		return (
			<div className={classname} key={answer.id}>
				<p>{isPolishActive ? answer.textPl : answer.text}</p>
			</div>
		);
	});

	return (
		<Modal>
			<div className={style["answer-display"]} onClick={hide}>
				<h2>
					{isPolishActive
						? playerAnswer.question.textPL
						: playerAnswer.question.text}
				</h2>
				<div className={style.answers}>
					<ul>{answersListContent}</ul>
				</div>
			</div>
		</Modal>
	);
};

PlayerAnswerDisplay.propTypes = {
	currentAnswer: pt.object,
	hide: pt.func,
};

export default PlayerAnswerDisplay;
