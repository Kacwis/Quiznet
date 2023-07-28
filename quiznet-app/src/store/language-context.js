import React from "react";

const LanguageContext = React.createContext({
	activeLang: "en",
	dictionary: null,
	setActiveLang: (lang) => {},
	isLoading: false,
});

export default LanguageContext;
