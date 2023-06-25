import React from "react";

const LanguageContext = React.createContext({
	activeLang: "en",
	langDict: null,
	setActiveLang: (lang) => {},
});

export default LanguageContext;
