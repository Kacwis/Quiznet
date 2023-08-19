import { useContext } from "react";
import style from "./MainPage.module.css";
import LanguageContext from "../../store/language-context";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";
import CentralPanel from "../ui/CentralPanel";

const StartPanel = () => {
	const { dictionary } = useContext(LanguageContext);

	return (
		<CentralPanel className={style["start-panel"]}>
			<h2>{dictionary.startPlaying}</h2>
			<div className={style["user-options"]}>
				<div className={style["auth-options"]}>
					<Link to="/log-in">{dictionary.logIn}</Link>
					<Link to="/sign-up">{dictionary.signUp}</Link>
				</div>
				<div className={style.or}>
					<p className={style.or}>{dictionary.or}</p>
				</div>
				<div className={style["guest-play"]}>
					<Link to="/guest">
						<div className={style["play-guest-content"]}>
							<FontAwesomeIcon icon={faUser} size="xl" />
							<p> {dictionary.playAsGuest} </p>
						</div>
					</Link>
				</div>
			</div>
		</CentralPanel>
	);
};

export default StartPanel;
