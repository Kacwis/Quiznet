import { useContext } from "react";
import style from "./MainPage.module.css";
import LanguageContext from "../../store/language-context";

const StartPanel = () => {
	const langContext = useContext(LanguageContext);

	const dict = langContext.langDict;

	return (
		<div className={style["start-panel"]}>
			<h2>{dict.startPlaying}</h2>
			<div className={style["user-options"]}>
				<div className={style["auth-options"]}>
					<button>{dict.logIn}</button>
					<button>{dict.signIn}</button>
				</div>
				<div>
					<button>{langContext.langDict.playAsQuest}</button>
				</div>
			</div>
		</div>
	);
};

export default StartPanel;
