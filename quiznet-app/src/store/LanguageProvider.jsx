import { useEffect, useState } from "react";
import LanguageContext from "./language-context";
import { DICTIONARY as en } from "../constants/en";
import { DICTIONARY as pl } from "../constants/pl";

import PropTypes from "prop-types";

const LanguageProvider = ({ children }) => {
	const [activeLang, setActiveLang] = useState("en");
	const [dictionary, setDictionary] = useState(en);

	useEffect(() => {
		if (activeLang === "en") {
			setDictionary(en);
		} else if (activeLang === "pl") {
			setDictionary(pl);
		}
	}, [activeLang]);

	const setActiveLangHandler = (lang) => {
		setActiveLang(lang);
	};

	const context = {
		activeLang,
		dictionary,
		setActiveLang: setActiveLangHandler,
	};

	return (
		<LanguageContext.Provider value={context}>
			{children}
		</LanguageContext.Provider>
	);
};

LanguageProvider.propTypes = {
	children: PropTypes.node,
};

export default LanguageProvider;
