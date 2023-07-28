import { useContext, useEffect, useState } from "react";
import style from "./Game.module.css";
import LanguageContext from "../../store/language-context";
import useHttp from "../../hooks/use-http";
import PropTypes from "prop-types";
import { getRandomCategories } from "../../api";
import CentralPanel from "../ui/CentralPanel";
import LoadingSpinner from "../ui/LoadingSpinner";

const CategorySelector = ({ setCategory }) => {
	const [categories, setCategories] = useState([]);

	const { activeLang, dictionary } = useContext(LanguageContext);

	const { status, error, data, sendRequest } = useHttp(
		getRandomCategories,
		true
	);

	useEffect(() => {
		sendRequest();
	}, []);

	useEffect(() => {
		if (status === "completed" && !error) {
			setCategories(data);
		}
	}, [status, error, data, setCategories]);

	const categoryClickHandler = (category) => {
		setCategory(category);
	};

	if (status === "pending") {
		return (
			<CentralPanel>
				<LoadingSpinner />
			</CentralPanel>
		);
	}

	const categoryButtonsContent = categories.map((cat, index) => {
		return (
			<button
				key={index}
				onClick={() => {
					categoryClickHandler(cat);
				}}
			>
				{activeLang === "pl" ? cat.namePl : cat.name}
			</button>
		);
	});

	return (
		<div className={style["cat-selector"]}>
			<h2>{dictionary.chooseCategory}</h2>
			<div className={style.categories}>{categoryButtonsContent}</div>
		</div>
	);
};

CategorySelector.propTypes = {
	setCategory: PropTypes.func,
};

export default CategorySelector;
