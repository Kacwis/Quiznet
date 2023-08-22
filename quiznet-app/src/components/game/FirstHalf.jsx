import { useContext, useEffect, useState } from "react";
import style from "./Game.module.css";
import CategorySelector from "./CategorySelector";

import PropTypes from "prop-types";
import Questions from "./Questions";
import useHttp from "../../hooks/use-http";
import { getThreeRandomQuestionByCategory } from "../../api";
import GameContext from "../../store/game-context";
import { useNavigate } from "react-router-dom";

const FirstHalf = ({ setCategory, category }) => {
	const [choosenCategory, setChoosenCategory] = useState(category);
	const [questions, setQuestions] = useState(null);

	const { activeGame, activeRound, stopRound, savePlayedRound } =
		useContext(GameContext);

	const { status, error, data, sendRequest } = useHttp(
		getThreeRandomQuestionByCategory
	);

	useEffect(() => {
		setChoosenCategory(category);
	}, [category]);

	useEffect(() => {
		if (choosenCategory !== null) {
			sendRequest(choosenCategory.name);
		}
	}, [choosenCategory, sendRequest]);

	useEffect(() => {
		if (status === "completed" && !error) {
			setQuestions(data);
		}
	}, [status, data, error]);

	const saveAnswers = () => {	
		savePlayedRound();
		stopRound();
	};

	return (
		<div className={style["round-half"]}>
			{category === null && <CategorySelector setCategory={setCategory} />}
			{questions !== null && (
				<>
					<Questions questions={questions} saveAnswers={saveAnswers} />
				</>
			)}
		</div>
	);
};

FirstHalf.propTypes = {
	setCategory: PropTypes.func,
	category: PropTypes.object,
};

export default FirstHalf;
