import { useContext } from "react";
import style from "./MainNavigation.module.css";
import LanguageContext from "../../store/language-context";

const MainNavigation = () => {
	const langContext = useContext(LanguageContext);

	const langButtonClickHandler = (lang) => {		
		langContext.setActiveLang(lang);
	};

	return (
		<div className={style.main}>
			<div className={style.langs}>
				<button
					onClick={() => langButtonClickHandler("pl")}
					className={langContext.activeLang === "pl" ? style.active : ""}
				>
					<img
						src={
							"https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Flag_of_Poland_2.svg/800px-Flag_of_Poland_2.svg.png"
						}
						alt={"Poland flag icon"}
					/>
				</button>
				<button
					onClick={() => langButtonClickHandler("en")}
					className={langContext.activeLang === "en" ? style.active : ""}
				>
					<img
						src={
							"https://upload.wikimedia.org/wikipedia/commons/thumb/f/f2/Flag_of_Great_Britain_%281707%E2%80%931800%29.svg/2560px-Flag_of_Great_Britain_%281707%E2%80%931800%29.svg.png"
						}
						alt={"United Kingdom flag icon"}
					/>
				</button>
			</div>
		</div>
	);
};

export default MainNavigation;
