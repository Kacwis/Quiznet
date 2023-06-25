import { useEffect, useState } from "react";
import LanguageContext from "./language-context";
import { DICTIONARY as en } from "../constants/en";
import { DICTIONARY as pl } from "../constants/pl";

const LanguageProvider = (props) => {
	const [activeLang, setActiveLang] = useState("en");
	const [langDict, setLangDict] = useState(en);

	const setActiveLangHandler = (lang) => {
		setActiveLang(lang);
	};

	useEffect(() => {
		const dictionary = activeLang === "pl" ? pl : en;
		setLangDict(dictionary);
	}, [activeLang]);

	const context = {
		activeLang,
		setActiveLang: setActiveLangHandler,
		langDict,
	};

	return (
		<LanguageContext.Provider value={context}>
			{props.children}
		</LanguageContext.Provider>
	);
};

export default LanguageProvider;
