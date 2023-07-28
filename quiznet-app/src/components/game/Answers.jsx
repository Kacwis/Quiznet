import { useContext, useEffect, useState } from "react";
import style from "./Game.module.css";

import PropTypes from "prop-types";
import LanguageContext from "../../store/language-context";

const Answers = ({ answers, confirmAnswer, correctAnswerId }) => {
	const [isAnswered, setIsAnswered] = useState(false);
	const [choosenAnswer, setChoosenAnswer] = useState(null);
	const [isResultTime, setIsResultTime] = useState(false);

	const { activeLang } = useContext(LanguageContext);

	const answerClickHandler = (answer) => {
		setIsAnswered(true);
		setChoosenAnswer(answer);
		confirmAnswer(answer);
		setTimeout(() => {
			setIsResultTime(true);
		}, [1000]);
	};

	useEffect(() => {
		setIsResultTime(false);
		setIsAnswered(false);
		setChoosenAnswer(null);
	}, [answers]);

	const answersListContent = answers.map((answer) => {
		let className = style.answer;
		if (
			choosenAnswer !== null &&
			choosenAnswer.id === answer.id &&
			isAnswered &&
			!isResultTime
		) {
			className = `${className} ${style.answered}`;
		}
		if (isResultTime) {
			className = `${className} ${
				correctAnswerId === answer.id ? style.correct : style.incorrect
			}`;
		}
		return (
			<li
				key={answer.id}
				className={className}
				onClick={() => answerClickHandler(answer)}
			>
				<p>{activeLang === "pl" ? answer.textPl : answer.text}</p>
			</li>
		);
	});

	return (
		<div className={style.answers}>
			<ul>{answersListContent}</ul>
		</div>
	);
};

Answers.propTypes = {
	answers: PropTypes.array,
	confirmAnswer: PropTypes.func,
	correctAnswerId: PropTypes.number,
};
export default Answers;
