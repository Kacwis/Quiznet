import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import style from "./GameSession.module.css";
import { faDoorOpen, faFlag } from "@fortawesome/free-solid-svg-icons";
import { Link } from "react-router-dom";

const GameSessionButtons = () => {
	const surrenderClickHandler = () => {};

	return (
		<div className={style["game-buttons"]}>
			<button onClick={surrenderClickHandler}>
				<FontAwesomeIcon icon={faFlag} size="3x" />
			</button>
			<Link to="/menu">
				<FontAwesomeIcon icon={faDoorOpen} size="3x" />
			</Link>
		</div>
	);
};

export default GameSessionButtons;
